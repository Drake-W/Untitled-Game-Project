/*Drake Wood
 * ClickToMove.cs
 * 
 * Script attaches to the player and needs 2 text mesh objects and an animator set. 
 * allows the player to left click to run to a position, hold left click to follow the mouse,
 * and right click on a tree or a rock to collect it. 
 * 
 * raycast location and movement were taken from this unity wiki page:
 * https://wiki.unity3d.com/index.php/Click_To_Move_C
 * 
 */

using UnityEngine;
using System.Collections;
using TMPro;

public class ClickToMove : MonoBehaviour
{
    // Movement 
    private Transform myTransform;             // this transform
    public Vector3 destinationPosition;        // The destination Point
    public float destinationDistance;          // The distance between myTransform and destinationPosition

    public float moveSpeed;                     // The Speed the character will move
    public float maxSpeed;

    public Vector3 targetPoint;

    // Gui
    public TextMeshProUGUI rocksText;
    public int rocksCount;

    public TextMeshProUGUI woodText;
    public int woodCount;

   
    // Animator for character 
    [SerializeField] private Animator animator;

    void Start()
    {
        Debug.Log("start");
        myTransform = transform;                            // sets myTransform to this GameObject.transform
        destinationPosition = myTransform.position;         // prevents myTransform reset
        // set gui to 0
        rocksCount = 0;
        rocksText.text = "Rocks: " + rocksCount;
        woodCount = 0;
        woodText.text = "Wood: " + woodCount;
       
        animator.SetBool("Moving", false); // start animator at idle
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //right click 
        {
            // get object clicked on
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.tag == "rock") // if clicked on a rock
                {
                    Debug.Log("tag rock");
                    rocksCount++; // increment count and add to the gui counter
                    rocksText.text = "Stone: " + rocksCount;
                    hitInfo.collider.gameObject.GetComponent<Pickupobject>().tagged = true; // set a tag on the object
                    Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                    float hitdist = 0.0f;

                    // move player to the position of the object clicked on
                    if (playerPlane.Raycast(ray, out hitdist))
                    {
                        Vector3 targetPoint = ray.GetPoint(hitdist);
                        destinationPosition = ray.GetPoint(hitdist);
                        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                        myTransform.rotation = targetRotation;
                    }
                    

                }
                
                if (hitInfo.collider.gameObject.tag == "tree") // if clicked on a tree
                {
                    Debug.Log("tag tree");
                    woodCount++; // increment count and add to the gui counter
                    woodText.text = "Wood: " + woodCount;
                    hitInfo.collider.gameObject.GetComponent<Pickupobject>().tagged = true; // set a tag on the object

                    Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                    float hitdist = 0.0f;

                    // move player to the position of the object clicked on
                    if (playerPlane.Raycast(ray, out hitdist))
                    {
                        Vector3 targetPoint = ray.GetPoint(hitdist);
                        destinationPosition = ray.GetPoint(hitdist);
                        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                        myTransform.rotation = targetRotation;
                    }


                }
            }
            
             
        }
        
        // keep track of the distance between this gameObject and destinationPosition
        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        if (destinationDistance < .5f)
        {       // To prevent shaking behavior when near destination, still happens 
            moveSpeed = 0;
            animator.SetBool("Moving", false);
        }
        else if (destinationDistance > .5f)
        {           // To Reset Speed to default
                    // currently the only speed 
            moveSpeed = maxSpeed;
            animator.SetBool("Moving", true);
        }


        // Moves the Player if the Left Mouse Button was clicked
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            animator.SetBool("Moving", true); // sets animator to play running animation

            Plane playerPlane = new Plane(Vector3.up, myTransform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            // move player to the clicken of location
            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPosition = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                myTransform.rotation = targetRotation;
            }
            
        }

        // Moves the player if the mouse button is hold down
        else if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0)
        {

            Plane playerPlane = new Plane(Vector3.up, myTransform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPosition = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                myTransform.rotation = targetRotation;
            }
            //	myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, moveSpeed * Time.deltaTime);
            animator.SetBool("Moving", true);
        }

        // To prevent code from running if not needed
        if (destinationDistance > .5f)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, maxSpeed * Time.deltaTime);
        }

        
    }
}