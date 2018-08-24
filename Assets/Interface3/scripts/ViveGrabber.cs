using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViveGrabber : MonoBehaviour {

    public GameObject viverig;
    public ViveController vivectlr;
    public Material hover_mat;
    [Range(-10, 10)]
    public float scroll_speed = 0.3f;

    public string grabbable_tag = "";
    public string jump_tag = "";

    public bool debug = false;
    public GameObject msg_board = null;
    public Text msg_txt = null;
    public LineRenderer msg_line = null;
    public Vector3 msg_offset = new Vector3(0.25f, 0.25f, -0.5f);

    public AudioClip soundclip = null;
    public AudioSource soundplayer = null;

    public Memory memory = null;

    private Material previous_mat = null;
    private GameObject previous_obj = null;
    private GameObject current_obj = null;

    private float ray_distance = 0;
    private Vector3 ray_hit_point;
    private Vector3 ray_offset;
    private Vector3 ray_space;
    private Vector3 ray_rel_hit;

    private bool grab_test = false;
    private bool grab_active = false;
    private Vector3 mouse_grab_init;

    private GameObject hit_debug;

    private List<string> msgs = new List<string> { "joli!", "bien joué!", "houuuuu!", "splendide!" };
    private int msg_index = 0;

    private GameObject generate_sphere()
    {

        GameObject sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sph.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        SphereCollider bc = sph.GetComponent<SphereCollider>();
        if (bc != null)
        {
            Destroy(bc);
        }

        return sph;

    }

    private void release_previous()
    {

        if (previous_obj != null)
        {
            Renderer obj_renderer = previous_obj.GetComponent<Renderer>();

            if (obj_renderer != null && hover_mat != null)
            {
                previous_obj.GetComponent<Renderer>().material = previous_mat;
            }
            if (grab_test)
            {
                Rigidbody rb = previous_obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = new Vector3(0, 0, 0);
                }
            }
            previous_obj = null;
        }

    }

    private void compute_hover()
    {

        if (grab_active)
        {
            return;
        }

        if (vivectlr != null)
        {
            // reseting current_obj
            current_obj = null;
            RaycastHit hit;

            //Ray ray = vivectlr.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(vivectlr.origin, vivectlr.direction * Vector3.forward * 1000, Color.yellow);

            if (Physics.Raycast(vivectlr.origin, vivectlr.direction * Vector3.forward, out hit))
            {
                
                ray_hit_point = hit.point;
                if (debug)
                {
                    hit_debug.transform.position = ray_hit_point;
                }

                current_obj = hit.transform.gameObject;
                ray_distance = Vector3.Distance(current_obj.transform.position, vivectlr.origin);
                ray_offset = hit.point - current_obj.transform.position;

                Renderer obj_renderer = current_obj.GetComponent<Renderer>();

                if (current_obj.tag == jump_tag && vivectlr.padPressed)
                {
                    viverig.transform.position = current_obj.transform.position;
                }

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
                    if (obj_renderer != null && hover_mat != null)
                    {
                        previous_mat = obj_renderer.material;
                        obj_renderer.material = hover_mat;
                    }
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

            if (memory != null)
            {
                memory.object_grabbed(current_obj);
            }

            if (msg_board != null)
            {
                msg_board.SetActive(true);
                if (msg_txt != null)
                {
                    msg_txt.text = msgs[msg_index];
                    msg_index = (++msg_index) % msgs.Count;
                }
            }

            if (soundplayer != null && soundclip != null)
            {
                soundplayer.PlayOneShot(soundclip, 0.8f);
            }

        }

    }

    void grab_stop()
    {

        //Debug.Log("grab_stop");

        grab_active = false;
        release_previous();

        if (msg_board != null)
        {
            msg_board.SetActive(false);
        }

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
            ray_space.z += scroll_speed;
        }
        else if (d < 0f)
        {
            ray_distance -= scroll_speed;
            ray_space.z -= scroll_speed;
        }

        Vector3 new_pos = vivectlr.origin + vivectlr.direction * Vector3.forward * ray_distance - ray_offset;
        current_obj.transform.position = new_pos;

        if (debug)
        {
            hit_debug.transform.position = new_pos;
        }

        if (msg_board != null)
        {

            msg_board.transform.position = new_pos + (vivectlr.transform.rotation * msg_offset);

            if (msg_line != null)
            {
                Vector3 mlp = msg_line.transform.position;
                msg_line.SetPosition(0, new_pos - mlp);
                msg_line.SetPosition(1, new Vector3(0, 0, 0));
            }

        }

    }

    void Start()
    {
        if (debug)
        {
            hit_debug = generate_sphere();
        }

        if (msg_board != null)
        {
            msg_board.SetActive(false);
        }

    }

    void Update()
    {

        if (vivectlr == null) {
            return;
        }

        compute_hover();

        if (vivectlr.triggerPressed && !grab_active && !grab_test)
        {
            grab_test = true;
            grab_start();
        }
        else if (!vivectlr.triggerPressed && grab_test)
        {
            grab_stop();
            grab_test = false;
        }

        if (grab_active)
        {
            grab_update();
        }

    }

}
