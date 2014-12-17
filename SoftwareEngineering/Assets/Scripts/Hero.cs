/* Program   : NyanWars MOBA V0.0.1
 * Author    : Travis Crumley, Dane Purkeypile, Ivan Alvarado, Misha Brajnikoff, Luke Travis, Stephen Treat, Alex Ziesmer
 * Date      : Wednesday, December 17 2014
 * Files     : Hero.cs
 * Purpose   : Control unit movement, selection, attack, and deaths
 * Change Log: 12/1/14  - Added functionality to highlight the tiles of the map based on what is returned from the step counter.
 *             12/5/14  - Added some basic turn determiners, which team has control of the board and their respective pieces.
 *             12/8/14  - Improved the speed of the move counter that we have. Added recursive functionality and improved it to be speedy.
 *             12/10/14 - Updated the recursive movement function and modified some of the if statements to work with it.  
 *                        Added non-working attack range code. Changed boundary condition on our max attack range loop.
 *             12/12/14 - Edited the hasClicked method to try and streamline and debug.
 *             12/16/14 - Rolled back some implementation that was not working to a previous version that was working.
 *             
 */
using UnityEngine;
using System.Collections;

//This class is a generic hero class that creates a hero in our gameManager and can react to it being clicked
public class Hero : MonoBehaviour
{
    public HeroAttributes heroAttributes;		                    //Attributes of this hero
    private Animator heroAnimator;
    private GameObject temp;
    public GameObject highlight;

    //Called before Start
    void Awake() 
    {
        heroAttributes = GetComponent<HeroAttributes>();            //Grab this instance's hero attributes script to mess with
    }

