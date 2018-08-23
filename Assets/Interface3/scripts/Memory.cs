using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memory : MonoBehaviour {

    public Text display = null;

    private Dictionary<GameObject, int> grab_count = new Dictionary<GameObject, int>();

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void object_grabbed(GameObject obj)
    {
        if (!grab_count.ContainsKey(obj))
        {
            grab_count[obj] = 1;
        }
        else {
            grab_count[obj]++;
        }

        if (display != null) {

            string txt = "";
            foreach (KeyValuePair<GameObject, int> g in grab_count) {
                txt += g.Key.name;
                if (g.Value == 1)
                {
                    txt += " first grab! [1]";
                }
                else {
                    txt += " grab count: " + g.Value.ToString();
                }
                txt += "\n";
            }

            display.text = txt;

        }
    }

}
