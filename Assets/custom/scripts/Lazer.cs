using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : SteamVR_TrackedController {

    //private SteamVR_TrackedController ctrlr;
    //private SteamVR_TrackedObject trkobj;

    public GameObject beam = null;
    [Range(-180, 180)]
    public float beam_angle = 0;
    public Vector3 beam_offset = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start () {

        //ctrlr = GetComponent<SteamVR_TrackedController>();
        //trkobj = GetComponent<SteamVR_TrackedObject>();

        if (beam != null) {
            beam.SetActive(false);
        }

    }

    public override void OnTriggerClicked(ClickedEventArgs e)
    {
        //if (TriggerClicked != null)
        //    TriggerClicked(this, e);
        if (beam != null) {
            beam.SetActive(true);
        }
    }

    public override void OnTriggerUnclicked(ClickedEventArgs e)
    {
        //if (TriggerClicked != null)
        //    TriggerClicked(this, e);
        if (beam != null)
        {
            beam.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {

        base.Update();

        if (beam != null && beam.active)
        {
            Vector3 pos = this.GetComponent<SteamVR_TrackedObject>().transform.position;
            Quaternion q = this.GetComponent<SteamVR_TrackedObject>().transform.rotation;
            Vector3 xaxis = new Vector3(1,0,0);
            xaxis = q * xaxis;
            beam.transform.rotation = q * Quaternion.AngleAxis(beam_angle, new Vector3(1, 0, 0));
            Vector3 boff = new Vector3(beam_offset.x, beam_offset.y, beam_offset.z);
            boff = beam.transform.rotation * beam_offset;
            beam.transform.position = pos + boff;
        }

    }
}
