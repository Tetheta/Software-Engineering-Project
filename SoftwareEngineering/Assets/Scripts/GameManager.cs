using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public int gridX;
	public int gridY;
	public static bool secondClick = false; //Have we clicked once already
	//An incrementing int that gives different hero numbers to differentiate
    public static int heroNum;
	//A list of all heroes with a boolean for if they were clicked
    public static List<bool> heroClicked;		
	//A list of all heroes' game objects
    public static List<GameObject> currentHero; 

	//A 2D Array of the grid
	public static int[,] gridArray;

	//Creating GameManager as a Singleton
    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
		heroNum = 0;
		heroClicked = new List<bool>();
		currentHero = new List<GameObject>();
		gridArray = new int[gridX, gridY];
    }
	
	// Use this for initialization
	void Start () {

        //currentHero.Add (null);
		//heroClicked.Add (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addHero(GameObject heroObject)
	{
		currentHero.Add (heroObject);
		heroClicked.Add (false);
	}

    public void moveHero(Transform newPos, int hNum)
    {
        Debug.Log("Moving Hero " + hNum);
        currentHero[hNum].transform.position = newPos.position;
    }
}
