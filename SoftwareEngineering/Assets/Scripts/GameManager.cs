using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static bool secondClick = false;
    public static int heroNum;
    public static List<bool> heroClicked;
    public static List<GameObject> currentHero;
    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
		heroNum = 0;
		heroClicked = new List<bool>();
		currentHero = new List<GameObject>();
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
