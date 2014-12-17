/* Program   : NyanWars MOBA V0.0.1 
 * Author    : Travis Crumley, Dane Purkeypile, Ivan Alvarado, Misha Brajnikoff, Luke Travis, Stephen Treat, Alex Ziesmer
 * Date      : Wednesday, December 17 2014
 * Files     : CameraScroll.cs
 * Purpose   : Allows for camera zoom and movement around the map.
 * Change Log: 12/1/14 - Implemented the ability to scroll around the map and zoom in and out of the map.
 *             12/8/14 - Added a skeleton for the header.
 */
using UnityEngine;
using System.Collections;

public class CameraScroll : MonoBehaviour 
{

    float speed = 1000f;                        //The speed of scrolling
    Camera camera;                              //The camera we're manipulating

	// Use this for initialization
	void Start () 
    {
        camera = GetComponent<Camera>();        //Grabs the camera from the object this is attached to
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))            //If right click is held down
        {
                                                //We're going to adjust the transform of this game object in X and Y directions 
                                                //with respect to the mouse X and Y input inverted
                                                //This makes it feel like we're pulling the map around
            transform.position += new Vector3(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed, 0f);
        }
        camera.fieldOfView -= 10*Input.GetAxis("Mouse ScrollWheel"); //Adjust the camera's field of view when we scroll, we'll want to limit this.
    }
}
