using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitMode : MonoBehaviour
{
    public static TimeLimitMode instance;

    private GameObject[] simpleLevels;
    private GameObject[] normalLevels;
    private GameObject[] hardLevels;

    private void Awake()
    {
        instance = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
