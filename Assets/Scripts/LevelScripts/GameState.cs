using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

class GameSaveData
{
    public int[] level_stars;
    public int currrent_level;
    public int level_count;
}
public class GameState : MonoBehaviour
{
    public int[] level_stars;
    public int currrent_level;
    public int level_count;

    public int unpass_level;
    private string save_path;

    public static GameState instance;

    private void Awake()
    {
     
    }
    // Start is called before the first frame update
    void Start()
    {
        save_path = Application.dataPath + "/data.save";
        level_count = 19;
        level_stars = new int[level_count];
        currrent_level = 0;
        LoadGame();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame()
    {
        GameSaveData save = new GameSaveData();
        save.level_count = level_count;
        save.level_stars =(int[])level_stars.Clone();
        save.currrent_level = currrent_level;
        
        SaveManager.getInstance().SetData(save_path,save);
    }

    public void LoadGame()
    {
        if (SaveManager.getInstance().IsFileExists(save_path))
        {
            GameSaveData data = (GameSaveData)SaveManager.getInstance().GetData(save_path, typeof(GameSaveData));
            level_stars = (int[])data.level_stars.Clone();
            level_count = data.level_count;
            currrent_level = data.currrent_level;
            
            for(int i=0;i<level_stars.Length;i++)
            {
                if(level_stars[i]==0)
                {
                    unpass_level = i;
                    break;
                }
            }
        }
    }

    public int GetNextLevelIndex()
    {
        if(currrent_level>=level_count-1)
        {
            return -1;
        }
        else
        {
            return currrent_level+1;
        }
    }
}
