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


	void Awake() //Called before Start
	{
			heroAttributes = GetComponent<HeroAttributes> (); //Grab this instance's hero attributes script to mess with
	}

	// Use this for initialization
	void Start ()
	{
			heroInt = GameManager.heroNum;		//Set our hero's number
			GameManager.heroNum++;				//Increment heroNums so we don't have two with the same number
			Debug.Log ("Hero #" + heroInt);		
			addHero ();							//Add our hero to the game
			heroAttributes.heroMake (1,1);      //Make a hero of default class (warrior I think)
			Debug.Log ("HeroPosX: " + heroAttributes.curPosX + "HeroPosY: " + heroAttributes.curPosY);
            heroAnimator = GetComponent<Animator>();    //Grab the animator attached to this gameObject
	}

	// Update is called once per frame
	void Update ()
	{
		//Debug.Log ("HeroPosX: " + heroAttributes.curPosX + "HeroPosY: " + heroAttributes.curPosY);	
	}

	/*
	 * This method adds a hero to our list of heroes and also adds a reference to it in heroClicked
	 */
	private void addHero()
	{
		GameManager.currentHero.Add (this);
		GameManager.heroClicked.Add (false);

	}

    public void Attack()
    {
        heroAnimator.SetTrigger("Attack");
    }

    public void Die()
    {
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

	public void wasClicked ()
	{
			if (GameManager.secondClick) {							//This is the second click

			for (int i = 0; i < GameManager.heroClicked.Count; i++)	//Loop through all of our heroes, to see if they clicked
			{
				if (GameManager.heroClicked[i] && i != heroInt)		//If a hero was clicked and it's not us... COMBAT!
				{
					//COMBAT HAPPENS HERE BECAUSE HYESS
					GameManager.heroClicked[i] = false;				//Let the GameManger know that hero is no longer clicked
					Debug.Log("Hero #" + i + " Clicked, now going to hurt hero " +heroInt + "!");
                    
					GameManager.Instance.initiateCombat(GameManager.currentHero[i], this); //Initiate combat between the attacker (currenthero[i] and this)
					GameManager.secondClick = false;				//We're no longer on the second click, reset it
				}
			}

			} else {												//First click!
					GameManager.secondClick = true;					//The next click will be the second one
					Debug.Log ("Hero " + heroInt + " was clicked!");
					GameManager.heroClicked [heroInt] = true;		//We are clicked! Add us to the clicked list
					Debug.Log ("Hero " + heroInt + " was clicked!2");
			}
	}

	public void destroyHero()
	{
		//SpecialEffectsHelper.Instance.Explosion(transform.position);	//Instantiate an explosion :o
		Destroy (gameObject);
	}
}
