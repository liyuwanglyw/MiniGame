using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveeditorlevel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerlevel;
    void Start()
    {
        Object.DontDestroyOnLoad(GameObject.Find("editorsaver"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
