using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showlevel : MonoBehaviour
{
    public GameObject panel;
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (door.GetComponent<Doorcontrol>().isdooropen == false)
        {
            GameObject.Find("Robin").GetComponent<CharacterControl>().Openlevel = panel;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GameObject.Find("Robin").GetComponent<CharacterControl>().Openlevel = null;
    }
}
