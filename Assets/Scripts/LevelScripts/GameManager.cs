using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Normal,
    TimeLimited
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameMode mode;

    private void Awake()
    {
        mode = GameMode.Normal;
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        mode = GameMode.Normal;
        LoadLevel(GameState.instance.unpass_level);
    }

    public bool NextLevel()
    {
        if (mode == GameMode.Normal)
        {
            int next_level_index = GameState.instance.GetNextLevelIndex();
            if (next_level_index != -1)
            {
                bool isLoaded = LoadLevel(next_level_index);
                if (isLoaded)
                {
                    GameState.instance.currrent_level++;
                }
                return isLoaded;
            }
            else
            {
                return false;
            }
        }
        
        if(mode==GameMode.TimeLimited)
        {

        }

        return false;
    }

    public bool LoadLevel(int level_index)
    {
        Debug.Log(string.Format("load level{0}", level_index));
        if(level_index>GameState.instance.level_count-1)
        {
            return false;
        }
        else
        {
            GameState.instance.currrent_level = level_index;
            string level_name = string.Format("level{0}", level_index + 1);
              
            bool isLoadLevel = false;
            isLoadLevel = MapControl.instance.StartLevel(level_name,PassLevel);
            return isLoadLevel;
        }
    }

    public void PassLevel(int star)
    {
        Debug.Log(star);
        MainUIManager.instance.ShowPage(MainUIManager.PageType.EndMenu);
        GameState.instance.level_stars[GameState.instance.currrent_level] = star;
        EndPageUI.instance.SetStar(star);
        GameState.instance.SaveGame();
    }

    public void QuitGame()
    {

    }
    
}
