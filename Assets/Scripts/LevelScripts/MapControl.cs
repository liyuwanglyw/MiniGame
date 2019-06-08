using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using UnityEngine.UI;
public enum DragType
{
    Empty,
    Sold,
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
    public Transform drag_item;
    [HideInInspector]
    public int direct;

    //public SpawnPool pool;
    public GameObject map_panel;
    public GameObject drag_panel;

    public Texture2D sold_cursor;

    [HideInInspector]
    public BaseModule[,] modules;
    [HideInInspector]
    public int m,n;
    private List<BaseModule> signal_gen;
    private List<BaseModule> signal_rev;

    [HideInInspector]
    public DragType mouse_state=DragType.Empty;
    
    public static MapControl instance;

    [HideInInspector]
    public SpawnPool pool;

    private void Awake()
    {
        instance = this;
        GameInit();
        pool = GameObject.Find("Managers").GetComponent<SpawnPool>();
    }

    public static MapControl getInstance()
    {
        return instance;
    }
    

    public void GameInit()
    {
        signal_gen = new List<BaseModule>();
        signal_rev = new List<BaseModule>();

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
                BaseModule temp = mod[i * n + j];
                modules[i, j] = temp;
                if (temp.init_type==BaseModule.ModuleType.SignalGen)
                {
                    signal_gen.Add(temp);
                }
                if (temp.init_type == BaseModule.ModuleType.SignalRev)
                {
                    signal_rev.Add(temp);
                }
            }
        }
    }

    public void GameStart()
    {
        for(int i=0;i<signal_gen.Count;i++)
        {
            signal_gen[i].OutputState();
        }
        StartCoroutine(CheckGameOver());
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
    }

    public IEnumerator CheckGameOver()
    {
        bool isGameOver = false;
        while (!isGameOver)
        {
            isGameOver = true;
            for (int i = 0; i < signal_rev.Count; i++)
            {
                if (signal_rev[i].rev_color != signal_rev[i].out_state)
                {
                    isGameOver = false;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        GameOver();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(mouse_state);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeMouseStateToEmpty();
        }

        if (mouse_state == DragType.Sold)
        {
            if (Input.GetMouseButtonDown(1))
            {
                ChangeMouseStateToEmpty();
            }
        }
        else if(mouse_state!=DragType.Empty)
        {
            if (drag_item != null && drag_item.gameObject.activeSelf)
            {
                drag_item.position = Input.mousePosition;
            }

            if (Input.GetMouseButtonDown(0))
            {
                ChangeMouseStateToEmpty();
            }
            else if (Input.GetMouseButtonDown(1))
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


    public void ChangeMouseStateToSold()
    {
        ChangeMouseState(DragType.Sold);
    }

    public void ChangeMouseStateToEmpty()
    {
        ChangeMouseState(DragType.Empty);

    }

    public void ChangeMouseState(DragType type)
    {
        if(type==DragType.Empty)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Invoke("HideDragItem", 0.2f);
        }
        else if(type==DragType.Sold)
        {
            Cursor.SetCursor(sold_cursor, Vector2.zero, CursorMode.Auto);
            mouse_state = type;
        }
    }

    public void ResetLevel()
    {
        for(int i=0;i<modules.GetLength(0);i++)
        {
            for(int j=0;j<modules.GetLength(1);j++)
            {
                modules[i, j].ResetModule();
            }
        }
    }
    

}
