﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class leveleditorplay : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject overpanel;
    public  Button upload;
    public Button back;

    void Start()
    {
        overpanel = GameObject.Find("overpanel");
        overpanel.SetActive(false);
        back.onClick.AddListener(backtomain);
        Invoke("startlevel",0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startlevel()
    {
        MapControl.getInstance().editorStartLevel("level2",callback);
    }
    public void callback(int star)
    {
        overpanel.SetActive(true);
    }
    public void backtomain()
    {
        SceneManager.LoadScene("MainUI");
    }
}