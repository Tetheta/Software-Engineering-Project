using UnityEngine;
using System.Collections;

/*
 * This class is a generic hero class that creates a hero in our gameManager and can react to it being clicked
 */

public class Hero : MonoBehaviour
{
	private int heroInt = 0;					//An int to keep track of this hero

	// Use this for initialization
	void Start ()
	{
			heroInt = GameManager.heroNum;		//Set our hero's number
			GameManager.heroNum++;				//Increment heroNums so we don't have two with the same number
			Debug.Log ("Hero #" + heroInt);		
			addHero ();							//Add our hero to the game
	}

	// Update is called once per frame
	//void Update ()
	//{
	//
	//}

	/*
	 * This method adds a hero to our list of heroes and also adds a reference to it in heroClicked
	 */
	private void addHero()
	{
		GameManager.currentHero.Add (gameObject);	
		GameManager.heroClicked.Add (false);

	}

	/*
	 * This function is used by the UI Buttons class to make the hero react to being clicked.
	 * If it's the second click, the hero moves to the desired location and potentially destroys an opponent
	 * If it's the first click, the hero is simply selected and put into the heroClicked list.
	 */

	public void wasClicked ()
	{
			if (GameManager.secondClick) {							//This is the second click

			for (int i = 0; i < GameManager.heroClicked.Count; i++)	//Loop through all of our heroes, to see if they clicked
			{
				if (GameManager.heroClicked[i] && i != heroInt)		//If a hero was clicked and it's not us...
				{
					GameManager.Instance.moveHero(transform, i);	//Move that other hero
					GameManager.heroClicked[i] = false;				//Let the GameManger know that hero is no longer clicked
					Debug.Log("Hero #" + i + " Clicked, now going to kill hero " +heroInt + "!");
					SpecialEffectsHelper.Instance.Explosion(transform.position);	//Instantiate an explosion :o
					GameManager.secondClick = false;				//We're no longer on the second click, reset it
					Destroy(gameObject);							//We got destroyed by hero i, destroy this hero.
				}
			}

			} else {												//First click!
					GameManager.secondClick = true;					//The next click will be the second one
					Debug.Log ("Hero " + heroInt + " was clicked!");
					GameManager.heroClicked [heroInt] = true;		//We are clicked! Add us to the clicked list
					Debug.Log ("Hero " + heroInt + " was clicked!2");
			}
	}
}
