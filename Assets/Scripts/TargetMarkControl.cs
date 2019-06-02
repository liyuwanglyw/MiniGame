using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetMarkControl : MonoBehaviour
{
    public string[] TargetMark = {"0","","" };
    public int Mark ;
    private string M;
    private float m = 0;
    public float time = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("TargetMark");
      
            obj[0].GetComponent<Text>().text = TargetMark[0];
            obj[1].GetComponent<Text>().text = TargetMark[1];
            obj[2].GetComponent<Text>().text = TargetMark[2];
        //此段给三个目标分数进行赋值

        Increase();
        GameObject.FindGameObjectWithTag("Mark").GetComponent<Text>().text = M;
    }

    private void Increase()//此函数是让显示的数字自然增长，增长时间为time
    {
        
        if (m<Mark)
        {
            m = m + (Mark / time) * Time.deltaTime;  
        }
        M = Mathf .Round (m).ToString ();
    }
}
