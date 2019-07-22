using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameState : MonoBehaviour
{
    public int[] level_stars;
    public int currrent_level;
    public int level_count;

    public static GameState instance;
    private void Awake()
    {
        level_count = 19;
        level_stars = new int[level_count];
        currrent_level = 3;
        LoadGame();
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

    public void SaveGame()
    {

    }

    public void LoadGame()
    {
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
