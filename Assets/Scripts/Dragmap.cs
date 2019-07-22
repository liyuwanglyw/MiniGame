using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragmap : MonoBehaviour, IPointerClickHandler
{
    public leveleditor editor;
    public GameObject item;
    public GameObject panel;
    public GameObject currentbridgein;
    public int type;
 
    // Start is called before the first frame update
    void Start()
    {
        editor = GameObject.Find("Canvas").GetComponent<leveleditor>();
        Debug.Log(editor.r);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(type == 0)
        {
            GameObject citem = Instantiate(item);
            citem.transform.parent = panel.transform;
            citem.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        if(type == 1)
        {
            GameObject citem = Instantiate(item);
            citem.GetComponent<BaseModule>().gen_color.isValid = true;
            citem.GetComponent<BaseModule>().gen_color.r = editor.r;
            citem.GetComponent<BaseModule>().gen_color.g = editor.g;
            citem.GetComponent<BaseModule>().gen_color.b = editor.b;
            citem.GetComponent<BaseModule>().direct = editor.direction;
            citem.GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, (editor.direction+3) * 90f);
            citem.transform.parent = panel.transform;
            citem.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        if (type == 2)
        {
            GameObject citem = Instantiate(item);
            citem.GetComponent<BaseModule>().rev_color.isValid = true;
            citem.GetComponent<BaseModule>().rev_color.r = editor.r;
            citem.GetComponent<BaseModule>().rev_color.g = editor.g;
            citem.GetComponent<BaseModule>().rev_color.b = editor.b;
            citem.GetComponent<BaseModule>().direct = editor.direction;
            citem.GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, (editor.direction + 1) * 90f);
            citem.transform.parent = panel.transform;
            citem.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        if (type == 3)
        {
            GameObject citem = Instantiate(item);
            citem.GetComponent<BaseModule>().direct = editor.direction;
            citem.GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, (editor.direction + 2) * 90f);
            citem.transform.parent = panel.transform;
            GameObject.Find("bridgeout").GetComponent<Dragmap>().currentbridgein = citem;
            citem.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        if (type == 4)
        {
            GameObject citem = Instantiate(item);
            citem.GetComponent<BaseModule>().direct = editor.direction;
            citem.GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, (editor.direction) * 90f);
            citem.transform.parent = panel.transform;
            citem.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            currentbridgein.GetComponent<BaseModule>().bridge_out = citem.GetComponent<BaseModule>();
        }
    }
}
