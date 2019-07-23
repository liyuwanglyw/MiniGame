using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePpageUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnContinueBtnClick()
    {
        MainUIManager.instance.ShowPage(MainUIManager.PageType.GamePage);
    }

    public void OnBackBtnClick()
    {
        MainUIManager.instance.ShowMainUI();
    }
}
