using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMenuControl : MonoBehaviour
{
    public GameObject mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMainMenu()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
