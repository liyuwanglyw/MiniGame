using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelchoosescripts : MonoBehaviour
{

    public bool[] currentkeystate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setlightcolor()
    {
        for(int i = 0; i < 16; i++)
        {
            string lightname = "door" + i.ToString();
            if (currentkeystate[i])
            {
                
            }
            else
            {

            }
        }
    }
}
