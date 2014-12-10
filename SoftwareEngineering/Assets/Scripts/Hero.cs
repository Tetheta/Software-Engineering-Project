using UnityEngine;
using System.Collections;

/*
 * This class is a generic hero class that creates a hero in our gameManager and can react to it being clicked
 */

public class Hero : MonoBehaviour
{
    private int heroInt = 0;					//An int to keep track of this hero
    public HeroAttributes heroAttributes;		//Attributes of this hero
    private Animator heroAnimator;
    private GameObject temp;
    public GameObject highlight;


    void Awake() //Called before Start
    {
        heroAttributes = GetComponent<HeroAttributes>(); //Grab this instance's hero attributes script to mess with
    }

    // Use this for initialization
    void Start()
    {
        heroInt = GameManager.heroNum;		//Set our hero's number
        GameManager.heroNum++;				//Increment heroNums so we don't have two with the same number
        Debug.Log("Hero #" + heroInt);
        addHero();							//Add our hero to the game
        //heroAttributes.heroMake (1,1);      //Make a hero of default class (warrior I think) || DOING THIS NOW IN GAMEMANAGER
        Debug.Log("HeroPosX: " + heroAttributes.curPosX + "HeroPosY: " + heroAttributes.curPosY);
        heroAnimator = GetComponent<Animator>();    //Grab the animator attached to this gameObject
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log ("HeroPosX: " + heroAttributes.curPosX + "HeroPosY: " + heroAttributes.curPosY);	
    }

    /*
     * This method adds a hero to our list of heroes and also adds a reference to it in heroClicked
     */
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
    public void Move(int x, int y, int moveCap) //We have an issue here where the x and y values are not linked up somehow with the right squares
    {
        if (heroAttributes.hasMoved == false)
        {
            if (moveCap > 0)
            {
                //Whichever of these next two if statements comes first determines where we have the hole. The 2nd one has a hole in it for some reason
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
            heroAttributes.hasMoved = true;
        }
        
    }
   IEnumerator MoveRecursive(int x, int y, int moveCap)
    {
        yield return new WaitForSeconds(0.001f); //Wait for two seconds before continuing
        //Mark the square we're selecting
        if (GameManager.mapArray[x, y] != null)
        {
            if (!GameManager.mapArray[x, y].isHero) //TROUBLE BREWS, FEAR YE WHO ENTER
            {
                Debug.Log("Highlight square " + GameManager.mapArray[x, y].square.x + ", " + GameManager.mapArray[x, y].square.y);
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

    public void Attack()
    {
        heroAnimator.SetTrigger("Attack");
    }

    public void Die()
    {
        GameManager.mapArray[heroAttributes.curPosX, heroAttributes.curPosY].isHero = false;
        heroAnimator.SetTrigger("Death");
        StartCoroutine(waitforDeathAnimation());
    }

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

        if (GameManager.secondClick)
        {							//This is the second click

            for (int i = 0; i < GameManager.heroClicked.Count; i++)	//Loop through all of our heroes, to see if they clicked
            {
                if (GameManager.heroClicked[i] && i != heroInt)		//If a hero was clicked and it's not us... COMBAT!
                {
                    //COMBAT HAPPENS HERE BECAUSE HYESS
                    GameManager.heroClicked[i] = false;				//Let the GameManger know that hero is no longer clicked
                    Debug.Log("Hero #" + i + " Clicked, now going to hurt hero " + heroInt + "!");
                    //Need to check to see if we're in range here
                    GameManager.Instance.initiateCombat(GameManager.currentHeroes[i], this); //Initiate combat between the attacker (currenthero[i] and this)
                    for (int j = 0; j < GameManager.mapX; j++)
                    {
                        for (int k = 0; k < GameManager.mapY; k++)
                        {
                            GameManager.mapArray[j, k].square.highlightSquare(false);
                        }
                    }
                    GameManager.secondClick = false;				//We're no longer on the second click, reset it
                }
            }

        }
        else if (heroAttributes.active) //Make sure we can be clicked
        {												//First click!
            GameManager.secondClick = true;					//The next click will be the second one
            Debug.Log("Hero " + heroInt + " was clicked!");
            GameManager.heroClicked[heroInt] = true;		//We are clicked! Add us to the clicked list
            Debug.Log("Hero " + heroInt + " was clicked!2");

            Move(heroAttributes.curPosX, heroAttributes.curPosY, heroAttributes.moveCap); //Start trying to move this hero
        }
    }

    public void destroyHero()
    {
        //SpecialEffectsHelper.Instance.Explosion(transform.position);	//Instantiate an explosion :o
        Destroy(gameObject);
    }
}
