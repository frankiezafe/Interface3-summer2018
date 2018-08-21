using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMouse: MonoBehaviour {

    public Camera cam;

    public Material hover_mat;

    [Range(-10,10)]
    public float scroll_speed = 0.3f;

    private Material previous_mat = null;
    private GameObject previous_obj = null;
    private GameObject current_obj = null;

    private float ray_distance = 0;
    private Vector3 ray_hit_point;

    private bool grab_active = false;
    private Vector3 mouse_grab_init;
    private Vector3 grab_offset;

    private void release_previous() {

        if (previous_obj != null)
        {
            previous_obj.GetComponent<Renderer>().material = previous_mat;
            previous_obj = null;
        }

    }

    private void compute_hover()
    {

        if (grab_active) {
            return;
        }

        if (cam != null)
        {
            // reseting current_obj
            current_obj = null;
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {

                ray_hit_point = hit.point;

                current_obj = hit.transform.gameObject;
                Renderer obj_renderer = current_obj.GetComponent<Renderer>();

                ray_distance = Vector3.Distance(current_obj.transform.position, cam.transform.position);

                if (previous_obj != null && previous_obj != current_obj)
                {
                    previous_obj.GetComponent<Renderer>().material = previous_mat;
                }
                if (previous_obj != current_obj)
                {
                    previous_mat = obj_renderer.material;
                    obj_renderer.material = hover_mat;
                    previous_obj = current_obj;
                }
            }
            else
            {
                release_previous();
            }
        }

    }

    void grab_start()
    {

        //Debug.Log("grab_start");

        if (current_obj != null)
        {
            mouse_grab_init = Input.mousePosition;
            grab_offset = current_obj.transform.position - ray_hit_point;
            grab_active = true;
        }

    }

    void grab_stop()
    {

        //Debug.Log("grab_stop");

        grab_active = false;
        release_previous();

    }

    void grab_update()
    {
        if (!grab_active)
        {
            return;
        }

        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            ray_distance += scroll_speed;
        }
        else if (d < 0f)
        {
            ray_distance -= scroll_speed;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 new_pos = cam.transform.position + ( ray.direction * ray_distance ) + grab_offset;
        current_obj.transform.position = new_pos;

        //Debug.Log("grab_update");

    }

    void Start()
    {
    }

    void Update()
    {

        compute_hover();

        if (Input.GetMouseButton(0) && !grab_active)
        {
            grab_start();
        }
        else if (!Input.GetMouseButton(0) && grab_active) {
            grab_stop();
        }

        if (grab_active) {
            grab_update();
        }

    }

}
