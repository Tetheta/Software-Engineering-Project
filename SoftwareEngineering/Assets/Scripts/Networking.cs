using UnityEngine;
using System.Collections;
//DO NOT RUN THIS AT ALL COSTS. For some reason their is a loop that causes you to refresh the host and check teh connection 4 million times a second
//I don't know why, but I can't test it because it will no longer let me connect.

public class Networking : MonoBehaviour {
	string gameName = "CPTR345-Networking-Prototype";
	bool  refreshing = false;
	float btnX;
	float btnY;
	float btnWidth;
	float btnHeight;

	// Use this for initialization
	void Start () {
		btnX = Screen.width * 0.05f;
		btnY = Screen.width * 0.05f;
		btnWidth = Screen.width * 0.2f;
		btnHeight = Screen.width *0.05f;
	}

	/* This function starts up a server and registers that server to a host. It also makes sure that the connections that are coming in 
	 * are not public addresses and asserts that they are false.
	 * The function also registers the game to the initialized server, so people will be able to see that name when trying to make a connection.
	 */
	void startServer() {
		Network.InitializeServer (32, 25004, !Network.HavePublicAddress());
		MasterServer.RegisterHost (gameName, "Prototype Networking", "This is a test, did it work?");
	}

	void refreshHostList (){
		MasterServer.RequestHostList (gameName);
		refreshing = true;
		//Debug.Log(MasterServer.PollHostList().Length);
	}

	//    														Debugging Messages


	/* This function lets the user know that the instantiation of a server was successful. 
	 * It piggybacks off of the "Network.InitializeServer" call.
	 */ 
	void OnServerInitialized (){
		Debug.Log ("Server Initialization Successful");
	}

	/* This function checks to see if the server was registered to the Unity Master Server successfully or not.
	 * 
	 * 
	 */
	void OnMasterServerEvent(MasterServerEvent mse){
		if (mse == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log("Server Registration Successful");
		}

	}

	/* This function creates buttons that a user can click to perform specific functionalities. One button will allow the user to create a match that other
	 * users should be able to join. The other button will allow the user to refresh the list of servers that people will be able to view.
	 * Debug messages are displayed in order to show that the button was selected.
	 */
	void OnGUI () {
		if (GUI.Button (new Rect (btnX, btnY, btnWidth, btnHeight), "Start Server")) {
						Debug.Log ("Starting Server...");
						startServer();
				}

	//This function creates a button that will eventually allow the user to start a server, or in our case, create a game
		if (GUI.Button(new Rect(btnX, btnY *1.2f + btnHeight, btnWidth, btnHeight), "Refresh"))
			Debug.Log ("Refreshing...");
			refreshHostList ();
	
	}

	// Update is called once per frame
	void Update () {
		if(refreshing){
			if(MasterServer.PollHostList().Length > 0){
				refreshing = false;
				Debug.Log(MasterServer.PollHostList ().Length);
			}
		}
		
	}
}
