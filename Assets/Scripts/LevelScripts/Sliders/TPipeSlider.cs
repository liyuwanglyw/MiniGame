using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPipeSlider : BaseSlider
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override IEnumerator StartFlow(Color[] colors)
    {
        int len = (sliders.Length < colors.Length ? sliders.Length : colors.Length);
        for(int i=0;i<len;i++)
        {
            fill_img[i].color = colors[i];
        }

        while(true)
        {
            sliders[0].value += flow_speed * Time.deltaTime;
            if(sliders[0].value>99)
            {
                break;
            }
            yield return null; 
        }
        while (true)
        {
            sliders[1].value += flow_speed * Time.deltaTime;
            sliders[2].value += flow_speed * Time.deltaTime;
            if (sliders[1].value > 99)
            {
                break;
            }
            yield return null;
        }

    }
}
