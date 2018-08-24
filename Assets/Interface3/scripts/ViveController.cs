using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : SteamVR_TrackedController
{

    public Vector3 origin = new Vector3(0,0,0);
    public Quaternion direction = new Quaternion();
    //public bool vivectlr_activated = false;
    //public bool vivectlr_trigger_pressed = false;

    void Start()
    {

    }

    //public override void OnTriggerClicked(ClickedEventArgs e)
    //{
    //    vivectlr_trigger_pressed = true;
    //}

    //public override void OnTriggerUnclicked(ClickedEventArgs e)
    //{
    //    vivectlr_trigger_pressed = false;
    //}
	
	void Update () {

        base.Update();

        origin = this.GetComponent<SteamVR_TrackedObject>().transform.position;
        direction = this.GetComponent<SteamVR_TrackedObject>().transform.rotation;

    }
}
