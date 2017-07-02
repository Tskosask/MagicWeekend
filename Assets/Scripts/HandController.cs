using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    private GameObject earthShieldObject;

    private SteamVR_TrackedObject trackedObj;

    private GameObject pickedUpObj;
    float power = 2f;

    public GameObject earthShield;
    private bool holdingObject;

    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (controller == null)
        {
            Debug.Log("Controller Initalization failed.");
            return;
        }

        //pick up object
        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && pickedUpObj != null)
        {

            if (pickedUpObj.tag == "ground")
            {
                CreateEarthShield();
                
            }

            holdingObject = true;
            pickedUpObj.transform.parent = this.transform;
            

            pickedUpObj.GetComponent<Rigidbody>().isKinematic = true;
        }

        //put down object / throw
        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {


            if (pickedUpObj != null)
            {
                Debug.Log(pickedUpObj.transform.name);

                pickedUpObj.transform.parent = null;
                holdingObject = false;
                if (pickedUpObj.transform.name == "EarthShield 1(Clone)") //the shield stays where you put it
                {
                    //check velocity to see if they are trying to throw it or move the shield
                    Debug.Log("velocity  " + controller.velocity);

                    float stayingVelocity = 1.5f;

                    if (controller.velocity.x > stayingVelocity || controller.velocity.y > stayingVelocity || controller.velocity.z > stayingVelocity ||
                        controller.velocity.x < -(stayingVelocity) || controller.velocity.y < -(stayingVelocity) || controller.velocity.z < -(stayingVelocity))
                    {
                        pickedUpObj.tag = "magicProjectile";
                        ThrowObject();
                    }
                }
                else
                {
                    ThrowObject();
                }
            }

        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (holdingObject == false)
        {
            pickedUpObj = col.gameObject;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (holdingObject == false) //dont change things if they are still holding an object
        {
             pickedUpObj = null;
        }
    }

    void CreateEarthShield()
    {
        earthShieldObject = Instantiate(earthShield, new Vector3(transform.position.x, transform.position.y, transform.position.z), new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
        pickedUpObj = earthShieldObject;
        earthShieldObject = null; //dont need the earth sheild anymore 
    }

    void ThrowObject()
    {
        pickedUpObj.GetComponent<Rigidbody>().isKinematic = false;

        float velocityStr = 1;

        if (pickedUpObj.tag == "magicProjectile") //magic projectiles are not affects by gravity and have more power
        {
            velocityStr = power;
            pickedUpObj.GetComponent<Rigidbody>().useGravity = false;
            Destroy(pickedUpObj, 15); //clean up the object after 15 seconds
        }

        //get controller velocity and apply it to object
        Vector3 throwVelocity = new Vector3(controller.velocity.x, controller.velocity.y, controller.velocity.z * velocityStr);
        //Quaternion throwRotation = transform.rotation;
        //pickedUpObj.GetComponent<Rigidbody>().rotation = throwRotation;
        pickedUpObj.GetComponent<Rigidbody>().velocity = throwVelocity;

    }
}
