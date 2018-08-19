using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour {

    [Range(1,20)]
    public int y_steps = 5;

    [Range(1, 20)]
    public int equator_steps = 10;

    [Range(0.1f, 20)]
    public float inner_radius = 3;

    [Range(0.1f, 2)]
    public float sphere_radius = 0.5f;

    public Material normal_mat = null;

    public Material highlight_mat = null;

    private List<GameObject> all_spheres;

    void Start () {

        all_spheres = new List<GameObject>();

        float asteps = Mathf.PI * 2 / equator_steps;

        for (int y = -y_steps; y <= y_steps; ++y)
        {

            for (float a = 0; a < Mathf.PI * 2; a += asteps) {

                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.parent = this.transform;
                sphere.transform.position = new Vector3(
                    Mathf.Cos(a) * inner_radius ,
                    (y *1f / y_steps) * inner_radius,
                    Mathf.Sin(a) * inner_radius
                    );
                sphere.transform.localScale = new Vector3(sphere_radius, sphere_radius, sphere_radius);
                sphere.AddComponent<SphereCollider>();

                //Rigidbody rb = sphere.AddComponent<Rigidbody>();
                //rb.useGravity = false;

                TouchableSphere ts = sphere.AddComponent<TouchableSphere>();
                ts.normal_mat = normal_mat;
                ts.highlight_mat = highlight_mat;

                if (normal_mat != null)
                {
                    sphere.GetComponent<Renderer>().material = normal_mat;
                }

            }

        }

    }

}
