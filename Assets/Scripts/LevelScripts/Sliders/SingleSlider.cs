using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSlider : BaseSlider
{
    private void Start()
    {
        //StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Test()
    {
        Color[] colors = new Color[4];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(1, 1, 1, 1);
        }
        while (true)
        {
            CleanPipe();
            yield return new WaitForSeconds(0.5f);
            FillPipe(colors);
            yield return new WaitForSeconds(1.5f);
        }
    }

    public override IEnumerator StartFlow(Color[] colors)
    {
        int len = (sliders.Length < colors.Length ? sliders.Length : colors.Length);
        for (int i = 0; i < len; i++)
        {
            fill_img[i].color = colors[i];
        }

        while (true)
        {
            sliders[0].value += flow_speed * Time.deltaTime;
            if (sliders[0].value > 99)
            {
                break;
            }
            yield return null;
        }
    }
}
