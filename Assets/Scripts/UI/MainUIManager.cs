using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    public GameObject prePage;
    public GameObject mainUI;
    public GameObject selectLevelMenu;
    public GameObject timeLimitedMenu;
    public GameObject pauseMenu;
    public GameObject endMenu;
    public GameObject gamePage;

    public enum PageType
    {
        PrePage,
        MainUI,
        SelectLevelMenu,
        TimeLimitedMenu,
        PauseMenu,
        EndMenu,
        GamePage
    }
    private PageType current_page;
    Dictionary<PageType,GameObject> page_dict;

    public static MainUIManager instance;

    private void Awake()
    {
        instance = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        page_dict = new Dictionary<PageType, GameObject>();
        page_dict.Add(PageType.PrePage, prePage);
        page_dict.Add(PageType.MainUI, mainUI);
        page_dict.Add(PageType.SelectLevelMenu, selectLevelMenu);
        page_dict.Add(PageType.TimeLimitedMenu, timeLimitedMenu);
        page_dict.Add(PageType.PauseMenu, pauseMenu);
        page_dict.Add(PageType.EndMenu, endMenu);
        page_dict.Add(PageType.GamePage, gamePage);

        ShowPage(PageType.PrePage);
        //ShowPage(PageType.EndMenu);
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
                        ShowPage(PageType.MainUI);
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

    public void ShowPage(PageType pageType)
    {
        foreach(var entry in page_dict)
        {
            if (entry.Value != null)
            {
                entry.Value.SetActive(entry.Key == pageType);
            }
        }
        current_page = pageType;
    }

    public void HideAllPages()
    {
        foreach (var entry in page_dict)
        {
            if (entry.Value != null)
            {
                entry.Value.SetActive(false);
            }
        }
    }

    public void ShowMainUI()
    {
        ShowPage(PageType.MainUI);
    }

    public void HideMainUI()
    {
        if (page_dict[PageType.MainUI] != null)
        {
            page_dict[PageType.MainUI].SetActive(false);
        }
    }


    public void OnStartGameBtnClick()
    {
        ShowPage(PageType.GamePage);
        GameManager.instance.StartGame();
    }

    public void OnSelectLevelBtnClick()
    {
        ShowPage(PageType.SelectLevelMenu);
    }

    public void OnLimitedTimeModeBtnClick()
    {
        ShowPage(PageType.TimeLimitedMenu);
        
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
