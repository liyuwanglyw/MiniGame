using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BaseSlider : MonoBehaviour
{
    public float flow_speed=200;
    protected Slider[] sliders;
    protected Image[] fill_img;
    // Start is called before the first frame update
    void Awake()
    {
        sliders = GetComponentsInChildren<Slider>();
        fill_img = new Image[sliders.Length];
        for(int i=0;i<sliders.Length;i++)
        {
            fill_img[i] = sliders[i].transform.GetChild(0).Find("Fill").GetComponent<Image>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillPipe(Color[] colors)
    {
        StartCoroutine(StartFlow(colors));
    }

    public virtual IEnumerator StartFlow(Color[] colors)
    {
        yield return null;
    }

    public void CleanPipe()
    {
        for(int i=0;i<sliders.Length;i++)
        {
            sliders[i].value = 0;
        }
    }
}
