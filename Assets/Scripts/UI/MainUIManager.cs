using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    public GameObject prePage;
    public GameObject mainUI;

    enum PageType
    {
        PrePage,
        MainUI
    }
    private PageType current_page;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        ShowPrePage();
    }

    // Update is called once per frame
    void Update()
    {
        switch(current_page)
        {
            case PageType.PrePage:
                {
                    if (Input.anyKey)
                    {
                        ShowMainUI();
                    }
                    break;
                }
            case PageType.MainUI:
                {
                    break;
                }
            default:
                break;
        }
    }

    public void ShowMainUI()
    {
        if (prePage != null)
        {
            prePage.SetActive(false);
        }
        if (mainUI != null)
        {
            current_page = PageType.MainUI;
            mainUI.SetActive(true);
        }
    }

    public void HideMainUI()
    {
        if (mainUI != null)
        {
            mainUI.SetActive(false);
        }
    }

    public void ShowPrePage()
    {
        if (prePage != null)
        {
            current_page = PageType.PrePage;
            prePage.SetActive(true);
        }
    }

    public void OnStartGameBtnClick()
    {

    }

    public void OnSelectLevelBtnClick()
    {

    }

    public void OnLimitedTimeModeBtnClick()
    {

    }

    public void OnQuitGameBtnClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        
    }
}
