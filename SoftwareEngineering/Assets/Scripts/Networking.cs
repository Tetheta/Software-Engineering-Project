/* Program   : Networking.cs  
 * Author    : Travis Crumley, Dane Purkeypile, Ivan Alvarado, Misha Brajnikoff, Luke Travis, Stephen Treat, Alex Ziesmer
 * Date      : Wednesday, December 17 2014
 * Files     : The files are all linked through the Unity Utility
 * Purpose   :
 * Change Log: 
 */

using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour
{
    HostData[] hostData;
    string gameName = "CPTR345-Networking-Prototype";
    bool refreshing = false;
    float btnX;
    float btnY;
    float btnWidth;
    float btnHeight;

    // Use this for initialization
    void Start()
    {
        btnX = Screen.width * 0.05f;
        btnY = Screen.width * 0.05f;
        btnWidth = Screen.width * 0.2f;
        btnHeight = Screen.width * 0.05f;
    }

    /* This function starts up a server and registers that server to a host. It also makes sure that the connections that are coming in 
     * are not public addresses and asserts that they are false.
     * The function also registers the game to the initialized server, so people will be able to see that name when trying to make a connection.
     */
    void startServer()
    {
        Network.InitializeServer(32, 25004, !Network.HavePublicAddress());
        MasterServer.RegisterHost(gameName, "Prototype Networking", "This is a test, did it work?");
    }

    /* This refreshes the connection to the host to check and see if there are currently any games available.
     */
    void refreshHostList()
    {
        MasterServer.RequestHostList(gameName);
        refreshing = true;
    }


    //    														Debugging Messages


    /* This function lets the user know that the instantiation of a server was successful. 
     * It piggybacks off of the "Network.InitializeServer" call.
     */
    void OnServerInitialized()
    {
        Debug.Log("Server Initialization Successful");
    }

    /* This function checks to see if the server was registered to the Unity Master Server successfully or not.
     */
    void OnMasterServerEvent(MasterServerEvent mse)
    {
        if (mse == MasterServerEvent.RegistrationSucceeded)
        {
            Debug.Log("Server Registration Successful");
        }

    }

    /* This function creates buttons that a user can click to perform specific functionalities. One button will allow the user to create a match that other
     * users should be able to join. The other button will allow the user to refresh the list of servers that people will be able to view.
     * Debug messages are displayed in order to show that the button was selected.
     */
    void OnGUI()
    {
        // if (Network.isClient && !Network.isServer)
        //{
        if (GUI.Button(new Rect(btnX, btnY, btnWidth, btnHeight), "Start Server"))
        {
            Debug.Log("Starting Server...");
            startServer();
        }

        //This creates a button that will eventually allow the user to start a server, or in our case, create a game
        if (GUI.Button(new Rect(btnX, btnY * 1.2f + btnHeight, btnWidth, btnHeight), "Refresh"))
        {
            Debug.Log("Refreshing...");
            refreshHostList();
        }

        /* This creates a button for the game after you have created it and have clicked the refresh hosts button.
         * Allows a person to "connect" to another game, can't do much with this yet.
         */

        /* This checks to see if there is a game that currently exists in the HostData array.
         */
        if (hostData != null)
        {
            // This loops over the array to make sure that the game name is updated if it is changed.
            for (int i = 0; i < hostData.Length; i++)
            {
                //This creates the clickable button for the user created server name.
                if (GUI.Button(new Rect(btnX * 1.5f + btnWidth, btnY * 1.2f + (btnHeight * i), btnWidth * 3.0f, btnHeight * 0.5f), hostData[i].gameName))
                {
                    Network.Connect(hostData[i]);
                }
            }
        }
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (refreshing)
        {
            if (MasterServer.PollHostList().Length > 0)
            {
                refreshing = false;
                Debug.Log(MasterServer.PollHostList().Length);
                hostData = MasterServer.PollHostList();
            }
        }

    }
}
