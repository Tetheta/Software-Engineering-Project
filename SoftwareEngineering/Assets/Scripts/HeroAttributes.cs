﻿/* Program   : Medieval MOBA V0.0.1
 * Author    : Travis Crumley, Dane Purkeypile, Ivan Alvarado, Misha Brajnikoff, Luke Travis, Stephen Treat, Alex Ziesmer
 * Date      : Wednesday, December 17 2014
 * Files     : HeroAttributes.cs
 * Purpose   : Controls the attributes of all the units in the game for each team.
 * Change Log: 11/14/14 - Added this to give the units on the board some basic stats, such as health, attack, defense, and adds bounders for attack and movement.
 *             11/21/14 - Changed to movement limiters by a few ticks.
 *             12/5/14  - Added a variable to control whether or not it was a units turn to move. Changed the units movement cap again.
 *             12/7/14  - Added functionality for leveling, gaining experience, and updating hitpoints based on leveling.
 *             12/8/14  - Changed the movement cap again. Also added the skeleton for the header.
 *             12/10/14 - Added functionality to determine if a specific unit has moved or not, and added a attack range minimum and maximum value. 
 *                        Added a case for the bases statistics.   
 *             12/16/14 - Added a 5th class, Medic, to the game, and added unit attributes for it! Added case to make sure that the base does not level up.
 */


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
        heroID = GameManager.heroNum;
        GameManager.heroNum++;
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
		case 3:     //mage
			baseMaxHealth = 15;
			baseDamage = 15;
			baseDefense = 4;
			minRange = 1;
			maxRange = 2;
			moveCap = 3;
			break;
        case 4:      //LOL I R BASE GUYZ
			baseMaxHealth = 200;
			baseDamage = 0;
			baseDefense = 0;
			minRange = 1;
			maxRange = 1;
			moveCap = 0;
			break;
        case 5:   // Medic
            baseMaxHealth = 20;
			baseDamage = -12;
			baseDefense = 6;
			minRange = 1;
			maxRange = 1;
			moveCap = 4;
            break;
        default:      //Think about adding tanks and healers
            baseMaxHealth = 1;
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
            if (levelDiff < -7)
            {
                exp = exp + 10;
            } 
            else if(levelDiff < 0)
            {
                exp += (10 + levelDiff) * 5;
            }
            else
                exp = exp + 50 + levelDiff * 5;
        }
        else if(levelDiff < -8)
        {
            exp++;
        }
        else
        {
            exp = exp + 10 + levelDiff;
        }
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
            case 3:     //Mage
                baseMaxHealth += 2;
                baseDamage += 5;
                baseDefense += 2;
                curHealth += 2;
                break;
            case 4:     //LOL BASES DON'T LEVEL
                break;
            case 5:     //Medic
                baseMaxHealth += 3;
                baseDamage -= 4;
                baseDefense += 3;
                curHealth += 3;
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