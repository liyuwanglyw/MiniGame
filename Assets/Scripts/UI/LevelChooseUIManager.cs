using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChooseUIManager : MonoBehaviour
{
    private const int LevelNumber = 19;
    private Button[] buttonList=new Button[LevelNumber];
    private int[] scoreList = new int[LevelNumber];

    //初次生成界面时，注册所有按钮的点击事件
    private void Start()
    {
        RegisterButtonsClickEvent();
    }

    //当界面被显示时，显示所有星星
    private void OnEnable()
    {
        for(int i=0;i<LevelNumber;i++)
        {
            buttonList[i] = transform.GetChild(i).GetComponent<Button>();
        }
        DisplayLevelScore();
    }

    //当界面被隐藏时，重置所有星星为不显示的状态
    private void OnDisable()
    {
        HideAllStars();
    }

    //按钮点击事件的回调函数，应调用关卡管理器根据关卡编号生成对应关卡（最好使用字符串生成，这样每次加入新关卡只要关卡命名正确即可运行）
    private void CallGenerateLevel(int levelIndex)
    {
        Debug.Log(levelIndex);
        GameManager.instance.LoadLevel(levelIndex);
        MainUIManager.instance.ShowPage(MainUIManager.PageType.GamePage);
    }
    //在Start时注册各个按钮的点击事件
    private void RegisterButtonsClickEvent()
    {
        for(int i=0;i<LevelNumber;i++)
        {
            Button button = buttonList[i];
            button.onClick.AddListener(delegate() { CallGenerateLevel(button.transform.GetSiblingIndex()); });
        }
    }
    //每次Enable时，显示所有关卡的积分情况的函数（星星）
    private void DisplayLevelScore()
    {
        //从存档文件中获取每关的星数
        GetAllLevelScore();

        //根据每关的星数将星星显示出来
        for (int i=0;i<LevelNumber;i++)
        {
            switch(scoreList[i])
            {
                case 1:
                    ShowStar(buttonList[i],1);
                    break;
                case 2:
                    ShowStar(buttonList[i],2);
                    break;
                case 3:
                    ShowStar(buttonList[i],3);
                    break;
            }
        }
    }
    //显示星星的函数
    private void ShowStar(Button tarButton,int starNum)
    {
        for(int i=0;i<starNum;i++)
        {
            tarButton.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    //隐藏所有星星的函数
    private void HideAllStars()
    {
        for(int i=0;i<LevelNumber;i++)
        {
            for (int j = 0; j < 3; j++)
            {
                buttonList[i].transform.GetChild(j).gameObject.SetActive(false);
            }
        }
    }
    //从存档文件中一次读取所有关卡的星数，并存入scoreList
    public void GetAllLevelScore()
    {
        int min = LevelNumber > GameState.instance.level_count ? GameState.instance.level_count : LevelNumber;
        for(int i=0;i<min;i++)
        {
            scoreList[i] = GameState.instance.level_stars[i];
        }
    }

}