    // Use this for initialization
    void Start()
    {
        addHero();							                        //Add our hero to the game
        Debug.Log("HeroPosX: " + heroAttributes.curPosX + "HeroPosY: " + heroAttributes.curPosY);
        heroAnimator = GetComponent<Animator>();                    //Grab the animator attached to this gameObject
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //This method adds a hero to our list of heroes and also adds a reference to it in heroClicked
    private void addHero()
    {
        GameManager.currentHeroes.Add(this);
        GameManager.heroClicked.Add(false);
    }

    /*
     * This recursive function moves the hero, it steps out in a direction, highlights that if there is no obstacle, 
     * and then moves from there recursively to check all possible moves.
     * Input: an int for the maximum movement
     * Output: The hero moves, but nothing is returned
     */
    public void Move(int x, int y, int moveCap)
    {
        if (heroAttributes.hasMoved == false)
        {
            GameManager.mapArray[x, y].square.highlightSquare(true); //Added so that you can click on the hero again to move back to that spot
            if (moveCap > 0)
            {
                if (x > 0 && GameManager.mapArray[x - 1, y].square.isHighlighted() < 2)
                {
                    StartCoroutine(MoveRecursive(x - 1, y, moveCap - 1));
                }
                if (y > 0 && GameManager.mapArray[x, y - 1].square.isHighlighted() < 2)
                {
                    StartCoroutine(MoveRecursive(x, y - 1, moveCap - 1));
                }
                if ((x < GameManager.mapX - 1) && GameManager.mapArray[x + 1, y].square.isHighlighted() < 2)
                {
                    StartCoroutine(MoveRecursive(x + 1, y, moveCap - 1));
                }
                if ((y < GameManager.mapY - 1) && GameManager.mapArray[x, y + 1].square.isHighlighted() < 2)
                {
                    StartCoroutine(MoveRecursive(x, y + 1, moveCap - 1));
                }
            }
        }
    }

    //This function recursively controls the generation of our step counter
    IEnumerator MoveRecursive(int x, int y, int moveCap)
    {
        yield return new WaitForSeconds(0.001f);                     //Wait before continuing
        //Mark the square we're selecting
        if (GameManager.mapArray[x, y] != null)
        {
            if (!GameManager.mapArray[x, y].isHero) 
            {
                GameManager.mapArray[x, y].square.highlightSquare(true);
                //Move
                if (moveCap > 0)
                {
                    if (x > 0 && GameManager.mapArray[x - 1, y].square.isHighlighted() < 2)
                    {
                        StartCoroutine(MoveRecursive(x - 1, y, moveCap - 1));
                    }
                    if (y > 0 && GameManager.mapArray[x, y - 1].square.isHighlighted() < 2)
                    {
                        StartCoroutine(MoveRecursive(x, y - 1, moveCap - 1));
                    }
                    if ((y < GameManager.mapY - 1) && GameManager.mapArray[x, y + 1].square.isHighlighted() < 2)
                    {
                        StartCoroutine(MoveRecursive(x, y + 1, moveCap - 1));
                    }
                    if ((x < GameManager.mapX - 1) && GameManager.mapArray[x + 1, y].square.isHighlighted() < 2)
                    {
                        StartCoroutine(MoveRecursive(x + 1, y, moveCap - 1));
                    }
                }
            }
        }
    }

    //Starts the animation for the sprite attack 
    public void Attack()
    {
        heroAnimator.SetTrigger("Attack");
    }

    //Pulls up the attack ranges of the specific units, based on their class identifiers.
    public void attackRange() 
    {
        int HEROLOCATIONX = heroAttributes.curPosX;
        int HEROLOCATIONY = heroAttributes.curPosY;
        //This function uses 2 for loops and 4 if statements to find and .square.highlightAttackSquare(true);the correct squares
        Debug.Log("Entered attackRange Function");
        int sum = 0;

        for (int i = 1; i <= heroAttributes.maxRange; i++)
        {
            //Debug.Log("Entered attackRange Function first for loop");
            sum = 0;
            for (int j = 0; sum <= heroAttributes.maxRange; j++)
            {
               // Debug.Log("Entered attackRange Function second for loop");
                sum = i + j;
                if (sum <= heroAttributes.maxRange && sum >= heroAttributes.minRange)
                {
                    if ((HEROLOCATIONX + i < GameManager.mapX) &&
                        (HEROLOCATIONY + j < GameManager.mapY) &&
                        !GameManager.mapArray[HEROLOCATIONX + i, HEROLOCATIONY + j].square.isAttackHighlighted())//Check first Quadrant
                    {
                        GameManager.mapArray[HEROLOCATIONX + i, HEROLOCATIONY + j].square.highlightAttackSquare(true);
                    }
                    if ((HEROLOCATIONX - j >= 0) &&
                        (HEROLOCATIONY + i < GameManager.mapY) &&
                        !GameManager.mapArray[HEROLOCATIONX - j, HEROLOCATIONY + i].square.isAttackHighlighted())//Check Second Quadrant
                    {
                        GameManager.mapArray[HEROLOCATIONX - j, HEROLOCATIONY + i].square.highlightAttackSquare(true);
                    }
                    if ((HEROLOCATIONX - i >= 0) &&
                        (HEROLOCATIONY - j >= 0) &&
                        !GameManager.mapArray[HEROLOCATIONX - i, HEROLOCATIONY - j].square.isAttackHighlighted())//Check Third Quadrant
                    {
                        GameManager.mapArray[HEROLOCATIONX - i, HEROLOCATIONY - j].square.highlightAttackSquare(true);
                    }
                    if ((HEROLOCATIONX + j < GameManager.mapX) &&
                        (HEROLOCATIONY - i >= 0) &&
                        !GameManager.mapArray[HEROLOCATIONX + j, HEROLOCATIONY - i].square.isAttackHighlighted())//Check Fourth Quadrant
                    {
                        GameManager.mapArray[HEROLOCATIONX + j, HEROLOCATIONY - i].square.highlightAttackSquare(true);
                    }
                }
            }
        }
    }

    //Removes the highlighted tiles from the map once the units turn is complete
    public void removeAttackRange()
    {
        for (int i = 0; i < GameManager.mapX; i++)
        {
            for (int j = 0; j < GameManager.mapY; j++)
            {
                if (GameManager.mapArray[i, j].square.isAttackHighlighted())
                {
                    GameManager.mapArray[i, j].square.highlightAttackSquare(false);
                }
            }
        }
    }

    //Starts the animation for the unit dying
    public void Die()
    {
        GameManager.mapArray[heroAttributes.curPosX, heroAttributes.curPosY].isHero = false;
        heroAnimator.SetTrigger("Death");
        StartCoroutine(waitforDeathAnimation());
    }

    //Makes the program wait for the death animation to finish before another move to be made.
    IEnumerator waitforDeathAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        destroyHero();
    }

