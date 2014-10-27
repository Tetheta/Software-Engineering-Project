using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{
	private int heroInt = 0;

	// Use this for initialization
	void Start ()
	{
			heroInt = GameManager.heroNum;
			GameManager.heroNum++;
			Debug.Log ("Hero #" + heroInt);
			GameManager.Instance.addHero (gameObject);
	}



	// Update is called once per frame
	void Update ()
	{

	}

	public void wasClicked ()
	{
			if (GameManager.secondClick) {

			for (int i = 0; i < GameManager.heroClicked.Count; i++)
			{
				if (GameManager.heroClicked[i] && i != heroInt)
				{
					GameManager.Instance.moveHero(transform, i);
					GameManager.heroClicked[i] = false;
					Debug.Log("Hero #" + i + " Clicked, now going to kill hero " +heroInt + "!");
					SpecialEffectsHelper.Instance.Explosion(transform.position);
					GameManager.secondClick = false;
					Destroy(gameObject);
				}
			}

			} else {
					GameManager.secondClick = true;
					Debug.Log ("Hero " + heroInt + " was clicked!");
					GameManager.heroClicked [heroInt] = true;
					Debug.Log ("Hero " + heroInt + " was clicked!2");
			}
	}
}
