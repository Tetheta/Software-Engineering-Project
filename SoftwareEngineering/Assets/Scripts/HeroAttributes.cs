using UnityEngine;
using System.Collections;

public class HeroAttributes : MonoBehaviour {
	
	public int baseMaxHealth;   //Maximum health
	public int baseDamage;      //How hard the hero hits
	public int baseDefense;    //How much the hero resists taking damage
	public int heroClass; //use int identifiers for the different classes and use the classes to dictate other stats
	public int minRange; //can attack adjacent
	public int maxRange; //can attack from AFLAC
	public int level, exp; //level and exp
	public int moveCap;  //max spaces to walk per turn
	public int team;   // team identifier
	public int heroID;  //which hero it is
    public bool hasAttacked; //Is it this unit's turn atm?
    public bool hasMoved; //Has this unit moved yet?

	public int curHealth; //Current health of the player
	public int curPosX; 
	public int curPosY;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void heroMake(int whichClass, int whichTeam)
	{
		heroClass = whichClass;
		team = whichTeam;
		switch(heroClass)
		{
		case 1:     //Warrior
			baseMaxHealth = 25;
			baseDamage = 15;
			baseDefense = 8;
			minRange = 1;
			maxRange = 1;
			moveCap = 4;
			break;
		case 2:     //Archer
			baseMaxHealth = 20;
			baseDamage = 12;
			baseDefense = 6;
			minRange = 2;
			maxRange = 2;
			moveCap = 3;
			break;
		case 3:     //mage?
			baseMaxHealth = 15;
			baseDamage = 15;
			baseDefense = 4;
			minRange = 1;
			maxRange = 2;
			moveCap = 2;
			break;
		default:      //LOL I R BASE GUYZ
			baseMaxHealth = 200;
			baseDamage = 1;
			baseDefense = 1;
			minRange = 1;
			maxRange = 1;
			moveCap = 1;
			break;
		}
		level = 1;
		exp = 0;
		curHealth = baseMaxHealth; //Set our current health to our max
        if(team == 1)
        {
            hasAttacked = false;
        }
	}

    public void getExp(int levelDiff, bool kill)
    {
        if (kill)
        {
            if (levelDiff < 0)
                exp = exp + (10 - levelDiff) * 5;
            else
                exp = exp + 50 + levelDiff * 5;
        }
        else
            exp = exp + 10 + levelDiff;
        if (exp > 99)
        {
            exp = exp - 100;
            levelUp();
        }
    }

    public void levelUp()
    {
        switch (heroClass)
        {
            case 1:     //Warrior
                baseMaxHealth += 3;
                baseDamage += 4;
                baseDefense += 3;
                curHealth += 3;
                break;
            case 2:     //Archer
                baseMaxHealth += 2;
                baseDamage += 4;
                baseDefense += 3;
                curHealth += 2;
                break;
            case 3:     //mage?
                baseMaxHealth += 2;
                baseDamage += 5;
                baseDefense += 2;
                curHealth += 2;
                break;
            default:      //lulz unit
                baseMaxHealth = 1;
                baseDamage = 0;
                baseDefense = 0;
                break;
        }
        level += 1;
        if (curHealth > baseMaxHealth)
        {
            curHealth = baseMaxHealth;
        }
    }
}