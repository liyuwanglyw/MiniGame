using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour,IPointerClickHandler
{
    public enum DragType
    {
        SPipe,
        TPipe,
        XPipe,
        AndGate,
        OrGate,
        NotGate,
        Plat,
        Clean
    }
    public DragType type;

    private MapControl map;
    // Start is called before the first frame update
    void Start()
    {
        map = MapControl.getInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Left)
        {
            if(map.mouse_state==MapControl.MouseState.Empty)
            {
            }
            
        }
    }
}
