using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guideUIcontrol : MonoBehaviour
{
    public GameObject guide;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destoryself", 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void destoryself()
    {
        guide.SetActive(false);
    }
}
