using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMouse: MonoBehaviour {

    public Camera camera;

    public Material normal;
    public Material touched;

    private GameObject previous = null;

    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject obj = hit.transform.gameObject;
            if (obj != previous)
            {
                previous = obj;
                obj.GetComponent<Renderer>().material = touched;
            }
        }
        else if (previous != null) {
            previous.GetComponent<Renderer>().material = normal;
            previous = null;
        } 
    }

}
