using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    private static MainManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public static MainManager getInstance()
    {
        return instance;
    }
}
