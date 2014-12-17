/* Program   : NyanWars MOBA V0.0.1
 * Author    : Travis Crumley, Dane Purkeypile, Ivan Alvarado, Misha Brajnikoff, Luke Travis, Stephen Treat, Alex Ziesmer
 * Date      : Wednesday, December 17 2014
 * Files     : LoadScene.cs
 * Purpose   : Loads our scene into the Unity Level.
 * Change Log: 10/31/14 - Added a script that allows us to load the level into the game scene.
 *             11/3/14  - Added the ability to load the scene into the unity menu by name.
 *             12/8/14  - Added header skeleton to the file.           
 */
using UnityEngine;
using System.Collections;

/*
 * This class is used to load a new scene in Unity
 * LoadLevel is called by a UI Button to load the next scene
 */
public class LoadScene : MonoBehaviour
{

	public string SceneName;	//String of the scene's name. Strings are slow but since it's only once it doesn't matter	

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

	//This function loads a unity scene by name
	public void LoadLevel () 
    {
		Application.LoadLevel(SceneName);
	}
}
