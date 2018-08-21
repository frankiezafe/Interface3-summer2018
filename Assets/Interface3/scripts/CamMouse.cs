using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMouse: MonoBehaviour {

    public Camera cam;

    public Material hover_mat;

    [Range(-10,10)]
    public float scroll_speed = 0.3f;

    public string grabbable_tag = "";

    public bool debug = false;

    private Material previous_mat = null;
    private GameObject previous_obj = null;
    private GameObject current_obj = null;

    private float ray_distance = 0;
    private Vector3 ray_hit_point;
    private Vector3 ray_offset;

    private bool grab_active = false;
    private Vector3 mouse_grab_init;

    private GameObject hit_debug;

    private GameObject generate_sphere() {

        GameObject sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sph.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        SphereCollider bc = sph.GetComponent<SphereCollider>();
        if ( bc != null ) {
            Destroy(bc);
        }

        return sph;

    }

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
                if ( debug )
                {
                    hit_debug.transform.position = ray_hit_point;
                }
                current_obj = hit.transform.gameObject;
                ray_offset = current_obj.transform.position - ray_hit_point;
                ray_distance = Vector3.Distance(current_obj.transform.position, cam.transform.position);

                Renderer obj_renderer = current_obj.GetComponent<Renderer>();

                if (current_obj.tag != grabbable_tag)
                {
                    current_obj = null;
                }

                if (previous_obj != null && previous_obj != current_obj)
                {
                    release_previous();
                }

                if (current_obj != null && previous_obj != current_obj)
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
        Vector3 new_pos = cam.transform.position + ( ray.direction * ray_distance ) + ray_offset;
        current_obj.transform.position = new_pos;

        //Debug.Log("grab_update");

    }

    void Start()
    {
        if (debug)
        {
            hit_debug = generate_sphere();
        }
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
