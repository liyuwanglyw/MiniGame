using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
public class MapControl : MonoBehaviour
{
    public BaseModule[,] modules;
    public int m,n;

    public enum MouseState
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

    public MouseState mouse_state=MouseState.Empty;

    public Transform drag_item;
    public SpawnPool pool;
    public GameObject map_panel;
    public GameObject drag_panel;

    public static MapControl instance;

    private void Awake()
    {
        instance = this;   
        
    }

    public static MapControl getInstance()
    {
        return instance;
    }

    private void Start()
    {
        pool = GameObject.Find("Managers").GetComponent<SpawnPool>();

        Vector2 rect_size=map_panel.GetComponent<RectTransform>().rect.size;
        Vector2 cell_size = map_panel.GetComponent<GridLayout>().cellSize;
        n =(int) (rect_size.x / cell_size.x);
        m = map_panel.transform.childCount / n;

        drag_item.GetComponent<RectTransform>().sizeDelta = cell_size;

        BaseModule[] mod = map_panel.GetComponentsInChildren<BaseModule>();
        for(int i=0;i<m;i++)
        {
            for(int j=0;j<n;j++)
            {
                modules[i,j] = mod[i * n + j];
            }
        }
    }

    private void Update()
    {
        if(mouse_state!=MouseState.Empty&&drag_item.gameObject.activeSelf)
        {
            drag_item.position = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            
        }
        else if(Input.GetMouseButton(1))
        {
            drag_item.Rotate(new Vector3());
        }
    }
    

}
