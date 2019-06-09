using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public bool Key = false;
    public GameObject[] obj;
    public int n = 0;
    private int i = 0;
    public  Vector3 Character_Pos = new Vector3 (0,0,0);
    public float Dis;
    private GameObject Character;
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

        Character = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Dis = Vector3.Distance(this.transform.position, Character.transform.position);
        if (Dis   < 1.5f  && Input .GetKeyDown (KeyCode.Space  ) && Key)
        {
            Debug.Log("距离足够");
            DoorOpen();
        }
    }

    private void DoorOpen()
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
}
