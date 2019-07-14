using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragItem : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public DragType type;
    public string note;
    
    private Sprite sprite;
    private Text gold_text;
    private MapControl map;
    private float enter_time=0;
    private bool isMouseStay = false;
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
        isMouseStay = false;
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            int gold = map.GetModuleCost(type);
            if (map.mouse_state==DragType.Empty&&map.gold>=gold)
            {
                AudioControl.instance.PlayModule();
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

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isMouseStay = true;
        enter_time = Time.time;

        StartCoroutine(CheckMouseStay());
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isMouseStay = false;
        ModuleNote.instance.gameObject.SetActive(false);
    }

    private IEnumerator CheckMouseStay()
    {
        Vector3 world_point;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            MapControl.getInstance().GetComponent<RectTransform>(), transform.position, Camera.main, out world_point);
        while (isMouseStay)
        {
            if(Time.time-enter_time>0.5f)
            {
                ModuleNote.instance.gameObject.SetActive(true);
                ModuleNote.instance.SetNote(note);
                ModuleNote.instance.SetPosition(world_point);
            }
            yield return null;
        }
    }
}