    /*
     * This function is used by the UI Buttons class to make the hero react to being clicked.
     * If it's the second click, the hero moves to the desired location and potentially destroys an opponent
     * If it's the first click, the hero is simply selected and put into the heroClicked list.
     */
    public void wasClicked()
    {
        Debug.Log("Hero " + heroAttributes.heroID + " was Clicked");
        if (GameManager.secondClick)
        {	//This is the second click
            Debug.Log("Second Click");
            if (GameManager.mapArray[heroAttributes.curPosX, heroAttributes.curPosY].square.isAttackHighlighted())
            {
                Debug.Log("Square is highlighted");
                for (int i = 0; i < GameManager.heroClicked.Count; i++)	                         //Loop through all of our heroes, to see if they clicked
                {
                    if (GameManager.heroClicked[i] && i != heroAttributes.heroID)		         //If a hero was clicked and it's not us... COMBAT!
                    {
                        //Combat happens here
                        GameManager.heroClicked[i] = false;				                         //Let the GameManger know that hero is no longer clicked
                        Debug.Log("Hero #" + i + " Clicked, now going to hurt hero " + heroAttributes.heroID + "!");
                                                                                                 //Need to check to see if we're in range here
                        GameManager.Instance.initiateCombat(GameManager.currentHeroes[i], this); //Initiate combat between the attacker (currenthero[i] and this)
                        GameManager.currentHeroes[i].removeAttackRange();
                        for (int j = 0; j < GameManager.mapX; j++)
                        {
                            for (int k = 0; k < GameManager.mapY; k++)
                            {
                                GameManager.mapArray[j, k].square.highlightSquare(false);
                            }
                        }
                        GameManager.secondClick = false;				                         //We're no longer on the second click, reset it
                    }
                }
            }
            else
            {
                removeAttackRange();
                GameManager.secondClick = false;				                                 //We're no longer on the second click, reset it
                for (int i = 0; i < GameManager.heroClicked.Count; i++)	                         //Loop through all of our heroes, to reset things
                {
                    GameManager.heroClicked[i] = false;
                }
            }
        }
        else if (!heroAttributes.hasAttacked)                                                    //Make sure we can be clicked, aka we haven't attacked yet
        {												//First click!

            if (heroAttributes.team > 2)
            {
                //Do nothing, we're a neutral mob!
            }
            else if (heroAttributes.baseDamage == 0)
            {
                //I'm the base! 
            }
            else
            {
                GameManager.secondClick = true;                                                   //The next click will be the second one
                Debug.Log("Hero " + heroAttributes.heroID + " was clicked!");
                GameManager.heroClicked[heroAttributes.heroID] = true;                            //We are clicked! Add us to the clicked list

                if (heroAttributes.hasMoved)
                {
                    attackRange();                                                                //Set up our possible attack range
                }
                else
                {
                    Move(heroAttributes.curPosX, heroAttributes.curPosY, heroAttributes.moveCap); //Start trying to move this hero
                    heroAttributes.hasMoved = true;                                               //Should be set after physical movement
                }
            }

        }
    }

    //Removes a dead hero from the map, after the dying animation has completed
    public void destroyHero()
    {
        Destroy(gameObject);
    }
}
