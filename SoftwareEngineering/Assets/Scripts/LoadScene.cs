/* Program   : LoadScene.cs  
 * Author    : Travis Crumley, Dane Purkeypile, Ivan Alvarado, Misha Brajnikoff, Luke Travis, Stephen Treat, Alex Ziesmer
 * Date      : Wednesday, December 17 2014
 * Files     : The files are all linked through the Unity Utility
 * Purpose   :
 * Change Log: 
 */

using UnityEngine;
using System.Collections;

/*
 * This class is used to load a new scene in Unity
 * LoadLevel is called by a UI Button to load the next scene
 */

public class LoadScene : MonoBehaviour {

	public string SceneName;	//String of the scene's name. Strings are slow but since it's only once it doesn't matter	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	 * This function loads a unity scene by name
	 */

	public void LoadLevel () {
		Application.LoadLevel(SceneName);
	}
}
