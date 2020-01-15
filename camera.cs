/*Drake Wood
 * camera.cs
 * 
 * Camera controls, Mouse wheel zooms in and out, A and D move left and right. 
 * Follows the player otherwise. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    //camera will follow the target.
    public Transform target;

    //camera is going to be that many units from the player
    public float Height = 10;
    public float radius = 10;
    public float angle = 0;

    // make this in degrees per Second
    // Setting it to 36 = 10 seconds to fully rotate around
    public float rotationalSpeed = 120f;

    // Update is called once per frame
    void Update()
    {
        var d = Input.GetAxis("Mouse ScrollWheel"); // allows the scroll wheel to zool in an out
                                                    // moves height and radius 1:1
        if (d > 0f)
        {
            Height = Height -.5f;
            radius = radius - .5f;
        }
        else if (d < 0f)
        {
            Height = Height + .5f;
            radius = radius + .5f;
        }
        float cameraX = target.position.x + (radius * Mathf.Cos(Mathf.Deg2Rad * angle));
        float cameraY = target.position.y + Height;
        float cameraZ = target.position.z + (radius * Mathf.Sin(Mathf.Deg2Rad * angle));

        transform.position = new Vector3(cameraX, cameraY, cameraZ);

        if (Input.GetKey(KeyCode.A)) // allows A to spin the camera to the left
        {
            angle = angle - rotationalSpeed * Time.deltaTime;
        }//end of if
        else if (Input.GetKey(KeyCode.D)) // allows D to spin the camera to the left
        {
            angle = angle + rotationalSpeed * Time.deltaTime;
        }//end of else if

        transform.LookAt(target.position);

    }//end of update
}
