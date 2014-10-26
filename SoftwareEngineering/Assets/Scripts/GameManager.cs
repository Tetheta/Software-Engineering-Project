using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static int heroNum = 0;
    public static List<bool> heroClicked;
    public static List<GameObject> currentHero;
    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

	public void addHero(GameObject heroObject)
	{
		currentHero.Add (heroObject);
		heroClicked.Add (false);
	}

	
	// Use this for initialization
	void Start () {
        heroClicked = new List<bool>();
        currentHero = new List<GameObject>();
        //currentHero.Add(null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void moveHero(Transform newPos, int hNum)
    {
        Debug.Log("Moving Hero");
        currentHero[hNum].transform.position = newPos.position;
    }
}
