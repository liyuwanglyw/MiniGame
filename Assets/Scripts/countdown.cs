using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdown : MonoBehaviour
{
    // Start is called before the first frame update
    private float totalTime1 = 900;
    private float intervalTime = 1;

    public Text CountDown1Text;
    void Start()
    {
        CountDown1Text.text = string.Format("{0:D2}:{1:D2}",
        (int)totalTime1 / 60, (int)totalTime1 % 60);
        StartCoroutine(CountDown1());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CountDown1()
    {
        while (totalTime1 > 0)
        {
            yield return new WaitForSeconds(1);
            totalTime1--;
            CountDown1Text.text = string.Format("{0:D2}:{1:D2}",
           (int)totalTime1 / 60, (int)totalTime1 % 60);
            if(totalTime1 == 1)
            {
                Debug.Log("计时结束");
            }
        }
    }
}
