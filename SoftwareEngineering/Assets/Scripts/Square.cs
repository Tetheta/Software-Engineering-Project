using UnityEngine;
using System.Collections;
//using UnityEngine.UI;

/// <summary>
/// Square is a script that is assigned to every square on our game board. 
/// It has a function for if it is clicked that will move the hero to its location
/// </summary>

public class Square : MonoBehaviour
{
    public int x, y; //Or location for this square
    private GameObject highlight;
    private int highlighted = 0;
    private bool canMoveTo = false;
    // Use this for initialization
    void Start()
    {
        foreach (Transform t in transform)
        {
            if (t.name == "Highlight")
            {
                highlight = t.gameObject; //Grab the child highlight object
                highlight.SetActive(false);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    /*
     * This function highlights a square to denote it can be clicked on and moved to
     */
    public void highlightSquare(bool value)
    {
        if (value)
            highlighted += 1;
        else
            highlighted = 0;
        highlight.SetActive(value);
        canMoveTo = value;
    }

    public int isHighlighted()
    {
        return highlighted;
    }

    /*
     * This is called by UI Button, and it basically just checks if we need to move a hero here
     */
    public void wasClicked()
    {
        if (GameManager.secondClick)
        {	//We can't select squares, so we first check if we're moving something
            for (int i = 0; i < GameManager.heroClicked.Count; i++)
            {	//Loop through until we find the clicked hero
                if (GameManager.heroClicked[i])
                {
                    if (canMoveTo)
                    {
                        GameManager.Instance.moveHero(transform, i, x, y);	//Move that hero here
                        GameManager.heroClicked[i] = false;			//That hero isn't clicked anymore.
                        Debug.Log("Hero #" + i + " Clicked, now going to move hero!");
                        for (int j = 0; j < GameManager.mapX; j++)
                        {
                            for (int k = 0; k < GameManager.mapY; k++)
                            {
                                GameManager.mapArray[j, k].square.highlightSquare(false);
                                GameManager.secondClick = false;	//Back to the first click, reset!
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Square was clicked!");
    }
}