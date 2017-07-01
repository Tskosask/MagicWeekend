using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    private GameObject pickedUpObj;
    float power = 2f;


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

        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && pickedUpObj != null)
        {
            pickedUpObj.transform.parent = this.transform;
            pickedUpObj.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && pickedUpObj != null)
        {
            pickedUpObj.transform.parent = null;
            pickedUpObj.GetComponent<Rigidbody>().isKinematic = false;
            pickedUpObj.GetComponent<Rigidbody>().useGravity = false;

            //get controller velocity and apply it to object
            Debug.Log("rb" + controller.velocity);
            Vector3 throwVelocity = new Vector3(controller.velocity.x, controller.velocity.y, controller.velocity.z * power);
            pickedUpObj.GetComponent<Rigidbody>().velocity = throwVelocity;
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "ground") //dont pick up the ground!
        {
            pickedUpObj = col.gameObject;
            Debug.Log("trigger enter");
        }

    }

    private void OnTriggerExit(Collider col)
    {
        pickedUpObj = null;
        Debug.Log("trigger exit");
    }
}
