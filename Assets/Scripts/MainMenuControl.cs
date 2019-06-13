using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuControl : MonoBehaviour
{
    public GameObject setMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartGameBtnClick()
    {
        SceneManager.LoadScene("LevelChoose2");
    }

    public void OnSetMenuBtnClick()
    {
        gameObject.SetActive(false);
        setMenu.gameObject.SetActive(true);
    }

    public void OnQuitBtnClick()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

        Debug.Log("编辑状态游戏退出");

#else

            Application.Quit();

            Debug.Log ("游戏退出"):

#endif
    }
}
