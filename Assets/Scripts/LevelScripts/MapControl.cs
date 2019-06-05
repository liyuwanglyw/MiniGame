using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using UnityEngine.UI;
public enum DragType
{
    Empty,
    TPipe,
    SPipe,
    XPipe,
    AndGate,
    OrGate,
    NotGate,
    Plat,
    Clean
}
public class MapControl : MonoBehaviour
{
    public BaseModule[,] modules;
    public int m,n;



    public DragType mouse_state=DragType.Empty;

    public Transform drag_item;
    public int direct;

    //public SpawnPool pool;
    public GameObject map_panel;
    public GameObject drag_panel;

    public static MapControl instance;

    private void Awake()
    {
        instance = this;
        GameStart();
    }

    public static MapControl getInstance()
    {
        return instance;
    }
    

    public void GameStart()
    {
        //pool = GameObject.Find("Managers").GetComponent<SpawnPool>();

        Vector2 rect_size = map_panel.GetComponent<RectTransform>().rect.size;
        Vector2 cell_size = map_panel.GetComponent<GridLayoutGroup>().cellSize;
        n = (int)(rect_size.x / cell_size.x);
        m = map_panel.transform.childCount / n;

        Debug.Log(m + ","+n);
        drag_item.GetComponent<RectTransform>().sizeDelta = cell_size;

        modules = new BaseModule[m, n];
        BaseModule[] mod = map_panel.GetComponentsInChildren<BaseModule>();
        Debug.Log(mod.Length);
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                modules[i, j] = mod[i * n + j];
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(mouse_state);
        }
        if(mouse_state!=DragType.Empty)
        {
            if (drag_item != null && drag_item.gameObject.activeSelf)
            {
                drag_item.position = Input.mousePosition;
            }

            if(Input.GetMouseButtonDown(0))
            {
                Invoke("HideDragItem", 0.2f);
            }
            else if(Input.GetMouseButtonDown(1))
            {
                drag_item.Rotate(0, 0, 90);
                direct += 1;
                direct %= 4;
            }
           
        }
    }

    public void HideDragItem()
    {
        drag_item.gameObject.SetActive(false);
        mouse_state = DragType.Empty;
        direct = 0;
    }
    

}
