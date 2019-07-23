using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveeditor : MonoBehaviour
{
    public GameObject savelevel;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        Object.DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
