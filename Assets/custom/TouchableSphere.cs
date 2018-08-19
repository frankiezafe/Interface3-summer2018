using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableSphere : MonoBehaviour {

    public Material normal_mat = null;

    public Material highlight_mat = null;

    public void highlight( Vector3 dir ) {

        if (highlight_mat == null) {
            return;
        }

        this.GetComponent<Renderer>().material = highlight_mat;

        Rigidbody rb = this.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = this.transform.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
        }
        rb.velocity += dir;

    }

    public void normal()
    {

        if (normal_mat == null)
        {
            return;
        }

        this.GetComponent<Renderer>().material = normal_mat;

    }

}
