using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catser : MonoBehaviour
{

    [Range(-100, 100)]
    public float rot_speed = 10;
    public Vector3 rot_axis = new Vector3(1, 0, 0);
    public GameObject axis = null;
    public GameObject hit_sphere = null;

    void Start()
    {

        if (hit_sphere != null)
        {
            hit_sphere.SetActive(false);
        }

    }

    void Update()
    {

        rot_axis.Normalize();
        Quaternion q = Quaternion.AngleAxis(Time.deltaTime * rot_speed, rot_axis);
        this.transform.rotation *= q;
        Vector3 forward = this.transform.rotation * new Vector3(0, 1, 0);

        if (axis != null)
        {
            Vector3 up = new Vector3(0, 1, 0);
            q = Quaternion.FromToRotation(up, rot_axis);
            axis.transform.rotation = q;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward, out hit, Mathf.Infinity))
        {
            //Debug.DrawRay(transform.position, forward * hit.distance, Color.red);
            //Debug.Log("Did Hit");
            if (hit_sphere != null)
            {
                hit_sphere.SetActive(true);
                hit_sphere.transform.position = transform.position + forward * hit.distance;
            }

            GameObject obj = hit.transform.gameObject;
            TouchableSphere ts = obj.GetComponent<TouchableSphere>();
            if (ts != null) {
                ts.highlight(forward * 4);
            }

        }
        else
        {

            if (hit_sphere != null)
            {
                hit_sphere.SetActive(false);
            }

        }
    }

}
