using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgiveRotor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * 10, new Vector3(0, 0, 1));
        //Debug.Log(this.transform.rotation);

    }

}
