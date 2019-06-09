using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public bool Key = false;
    public GameObject[] obj;
    public int n = 0;  //每次会显示的镜头
    private int i = 0;

    public  Vector3 Character_Pos = new Vector3 (0,0,0);
    public float Dis;

    private GameObject Character;

    private  GameObject[] door;
    public GameObject nearestdoor; //最近一个需要开关的门
    // Start is called before the first frame update
    void Start()
    {
        


        obj[0].SetActive(true);
        obj[1].SetActive(false);
        obj[2].SetActive(false);
        obj[3].SetActive(false);
        obj[4].SetActive(false);
        obj[5].SetActive(false);
        obj[6].SetActive(false);
        obj[7].SetActive(false);
        obj[8].SetActive(false);

        door = GameObject.FindGameObjectsWithTag("door");
        Character = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FindNearDoor();
        Dis = Vector3.Distance(this.transform.position, Character.transform.position);
        if (Dis   < 1.5f  && Input .GetKeyDown (KeyCode.Space  ) && Key)
        {
            Debug.Log("距离足够");
            ChangeCamera ();
        }
    }

    private void ChangeCamera()
    {
        i = 0;
       
        while (i < 9)
        {
            obj[i].SetActive(false);
            i = i + 1;
        }
            obj[n].SetActive(true);
        Character.transform.position = Character_Pos;
        
    }

    private void FindNearDoor()
    {
        i = 0;
        nearestdoor = door[0];
        while (i<18)
        {
            if (Vector3.Distance(door[i].transform.position, Character.transform.position) < Vector3.Distance(nearestdoor.transform.position, Character.transform.position))
            {
                nearestdoor = door[i];
            }
            i = i + 1;
        }
    }

    private void CloseDoor()
    {
        
    }
}
