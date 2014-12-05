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

public class GameManager : MonoBehaviour {

    //Create a structure to hold our map attributes
    public class mapAttributes
    {
        public Square square;
        public int tileType = 0;
		public bool isHero = false;	//There is a hero on this tile
		public int x;				//These are the values in tiles of this map section
		public int y;
    }
    public static int mapX = 10;                //These variables are for the size of the map in tiles
    public static int mapY = 10;
    public static int tileX = 70; 				//These variables are for the size of each square on the board
    public static int tileY = 70;
	public static mapAttributes[,] mapArray;	//A 2D Array of the map
	private GameObject[] tileTypes;             //An array of our possible tile types.
    public GameObject terrainTile;              //A terrain tile we can assign from the editor
	public GameObject heroObject;				//A hero that can be assigned from the editor
	public Transform MainCanvas;				//Our main canvas, make UI objects a child of this to display them

	public static bool secondClick = false; 	//Have we clicked once already?
	public static int heroNum; 					//An incrementing int that gives different hero numbers to differentiate
	public static List<bool> heroClicked; 		//A list of all heroes with a boolean for if they were clicked
	public static List<Hero> currentHeroes;       //A list of all heroes' game objects
    public static int curTeam;                  //Whose turn is it?

	public static GameManager Instance;			//Creating GameManager as a Singleton

	private GameObject temp;					//Temp game object for our map array initialization
	private Hero tempHeroScript;

    void Awake()								//This method is called before Start, it's the very first thing after engine init
    {
        Instance = this;						//Declare this instantiated GameObject to be the instance
        DontDestroyOnLoad(this);				//Don't destroy this if we switch scenes. This is in reference to the Game Object holding this script
		heroNum = 0;							//We'll start our heroes numbering at 0
        curTeam = 1;                            //Teams are 1 and 2
		heroClicked = new List<bool>();			//Create a list of clicked heroes (empty)
		currentHeroes = new List<Hero>();	        //Create our list of currentHeroes (also empty)
		mapArray = new mapAttributes[mapX, mapY];	//Create an array of the game board
        tileTypes = new GameObject[10];			//Initialize our array of tile types
    }

	void Start () {								// Use this for initialization. This is called after Awake()
        //Create our initial map Array
        for (int i = 0; i < mapX; i++)
        {
			for (int j = 0; j < mapY; j++)
			{
                mapArray[i, j] = new mapAttributes();
                mapArray[i, j].tileType = 0; //Set everything to our default terrain type;
                //if (j % 5 == 1) {
                //    mapArray[i,j].isHero = true;
                //    Debug.Log("mapArray isHero at " + i + " " + j);
                //}
            }
        }

        //Create our tile types
        for (int i = 0; i < 10; i++)
        {
            //tileTypes[i] = new GameObject();
            tileTypes[i] = terrainTile;
        }
        createMap();
	}

    public Transform getMainCanvas()
    {
        return MainCanvas;
    }

    private void createMap()
    {
        //Instantiate our tiles
		for (int i = 0; i < mapX; i++)
        {
			for (int j = 0; j < mapY; j++)
			{
				mapArray[i,j].x = i;
				mapArray[i,j].y = j;
				temp = (GameObject)Instantiate(tileTypes[mapArray[i,j].tileType], 
				                                          new Vector2(((i-mapX/2)* tileX), ((j-mapY/2)* tileY)), 
				                                          	Quaternion.identity);
				temp.transform.parent = MainCanvas;
                mapArray[i, j].square = temp.GetComponent<Square>(); //Add the square script of this square to our squares.
                mapArray[i, j].square.x = i;
                mapArray[i, j].square.y = j;
            }
        }
    

		//Instantiate our map
		for (int i = 0; i < mapX; i++)
		{
			for (int j = 0; j < mapY; j++)
			{
				if (Random.Range (0,10) == 1) {
					temp = (GameObject)Instantiate(heroObject, 
					                               new Vector2(((i-mapX/2)* tileX), ((j-mapY/2)* tileY)), 
					                               Quaternion.identity);
					tempHeroScript = temp.GetComponent<Hero>();
					tempHeroScript.heroAttributes.curPosX = i;
					tempHeroScript.heroAttributes.curPosY = j;
                    tempHeroScript.heroAttributes.heroMake(2, 2); //Make archers on team 2 //WORKING HERE!!! TODO
					temp.transform.parent = MainCanvas;
                    mapArray[i, j].isHero = true;
				}
			}
		}


	}

	void Update () {		// Update is called once per frame. There is also FixedUpdate (updates w/physics) and LateUpdate
	
	}

	/*
	 * This function accepts a transform position, and then moves the hero with number hNum to that position
	 */

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
        heroAtk.Attack();
		heroDef.heroAttributes.curHealth -= (heroAtk.heroAttributes.baseDamage - heroDef.heroAttributes.baseDefense);
		if (heroDef.heroAttributes.curHealth <= 0) {
            heroDef.Die();
		}
	}

    /*
     * This method ends a turn, disabling all herores of the team specified and enabling those of the opposing team
     */
    public void endTurn()
    {
        secondClick = false; //We're resetting so we're on our first click again
        for (int i = 0; i < currentHeroes.Count ; i++)
        {
            if (currentHeroes[i].heroAttributes.team == curTeam)
            {
                currentHeroes[i].heroAttributes.active = false;
            }
            else //This only allows for a 2 player game,since it assumes anything not in team one is going to be set active
            {
                currentHeroes[i].heroAttributes.active = true;
            }
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