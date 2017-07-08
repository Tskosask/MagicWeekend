using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    private GameObject earthShieldObject;

    private SteamVR_TrackedObject trackedObj;

    private GameObject pickedUpObj;
    float power = 3f;

    public GameObject earthShield;
    public GameObject fireSpell;
    public GameObject waterSpell;
    public GameObject airSpell;

    private bool holdingObject;

    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if (controller == null)
        {
            Debug.Log("Controller Initalization failed.");
            return;
        }

        //pick up object
        //if they are pressing the trigger, there is an item to pick up
        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && pickedUpObj != null)
        {
            if (pickedUpObj.tag == "ground")
            {
                CreateEarthShield();
            }
            else if (pickedUpObj.tag == "firesource")
            {
                CreateFireSpell();
            }
            else if (pickedUpObj.tag == "watersource")
            {
                CreateWaterSpell();
            }
            else if (pickedUpObj.tag == "airsource")
            {
                CreateAirSpell();
            }

            //only able to grab an object if it has a rigid body
            if (pickedUpObj.gameObject.GetComponent<Rigidbody>() != null)
            {
                holdingObject = true;
                //connect it to the controller  (the grab)
                pickedUpObj.transform.parent = this.transform;
                pickedUpObj.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        //put down object / throw
        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (pickedUpObj != null && pickedUpObj.tag != "noPickup") //you cannot pick up debris
            {
                pickedUpObj.transform.parent = null;
                holdingObject = false;

                if (pickedUpObj.transform.name == "EarthShield(Clone)") //the shield stays where you put it
                {
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
        if (holdingObject == false && col.gameObject.tag != "noPickup") //dont pick up debris or if you are holding something
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

    void ThrowObject()
    {
        pickedUpObj.GetComponent<Rigidbody>().isKinematic = false;

        float velocityStr = 1; 

        if (pickedUpObj.tag == "magicProjectile") //magic projectiles are not affects by gravity and have more power
        {
            velocityStr = power;
            pickedUpObj.GetComponent<Rigidbody>().useGravity = false;
            if (pickedUpObj.name == "WaterSpell(Clone)")
            {
                pickedUpObj.GetComponent<ParticleSystem>().Stop();
            }

            //throwing magic objects will make the controller vibrate
            controller.TriggerHapticPulse(3999);

            Destroy(pickedUpObj, 10); //clean up the object after 15 seconds
        }

        //get controller velocity and apply it to object
        Vector3 throwVelocity = controller.velocity * velocityStr;

        pickedUpObj.GetComponent<Rigidbody>().velocity = throwVelocity;
        pickedUpObj = null; //get ready to pick up next object
    }

    //ricoceting fire ball
    void CreateFireSpell()
    {
        pickedUpObj = Instantiate(fireSpell, transform.position, transform.rotation);
        controller.TriggerHapticPulse(3999);
    }

    //spout style water particle spell
    void CreateWaterSpell()
    {
        controller.TriggerHapticPulse(3999);
        pickedUpObj = Instantiate(waterSpell, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x + 45, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
    }

    //creates a throwable shield of earth
    void CreateEarthShield()
    {
        Vector3 shieldSize = earthShield.GetComponent<Renderer>().bounds.size;

        //spawn the shield in the middle of the hand
        Vector3 shieldPosition = new Vector3(transform.position.x - (shieldSize.y / 2), transform.position.y - (shieldSize.x), transform.position.z);

        controller.TriggerHapticPulse(3999);

        earthShieldObject = Instantiate(earthShield, shieldPosition, new Quaternion(90, 90, 0, 0));
        pickedUpObj = earthShieldObject;
        earthShieldObject = null; //dont need the earth sheild anymore 
    }

    //creates a deflection only air spell
    void CreateAirSpell()
    {
        pickedUpObj = Instantiate(airSpell, transform.position, transform.rotation);
        controller.TriggerHapticPulse(3999);
    }
}
