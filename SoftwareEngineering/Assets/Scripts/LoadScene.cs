using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	public string SceneName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadLevel () {
		Application.LoadLevel(SceneName);
	}
}
