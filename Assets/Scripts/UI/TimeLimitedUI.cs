using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeLimitedUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnEasyBtnClick()
    {
        MainUIManager.instance.ShowPage(MainUIManager.PageType.GamePage);
        TimeLimitMode.instance.StartGame(TimeLimitMode.Difficulty.Simple);
    }

    public void OnNormalBtnClick()
    {
        MainUIManager.instance.ShowPage(MainUIManager.PageType.GamePage);

        TimeLimitMode.instance.StartGame(TimeLimitMode.Difficulty.Normal);

    }

    public void OnHardBtnClick()
    {
        MainUIManager.instance.ShowPage(MainUIManager.PageType.GamePage);

        TimeLimitMode.instance.StartGame(TimeLimitMode.Difficulty.Hard);
    }

}
