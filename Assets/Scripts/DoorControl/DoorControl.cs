using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorcontrol : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 initposition;
    public Vector3 stopposition;
    public bool isdooropen = false;

    void Start()
    {
        initposition = this.transform.position;
        stopposition = new Vector3(initposition.x, initposition.y , initposition.z+1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isdooropen && initposition.z<stopposition.z)
        {
            initposition = new Vector3(initposition.x, initposition.y, initposition.z + 0.05f);
            this.transform.position = initposition;
        }
    }
}
