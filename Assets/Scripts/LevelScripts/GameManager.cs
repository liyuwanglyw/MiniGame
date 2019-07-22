using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
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
        LoadLevel(GameState.instance.currrent_level);
    }

    public bool NextLevel()
    {
        int next_level_index = GameState.instance.GetNextLevelIndex();
        if (next_level_index!=-1)
        {
            bool isLoaded= LoadLevel(next_level_index);
            if(isLoaded)
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

    public bool LoadLevel(int level_index)
    {
        if(level_index>GameState.instance.level_count-1)
        {
            return false;
        }
        else
        {
            string level_name = string.Format("level{0}", level_index + 1);
              
            bool isLoadLevel = false;
            isLoadLevel = MapControl.instance.StartLevel(level_name,PassLevel);
            return isLoadLevel;
        }
    }

    public void PassLevel(int star)
    {
        GameState.instance.level_stars[GameState.instance.currrent_level] = star;
        EndPageUI.instance.SetStar(star);
        MainUIManager.instance.ShowPage(MainUIManager.PageType.EndMenu);
    }

    public void QuitGame()
    {

    }
    
}
