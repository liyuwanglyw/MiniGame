using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class leveleditor : MonoBehaviour
{
    public int direction;
    public bool r = true;
    public bool g = true;
    public bool b = true;
    public Toggle toggler;
    public Toggle toggleg;
    public Toggle toggleb;
    public Dropdown dropdirection;
    public Button save;
    public GameObject level;
    // Start is called before the first frame update
    void Start()
    {
        
        toggler.onValueChanged.AddListener(changeR);
        toggleg.onValueChanged.AddListener(changeG);
        toggleb.onValueChanged.AddListener(changeB);
        dropdirection.onValueChanged.AddListener(changeD);
        save.onClick.AddListener(savegame);
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
        PrefabUtility.SaveAsPrefabAsset(level, "Assets/playersavelevel/mylevel.prefab");
    }
}
