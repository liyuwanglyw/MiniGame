using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    private Vector3 offset = new Vector3(0,0,10);

    void Start()
    {
        //设置相对偏移
        offset = target.position - this.transform.position;
    }

    void Update()
    {
        this.transform.position = target.position - offset;
    }
}
