/* Program   : NyanWars MOBA V0.0.1
 * Author    : Travis Crumley, Dane Purkeypile, Ivan Alvarado, Misha Brajnikoff, Luke Travis, Stephen Treat, Alex Ziesmer
 * Date      : Wednesday, December 17 2014
 * Files     : GameManager.cs
 * Purpose   : Controls the start up of the game and instantiates objects for the game.
 * Change Log: 10/26/14 - Added a function that allows us to create a hero and make it a game object.
 *             10/31/14 - Added variables that allow for control of the units that have been added to the game.
 *             11/3/14  - Added a function that creates a map as an array, and allows that map to have 
 *                        specific different types of attributes. Also, commented some of the code.
 *             11/7/14  - Finished implementing the array that creates the map that the heroes move around on.
 *             11/10/14 - Added functionality that allows us to determine if a hero is in a specific tile on the map.
 *             11/14/14 - Added a very basic implementation of the function that is going to allow the units to engage in combat with one another. UNITS CAN KILL OTHER UNITS!
 *             11/21/14 - Implemented the functionality that highlights the map tiles from the step counter, and finished implementing the step counter.
 *             12/5/14  - Implemented teams and started turn based functionality, and got a basis for it working.
 *             12/7/14  - Implemented an experience and leveling system for the units.
 *             12/8/14  - Added the ability to not attack teammates and added the ability to choose the type of units a team consists of.
 *             12/10/14 - Implemented unit spawning onto the map. Also began working on placing those units into specific tiles of the map. 
 *                        Came up with a diagonal based unit placement orientation.  
 *             12/16/14 - Added neautral enemies that people can kill for experience, global experience for all characters per turn, and added 
 *                        a victory screen.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * This class is designed as a manager of all game functions and activities
 * It is declared as a Singleton here, which means it is never destroyed and can act as
 * A liason between different scenes and objects of the game.
 * 
 * The public functions it holds can be accessed through this singleton, while the
 * static variables can be accessed directly by calling GameManager.variable
 */
public class GameManager : MonoBehaviour
{

    //Create a structure to hold our map attributes
    public class mapAttributes
    {
        public Square square;
        public int tileType = 0;
        public bool isHero = false;	                //There is a hero on this tile
        public int x;				                //These are the values in tiles of this map section
        public int y;
    }
    public static int mapX = 10;                    //These variables are for the size of the map in tiles
    public static int mapY = 10;
    public static int tileX = 70; 				    //These variables are for the size of each square on the board
    public static int tileY = 70;
    public static mapAttributes[,] mapArray;	    //A 2D Array of the map
    private GameObject[] tileTypes;                 //An array of our possible tile types.
    public GameObject terrainTile;                  //A terrain tile we can assign from the editor
    public GameObject heroObject;				    //A hero that can be assigned from the editor
    public Transform MainCanvas;				    //Our main canvas, make UI objects a child of this to display them
    public GameObject VictoryScreen;                //Our victory screen menu thing
    public static bool secondClick = false; 	    //Have we clicked once already?
    public static int heroNum; 					    //An incrementing int that gives different hero numbers to differentiate
    public static List<bool> heroClicked; 		    //A list of all heroes with a boolean for if they were clicked
    public static List<Hero> currentHeroes;         //A list of all heroes' game objects
    public static int curTeam;                      //Whose turn is it?

    //Variables for unit selection/etc
    public static int[] unitSelection1;             //An array to hold the units selected by Team 1
    public static int[] unitSelection2;             //An array to hold the units selected by Team 2
    private int teamSel, unitSel;                   //Ints to hold what we're selecting


    public static GameManager Instance;			    //Creating GameManager as a Singleton

    private GameObject temp;					    //Temp game object for our map array initialization
    private Hero tempHeroScript;

    void Awake()								    //This method is called before Start, it's the very first thing after engine init
    {
        Instance = this;						    //Declare this instantiated GameObject to be the instance
        DontDestroyOnLoad(this);				    //Don't destroy this if we switch scenes. This is in reference to the Game Object holding this script
        heroNum = 0;							    //We'll start our heroes numbering at 0
        curTeam = 1;                                //Teams are 1 and 2
        heroClicked = new List<bool>();			    //Create a list of clicked heroes (empty)
        currentHeroes = new List<Hero>();	        //Create our list of currentHeroes (also empty)
        mapArray = new mapAttributes[mapX, mapY];	//Create an array of the game board
        tileTypes = new GameObject[10];			    //Initialize our array of tile types
    }

    //Begins a game with specified unit types and places them on the map.
    void Start()
    {								                // Use this for initialization. This is called after Awake()
        VictoryScreen.SetActive(false);
        //Create our initial map Array
        for (int i = 0; i < mapX; i++)
        {
            for (int j = 0; j < mapY; j++)
            {
                mapArray[i, j] = new mapAttributes();
                mapArray[i, j].tileType = 0;        //Set everything to our default terrain type;
            }
        }

        //Create our tile types
        for (int i = 0; i < 10; i++)
        {
            tileTypes[i] = terrainTile;
        }
        unitSelection1 = new int[5] { 2, 3, 2, 1, 2};//Order here is Warrior, Archer, Mage, Base, Medic
        unitSelection2 = new int[5] { 2, 2, 4, 1, 2};
        createMap();
    }

