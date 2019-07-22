using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager : MonoBehaviour
{
    public GameObject[] Texts;
    public GameObject[] HighLight;
    private int pointer = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void PointerDown()
    {
        Texts[pointer].SetActive(false);
        //HighLight[pointer].SetActive(false);
        pointer++;

        if (pointer != Texts.Length)
        {
            Texts[pointer].SetActive(true);
            //HighLight[pointer].SetActive(true);
        }
        else
            Destroy(gameObject);

    }

}
