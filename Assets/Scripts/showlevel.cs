using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showlevel : MonoBehaviour
{
    public string levelname;
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
            GameObject.Find("Robin").GetComponent<CharacterControl>().Openlevel = levelname;
            GameObject.Find("Robin").GetComponent<CharacterControl>().opendoor = door;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GameObject.Find("Robin").GetComponent<CharacterControl>().Openlevel = null;
    }
}