    //Pulls up the board!
    public Transform getMainCanvas()
    {
        return MainCanvas;
    }

    //Generates the map, and places 
    private void createMap()
    {
        //Instantiate our tiles
        for (int i = 0; i < mapX; i++)
        {
            for (int j = 0; j < mapY; j++)
            {
                mapArray[i, j].x = i;
                mapArray[i, j].y = j;
                temp = (GameObject)Instantiate(tileTypes[mapArray[i, j].tileType],
                                                          new Vector2(((i - mapX / 2) * tileX), ((j - mapY / 2) * tileY)),
                                                            Quaternion.identity);
                temp.transform.parent = MainCanvas;
                mapArray[i, j].square = temp.GetComponent<Square>(); //Add the square script of this square to our squares.
                mapArray[i, j].square.x = i;
                mapArray[i, j].square.y = j;
            }
        }
        //Start team 1 placement-------------------------------------
        teamSel = 1;
        int xPlace = 0;
        int xFurthest = 0;
        int yPlace = 0;
        while ((unitSelection1[0] + unitSelection1[1] + unitSelection1[2] + unitSelection1[3] + unitSelection1[4]) != 0)
        {
            if (xPlace == 1 && yPlace == 1)
            {
                {
                    unitSel = 4;
                    unitSelection1[3] -= 1;
                }
            }
            else if (unitSelection1[4] > 0)
            {
                unitSel = 5;
                unitSelection1[4] -= 1;
            }
            else if (unitSelection1[2] > 0)
            {
                unitSel = 3;
                unitSelection1[2] -= 1;
            }
            else if (unitSelection1[1] > 0)
            {
                unitSel = 2;
                unitSelection1[1] -= 1;
            }
            else if (unitSelection1[0] > 0)
            {
                unitSel = 1;
                unitSelection1[0] -= 1;
            }
            temp = (GameObject)Instantiate(heroObject,
                                                   new Vector2(((xPlace - mapX / 2) * tileX), ((yPlace - mapY / 2) * tileY)),
                                                   Quaternion.identity);
            tempHeroScript = temp.GetComponent<Hero>();
            tempHeroScript.heroAttributes.curPosX = xPlace;
            tempHeroScript.heroAttributes.curPosY = yPlace;
            tempHeroScript.heroAttributes.heroMake(unitSel, teamSel);
            temp.transform.parent = MainCanvas;
            mapArray[xPlace, yPlace].isHero = true;

            if (xPlace == 0) //Placement starts at 0,0, then 1,0, 0,1, then 2,0, 1,1, 0,2, etc...
            {
                xFurthest++;
                xPlace = xFurthest;
                yPlace = 0;
            }
            else
            {
                yPlace++;
                xPlace--;
            }
        }
        //Start team 2 placement----------------------------------------
        teamSel = 2;
        xPlace = mapX - 1;
        yPlace = mapY - 1;
        xFurthest = mapX - 1;
        while ((unitSelection2[0] + unitSelection2[1] + unitSelection2[2] + unitSelection2[3] + unitSelection2[4]) != 0)
        {
            if (xPlace == (mapX - 2) && yPlace == (mapY - 2))
            {
                {
                    unitSel = 4;
                    unitSelection2[3] -= 1;
                }
            }
            else if (unitSelection2[4] > 0)
            {
                unitSel = 5;
                unitSelection2[4] -= 1;
            }
            else if (unitSelection2[2] > 0)
            {
                unitSel = 3;
                unitSelection2[2] -= 1;
            }
            else if (unitSelection2[1] > 0)
            {
                unitSel = 2;
                unitSelection2[1] -= 1;
            }
            else if (unitSelection2[0] > 0)
            {
                unitSel = 1;
                unitSelection2[0] -= 1;
            }
            temp = (GameObject)Instantiate(heroObject,
                                                   new Vector2(((xPlace - mapX / 2) * tileX), ((yPlace - mapY / 2) * tileY)),
                                                   Quaternion.identity);
            tempHeroScript = temp.GetComponent<Hero>();
            tempHeroScript.heroAttributes.curPosX = xPlace;
            tempHeroScript.heroAttributes.curPosY = yPlace;
            tempHeroScript.heroAttributes.heroMake(unitSel, teamSel);
            temp.transform.parent = MainCanvas;
            mapArray[xPlace, yPlace].isHero = true;

            if (xPlace == mapX - 1)
            {
                xFurthest--;
                xPlace = xFurthest;
                yPlace = mapY - 1;
            }
            else
            {
                yPlace--;
                xPlace++;
            }
        }
        //Neutral mob placement----------------------------------------------
        for (int i = mapX - 1; i >= 0; i--)
        {
            temp = (GameObject)Instantiate(heroObject,
                                                   new Vector2(((i - mapX / 2) * tileX), (((mapY - 1 - i) - mapY / 2) * tileY)),
                                                   Quaternion.identity);
            tempHeroScript = temp.GetComponent<Hero>();
            tempHeroScript.heroAttributes.curPosX = i;
            tempHeroScript.heroAttributes.curPosY = mapY - 1 - i;
            tempHeroScript.heroAttributes.heroMake(1, 3);
            temp.transform.parent = MainCanvas;
            mapArray[i, mapY - 1 - i].isHero = true;
        }
    }

