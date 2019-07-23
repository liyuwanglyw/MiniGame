using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPageUI : MonoBehaviour
{
    private StarControl[] stars;
    public static EndPageUI instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        stars = GetComponentsInChildren<StarControl>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStar(int star)
    {
        if(stars==null)
        {
            return;
        }

        if (stars.Length == 3)
        {
            for (int i = 0; i < star; i++)
            {
                stars[i].SetStarActive(true);
            }
            for (int i = star; i < 3; i++)
            {
                stars[i].SetStarActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                stars[i].SetStarActive(false);
            }
        }
    }

    public void OnNextLevelBtnClick()
    {
        if(GameManager.instance==null)
        {
            return;
        }
        else
        {
            MainUIManager.instance.ShowPage(MainUIManager.PageType.GamePage);
            GameManager.instance.NextLevel();
        }
    }
}
