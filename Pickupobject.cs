/*Drake Wood
 * pickupobject.cs
 * 
 * script attaches to objects that need to be picked up. 
 * Being right clicked by the player will set a tag on the object.
 * When the player moves to and collides with an object that is tagged
 * it will play a pikcup animation and set the object as not active. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupobject : MonoBehaviour
{
    public bool tagged;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        tagged = false; // make sure that the tag is false at start
    }

    // Update is called once per frame
    void Update()
    {
        //nothing happens
    }

    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        //if the player collides with this then start to get picked up when it is tagged
        if ((collision.gameObject.tag == "Player") && (tagged))
        {   
            //stop walking on collision
            //without stopping when hitting an object the player will shake
            collision.gameObject.GetComponent<ClickToMove>().destinationPosition = collision.gameObject.transform.position;
            collision.gameObject.GetComponent<ClickToMove>().targetPoint = collision.gameObject.transform.position;

            //plays pickup animation
            collision.gameObject.GetComponent<Animator>().SetTrigger("Pickup");
            
            //start waiting after animation is played
            StartCoroutine(waiter()); 

        }
        else
        {
            //stop walking on collision, this is used when left click to move, not hold
            //stops the player because there is no pathing around objects 
            collision.gameObject.GetComponent<ClickToMove>().destinationPosition = collision.gameObject.transform.position;
            collision.gameObject.GetComponent<ClickToMove>().targetPoint = collision.gameObject.transform.position;
        }
    }

    // let the pickup animation play for .5 seconds before settings the object as not active
    IEnumerator waiter()
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(.5f);
        Debug.Log("Done Waiting");
        gameObject.SetActive(false);
    }
}
