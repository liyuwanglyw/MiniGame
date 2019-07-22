using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testlevelopen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("lopen", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lopen()
    {
        MapControl.getInstance().StartLevel("level1", testcallback);
    }
    public void testcallback(int star)
    {
        Debug.Log("callbacksuccess");
    }
}
