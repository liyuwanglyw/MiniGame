using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quitgame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void quitgame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

        Debug.Log("编辑状态游戏退出");

#else

            Application.Quit();

            Debug.Log ("游戏退出");

#endif
    }
}
