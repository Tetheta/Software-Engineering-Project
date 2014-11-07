using UnityEngine;
using System.Collections;

/// <summary>
/// Square is a script that is assigned to every square on our game board. 
/// It has a function for if it is clicked that will move the hero to its location
/// </summary>

public class Square : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

	/*
	 * This is called by UI Button, and it basically just checks if we need to move a hero here
	 */

    public void wasClicked()
    {
		if (GameManager.secondClick) {	//We can't select squares, so we first check if we're moving something
						for (int i = 0; i < GameManager.heroClicked.Count; i++) {	//Loop through until we find the clicked hero
								if (GameManager.heroClicked [i]) {
										GameManager.Instance.moveHero (transform, i);	//Move that hero here
										GameManager.heroClicked [i] = false;			//That hero isn't clicked anymore.
										Debug.Log ("Hero #" + i + " Clicked, now going to move hero!");
								}
						}
				}
		GameManager.secondClick = false;	//Back to the first click, reset!
        Debug.Log("Square was clicked!");
    }
}