using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarControl : MonoBehaviour
{
    public Sprite active;
    public Sprite inactive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStarActive(bool selfActive)
    {
        if (selfActive)
        {
            GetComponent<Image>().sprite = active;
        }
        else
        {
            GetComponent<Image>().sprite = inactive;
        }
    }
}
