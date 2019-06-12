using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragItem : MonoBehaviour,IPointerClickHandler
{
    public DragType type;
    
    private Sprite sprite;
    private Text gold_text;
    private MapControl map;
    // Start is called before the first frame update
    void Start()
    {
        map = MapControl.getInstance();
        sprite = transform.GetChild(0).GetComponent<Image>().sprite;
        GetComponentInChildren<Text>().text=map.GetModuleCost(type).ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        
        if(eventData.button==PointerEventData.InputButton.Left)
        {
            int gold = map.GetModuleCost(type);
            if (map.mouse_state==DragType.Empty&&map.gold>=gold)
            {
                map.gold -= gold;
                map.drag_item.GetComponent<Image>().sprite = sprite;
                map.drag_item.gameObject.SetActive(true);
                map.drag_item.rotation = transform.rotation;
                map.mouse_state = type;
                map.direct = 0;
                map.isUsed = false;
            }
        }
    }
}
