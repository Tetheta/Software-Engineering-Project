using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

    private int heroInt = 0;

	// Use this for initialization
	void Start () {
        //GameManager.currentHero.Add(gameObject);
		heroInt = GameManager.heroNum;
		GameManager.heroNum++;
		Debug.Log("Hero #" + heroInt);
		GameManager.Instance.addHero (gameObject, heroInt);
        //GameManager.heroClicked.Add(false);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void wasClicked()
    {
        Debug.Log("Hero " + heroInt + " was clicked!");
        GameManager.heroClicked[heroInt] = true;
    }
}
