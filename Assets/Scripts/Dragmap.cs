using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragmap : MonoBehaviour, IPointerClickHandler
{
    public GameObject mapitem;
    public int itemdirection;
    public int itemcolor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changedirection(int direction)
    {
        itemdirection = direction;
    }
    public void changecolor(string direction)
    {
        int d = int.Parse(direction);
        itemdirection = d;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {

    }
}
