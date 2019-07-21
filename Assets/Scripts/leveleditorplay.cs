using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leveleditorplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("startlevel",0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startlevel()
    {
        MapControl.getInstance().editorStartLevel("level2",callback);
    }
    public void callback()
    {

    }
}
