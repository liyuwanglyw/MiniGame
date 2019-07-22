using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class leveleditor : MonoBehaviour
{
    public int direction;
    public bool r = true;
    public bool g = true;
    public bool b = true;
    public int gold = 30000;
    public int star2 = 2000;
    public int star3 = 5000;
    public Toggle toggler;
    public Toggle toggleg;
    public Toggle toggleb;
    public Dropdown dropdirection;
    public Button save;
    public Button hide;
    public GameObject level;
    public GameObject editorpanel;
    public InputField goldinput;
    public InputField star2input;
    public InputField star3input;
    bool isshow = true;

    // Start is called before the first frame update
    void Start()
    {
        
        toggler.onValueChanged.AddListener(changeR);
        toggleg.onValueChanged.AddListener(changeG);
        toggleb.onValueChanged.AddListener(changeB);
        dropdirection.onValueChanged.AddListener(changeD);
        goldinput.onValueChange.AddListener(changegold);
        star2input.onValueChange.AddListener(changestar2);
        star3input.onValueChange.AddListener(changestar3);
        save.onClick.AddListener(savegame);
        hide.onClick.AddListener(hidepanel);
        r = true;
        b = true;
        g = true;
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeR(bool newr)
    {
        r = newr;
        Debug.Log(r);
    }
    public void changeG(bool newg)
    {
        g = newg;
    }
    public void changeB(bool newb)
    {
        b = newb;
    }
    public void changeD(int newd)
    {
        direction = newd;
    }
    public void savegame()
    {
        Debug.Log("insavelevel");
        level.GetComponent<goldsave>().gold = this.gold;
        PrefabUtility.SaveAsPrefabAsset(level, "Assets/Resources/playersavelevel/mylevel.prefab");
        Invoke("switchscene", 0.5f);
    }
    public void switchscene()
    {
        SceneManager.LoadScene("Leveleditorplay");
    }
    public void changegold(string goldstring)
    {
        gold = int.Parse(goldstring);
    }
    public void changestar2(string star2string)
    {
        star2 = int.Parse(star2string);
    }
    public void changestar3(string star3string)
    {
        star3 = int.Parse(star3string);
    }
    public void hidepanel()
    {
        if (isshow)
        {
            editorpanel.SetActive(false);
            isshow = false;
        }
        else
        {
            editorpanel.SetActive(true);
            isshow = true;
        }
    }
}