    void Update()
    {		

    }

    //This function accepts a transform position, and then moves the hero with number hNum to that position
    public void moveHero(Transform newPos, int hNum, int x, int y)
    {
        Debug.Log("Moving Hero " + hNum);
        GameManager.mapArray[currentHeroes[hNum].heroAttributes.curPosX, currentHeroes[hNum].heroAttributes.curPosY].isHero = false;
        GameManager.mapArray[x, y].isHero = true;
        currentHeroes[hNum].heroAttributes.curPosX = x;
        currentHeroes[hNum].heroAttributes.curPosY = y;
        currentHeroes[hNum].transform.position = newPos.position;
    }

    /*
     * This method initiates combat between two heroes, an attacking hero and a defending hero.
     * Inputs: A Hero Script "heroAtk" and a Hero Script "heroDef"
     */
    public void initiateCombat(Hero heroAtk, Hero heroDef)
    {
        Debug.Log("Hero " + heroAtk.heroAttributes.heroID + " Attacking Hero " + heroDef.heroAttributes.heroID);
        if (heroAtk.heroAttributes.baseDamage < 0)                                                      //Checks for if the attack is a medic
        {
            if ((heroAtk.heroAttributes.team == heroDef.heroAttributes.team) && (heroDef.heroAttributes.curHealth != heroDef.heroAttributes.baseMaxHealth))
            {
                heroAtk.Attack();
                heroDef.heroAttributes.curHealth -= heroAtk.heroAttributes.baseDamage;                  //Defense doesn't apply when getting healed
                heroAtk.heroAttributes.getExp(-3, true);                                                //Applies a set xp amount per heal
                if(heroDef.heroAttributes.curHealth > heroDef.heroAttributes.baseMaxHealth)
                {
                    heroDef.heroAttributes.curHealth = heroDef.heroAttributes.baseMaxHealth;            //Eliminates overhealing
                }
            }
        }
        else if (heroAtk.heroAttributes.team != heroDef.heroAttributes.team)
        {
            heroAtk.Attack();
            heroDef.heroAttributes.curHealth -= (heroAtk.heroAttributes.baseDamage - heroDef.heroAttributes.baseDefense);
            int levelDiff = heroDef.heroAttributes.level - heroAtk.heroAttributes.level;
            if (heroDef.heroAttributes.curHealth <= 0)
            {
                if (heroDef.heroAttributes.baseDefense == 0)
                {
                    //We're the base, end the game!
                    Debug.Log("Game Over");
                    VictoryScreen.SetActive(true);
                }
                heroDef.Die();
                heroAtk.heroAttributes.getExp(levelDiff, true);
            }
            else
            {
                heroAtk.heroAttributes.getExp(levelDiff, false);
            }
            heroAtk.heroAttributes.hasAttacked = true;
        }
        heroAtk.removeAttackRange();
    }

    //This method ends the game and resets it (called from victory button)
    public void resetGame()
    {
        Application.LoadLevel("Board");
    }

    //This method ends a turn, disabling all herores of the team specified and enabling those of the opposing team 
    public void endTurn()
    {
        secondClick = false; //We're resetting so we're on our first click again
        for (int i = 0; i < currentHeroes.Count; i++)
        {
            if (currentHeroes[i].heroAttributes.team == curTeam)
            {
                currentHeroes[i].heroAttributes.hasAttacked = true;
                currentHeroes[i].heroAttributes.hasMoved = true;
            }
            else //This only allows for a 2 player game,since it assumes anything not in team one is going to be set active
            {
                currentHeroes[i].heroAttributes.hasAttacked = false;
                currentHeroes[i].heroAttributes.hasMoved = false;
                if(currentHeroes[i].heroAttributes.team == 3)
                {
                    currentHeroes[i].heroAttributes.getExp(-7, true);
                }
                else
                {
                    currentHeroes[i].heroAttributes.getExp(-4, true);
                }
            }
        }
        //Reset our heroes clicked array
        for (int i = 0; i < heroClicked.Count; i++)
        {
            heroClicked[i] = false;
        }
        if (curTeam == 1)
        {
            curTeam = 2;
        }
        else
        {
            curTeam = 1;
        }
    }
}