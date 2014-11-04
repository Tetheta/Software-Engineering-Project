using UnityEngine;
using System.Collections;
using System.Collections.Generic;



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
        public int tileType = 0;
    }

    public static int mapX = 10;                //These variables are for the size of the map in tiles
    public static int mapY = 10;
    public static int tileX = 50; 				//These variables are for the size of each square on the board
    public static int tileY = 50;
    public static mapAttributes[,] mapArray = new mapAttributes[mapX, mapY];	//A 2D Array of the map
    private GameObject[] tileTypes = new GameObject[10];             //An array of our possible tile types.
    public GameObject terrainTile;              //A terrain tile we can assign from the editor

	public static bool secondClick = false; 	//Have we clicked once already?
	public static int heroNum; 					//An incrementing int that gives different hero numbers to differentiate
	public static List<bool> heroClicked; 		//A list of all heroes with a boolean for if they were clicked
	public static List<GameObject> currentHero; //A list of all heroes' game objects

	public static GameManager Instance;			//Creating GameManager as a Singleton

    void Awake()								//This method is called before Start, it's the very first thing after engine init
    {
        Instance = this;						//Declare this instantiated GameObject to be the instance
        DontDestroyOnLoad(this);				//Don't destroy this if we switch scenes. This is in reference to the Game Object holding this script
		heroNum = 0;							//We'll start our heroes numbering at 0
		heroClicked = new List<bool>();			//Create a list of clicked heroes (empty)
		currentHero = new List<GameObject>();	//Create our list of currentHeroes (also empty)
		//mapArray = new mapAttributes[mapX, mapY];		//Create an array of the game board
        //tileTypes = new GameObject[10];
    }

	void Start () {								// Use this for initialization. This is called after Awake()
        //Create our initial map Array
        mapArray[0, 0] = new mapAttributes();
        mapArray[0, 0].tileType = 0;
        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < 1; j++)
            {
                //mapArray[i, j] = new mapAttributes();
                mapArray[i, j].tileType = 0; //Set everything to our default terrain type;
            }
        }

        //Create our tile types
        for (int i = 0; i < 10; i++)
        {
            tileTypes[i] = new GameObject();
            tileTypes[i] = terrainTile;
        }
        createMap();
	}

    private void createMap()
    {
        //Instantiate our map
        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < 1; i++)
            {
                Instantiate(terrainTile, new Vector2(0, 0), Quaternion.identity);
                //Instantiate(tileTypes[mapArray[i, j].mapType], new Vector2(i * tileX, j * tileY), Quaternion.identity); 
            }
        }
    }

	void Update () {							// Update is called once per frame. There is also FixedUpdate (updates w/physics) and LateUpdate
	
	}

	/*
	 * This function accepts a transform position, and then moves the hero with number hNum to that position
	 */

    public void moveHero(Transform newPos, int hNum)
    {
        Debug.Log("Moving Hero " + hNum);
        currentHero[hNum].transform.position = newPos.position;
    }
}