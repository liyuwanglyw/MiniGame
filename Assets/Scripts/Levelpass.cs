using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelpass : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ui;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ui.SetActive(true);
    }
}
