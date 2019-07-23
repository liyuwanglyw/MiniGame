using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitMode : MonoBehaviour
{
    public countdown timer;

    public enum Difficulty
    {
        Simple,
        Normal,
        Hard
    }

    private Difficulty game_difficulty;
    private int current_level;

    public static TimeLimitMode instance;

    private GameObject[] simpleLevels;
    private GameObject[] normalLevels;
    private GameObject[] hardLevels;

    private GameObject[] currentModeLevels;
    private int score;
    private void Awake()
    {
        instance = this;

        simpleLevels = Resources.LoadAll<GameObject>("timelevel/simple/");
        normalLevels = Resources.LoadAll<GameObject>("timelevel/normal/");
        hardLevels = Resources.LoadAll<GameObject>("timelevel/hard/");
        Debug.Log("Load Simple Levels Count:" + simpleLevels.Length);
        Debug.Log("Load Normal Levels Count:" + normalLevels.Length);
        Debug.Log("Load Hard Levels Count:" + hardLevels.Length);
    }

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(Difficulty difficulty)
    {
        current_level = 0;
        score = 0;
        timer.gameObject.SetActive(true);

        game_difficulty = difficulty;
        switch (game_difficulty)
        {
            case Difficulty.Simple:
            default:
                currentModeLevels = simpleLevels;
                break;
            case Difficulty.Normal:
                currentModeLevels = normalLevels;
                break;
            case Difficulty.Hard:
                currentModeLevels = hardLevels;
                break;
        }

        LoadLevel(current_level);
    }

    public void NextLevel()
    {
        current_level++;
        LoadLevel(current_level);
    }

    public void LoadLevel(int level_index)
    {
        GameManager.instance.mode = GameMode.TimeLimited;
        if (level_index>=currentModeLevels.Length)
        {
            EndGame();
        }
        else
        {
            MapControl.instance.StartLevel(currentModeLevels[level_index],PassLevel);
        }
    }

    public void PassLevel(int star)
    {
        score += star;
        if(current_level==currentModeLevels.Length-1)
        {
            EndGame();
        }
        else
        {
            MainUIManager.instance.ShowPage(MainUIManager.PageType.EndMenu);
            EndPageUI.instance.SetStar(star);
        }
    }

    public void EndGame()
    {
        timer.gameObject.SetActive(false);
        MainUIManager.instance.ShowPage(MainUIManager.PageType.TimeLimitedEndPage);
        TimeLimitedEndPage.instance.SetScore(score);
    }
}
