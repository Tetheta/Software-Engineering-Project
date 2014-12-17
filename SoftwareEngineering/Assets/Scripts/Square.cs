/* Program   : NyanWars MOBA V0.0.1
 * Author    : Travis Crumley, Dane Purkeypile, Ivan Alvarado, Misha Brajnikoff, Luke Travis, Stephen Treat, Alex Ziesmer
 * Date      : Wednesday, December 17 2014
 * Files     : Square.cs
 * Purpose   : Allows for tile modificationm clickability, and highlighting.
 * Change Log: 11/3/14  - Added a script that runs for all of the fame tiles that we have on the board.
 *             11/21/14 - Added highlighting functionality and added safeguards to make sure we had units selected before selecting tiles on the map.
 *             12/8/14  - Updated the highlighting by making the function return a bool value.
 *             12/10/14 - Added the attack range tile highlighting ability.            
 */
using UnityEngine;
using System.Collections;

// Square is a script that is assigned to every square on our game board. 
// It has a function for if it is clicked that will move the hero to its location

public class Square : MonoBehaviour
{
    public int x, y;                                 //Or location for this square
    private GameObject highlight;
    private GameObject attackHighlight;
    private int highlighted = 0;
    private bool attackHighlighted = false;
    private bool canMoveTo = false;
    private bool canAttack = false;

    // Use this for initialization
    void Start()
    {
        foreach (Transform t in transform)
        {
            if (t.name == "Highlight")
            {
                highlight = t.gameObject;            //Grab the child highlight object
                highlight.SetActive(false);
            }
            else if (t.name == "attackHighlight")
            {
                attackHighlight = t.gameObject;      //Grab the child attackHighlight object
                attackHighlight.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //This functions determines which tiles are currently highlighted.
    public void highlightSquare(bool value)
    {
        if (value)
            highlighted += 1;
        else
            highlighted = 0;
        highlight.SetActive(value);
        canMoveTo = value;
    }

    
    //This function highlights a square to denote it can be clicked on and moved to.
    public int isHighlighted()
    {
        return highlighted;
    }

    //This functions highlights the squares a different color based on what the attack range is.
    public void highlightAttackSquare(bool value)
    {
       // Debug.Log("Highlight Attack in Square");
        attackHighlighted = value;
        attackHighlight.SetActive(value);
    }

    //This function determines if a tile is highlighted as an attack tile.
    public bool isAttackHighlighted()
    {
        return attackHighlighted;
    }

    //This is called by UI Button, and it basically just checks if we need to move a hero here
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
                        GameManager.heroClicked[i] = false;			        //That hero isn't clicked anymore.
                        Debug.Log("Hero #" + i + " Clicked, now going to move hero!");
                        for (int j = 0; j < GameManager.mapX; j++)
                        {
                            for (int k = 0; k < GameManager.mapY; k++)
                            {
                                GameManager.mapArray[j, k].square.highlightSquare(false);
                                GameManager.secondClick = false;	        //Back to the first click, reset!
                            }
                        }
                    }
                    else
                    {
                        GameManager.currentHeroes[i].removeAttackRange();
                        GameManager.secondClick = false;
                    }
                }
            }
        }
        Debug.Log("Square was clicked!");
    }
}