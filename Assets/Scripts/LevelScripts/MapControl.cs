using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using UnityEngine.UI;
using System;
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
    public Transform drag_item;     //拖拽时显示的图片
    public GameObject level_panel; //关卡预设
    public GameObject map_panel     //地图Panel对象
    {
        get
        {
            return level_panel.transform.GetChild(0).gameObject;
        }
    }    
    public GameObject drag_panel     //拖拽Panel对象
     {
        get
        {
            return level_panel.transform.GetChild(1).gameObject;
        }
    } 
    public GameObject help_panel     //点击问号跳出的提示框
    {
        get
        {
            return level_panel.transform.GetChild(2).gameObject;
        }
    }
    public GameObject gold_panel;    //金币Panel对象
    public Texture2D sold_cursor;    //出售物品时鼠标指针
    
    private Transform back_area
    {
        get
        {
            return transform.GetChild(0);
        }
    }

    public int init_gold;   //初始化金币
    public delegate void GameOverCallBack(int star);
    private GameOverCallBack game_over;//游戏结束的回调函数

    public int gold
    {
        set
        {
            SetGold(value);
        }
        get
        {
            return GetGold();
        }
    }

    [HideInInspector]
    public BaseModule[,] modules;//存放所有BaseModule对象
    private int module_count;
    [HideInInspector]
    public int m,n;//地图规格 m*n
    private List<BaseModule> signal_gen;//存放所有发射点
    private List<BaseModule> signal_rev;//存放所有接收点

    //鼠标拖拽物体的状态
    [HideInInspector]
    public DragType mouse_state=DragType.Empty;
    [HideInInspector]
    public int direct;//拖拽时物体的方向
    [HideInInspector]
    public bool isUsed=true;

    [HideInInspector]
    public SpawnPool pool;//对象池

    public static MapControl instance;

    private void Awake()
    {
        instance = this;
        GameInit();
        GameStart();
    }

  
    
    public static MapControl getInstance()
    {
        return instance;
    }

    #region 测试用函数
    public void TestStart()
    {
        GameStart();
        SetGameOverCallBack(Over);
    }

    public void Over(int star)
    {
        Debug.Log("over");
    }

    public void Over2()
    {
        Debug.Log("over2");
    }
    #endregion

    #region 游戏流程控制
    //游戏进程初始化
    public void GameInit()
    {
        //初始化 发射点 和 接收点 List
        signal_gen = new List<BaseModule>();
        signal_rev = new List<BaseModule>();

        //初始化m，n的值
        Vector2 rect_size = map_panel.GetComponent<RectTransform>().rect.size;
        Vector2 cell_size = map_panel.GetComponent<GridLayoutGroup>().cellSize;
        int max_column=(int)(rect_size.x / cell_size.x);
        n = (max_column < map_panel.transform.childCount ? max_column : map_panel.transform.childCount);
        m = Mathf.CeilToInt(map_panel.transform.childCount / (float)max_column);
        

        //获取对象池
        pool = GameObject.Find("Managers").GetComponent<SpawnPool>();

        //初始化拖拽时候图片的大小
        Debug.Log(m + ","+n);
        drag_item.GetComponent<RectTransform>().sizeDelta = cell_size;

        //初始化Module数组
        modules = new BaseModule[m, n];
        BaseModule[] mod = map_panel.GetComponentsInChildren<BaseModule>();
        Debug.Log(mod.Length);
        module_count = mod.Length;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i * n + j < mod.Length)
                {
                    BaseModule temp = mod[i * n + j];
                    modules[i, j] = temp;
                    if (temp.init_type == BaseModule.ModuleType.SignalGen)
                    {
                        signal_gen.Add(temp);
                    }
                    if (temp.init_type == BaseModule.ModuleType.SignalRev)
                    {
                        signal_rev.Add(temp);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        //初始化金币
        gold = init_gold;
    }
    
    //开始游戏
    public void GameStart()
    {
        StartCoroutine(CheckGameStart());
    }
    
    //在模块都初始化完毕后才开始游戏
    private IEnumerator CheckGameStart()
    {
        bool isModuleInit = false;
        while(!isModuleInit)
        {
            isModuleInit = true;
            for(int i=0;i<modules.GetLength(0);i++)
            {
                for (int j = 0; j < modules.GetLength(1); j++)
                {
                    if (i * n + j < module_count)
                    {
                        if (modules[i, j] != null && !modules[i, j].isInit)
                        {
                            isModuleInit = false;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            yield return null;
        }
        ShowGame();

        for (int i = 0; i < signal_gen.Count; i++)
        {
            signal_gen[i].OutputState();
        }
        for (int i = 0; i < signal_rev.Count; i++)
        {
            signal_rev[i].FillPipe();
        }
        
        StartCoroutine(CheckGameOver());
    }
    
    //检查游戏是否完成
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
                    signal_rev[i].CloseLock();
                    isGameOver = false;
                }
                else
                {
                    signal_rev[i].OpenLock();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        game_over?.Invoke(GetStar());
    }

    //获得星数
    public int GetStar()
    {
        goldsave goldSave = GetComponent<goldsave>();
        if(goldSave==null)
        {
            return 1;
        }
        if(gold>=goldSave.star3)
        {
            return 3;
        }
        else if(gold>=goldSave.star2)
        {
            return 2;
        }
        else if(gold>=goldSave.star1)
        {
            return 1;
        }
        else
        {
            return 1;
        }   
    }
    
    //添加游戏结束相应函数
    public void SetGameOverCallBack(GameOverCallBack callBack)
    {
        //清空原有函数列表
        if (game_over != null)
        {
            Delegate[] delegates = game_over.GetInvocationList();
            foreach (Delegate d in delegates)
            {
                game_over -= (GameOverCallBack)d;
            }
        }
        game_over += callBack;
    }
    #endregion

    #region 游戏关卡控制
    //开始关卡
    public bool StartLevel(string level_name,GameOverCallBack callBack = null)
    {
        ShowGame();
        bool succeed = ChangeLevel(level_name, callBack);
        GameStart();
        return succeed;
    }

    //切换关卡
    public bool ChangeLevel(string level_name, GameOverCallBack callBack = null)
    {
        string level_path = "Level/" + level_name;
        GameObject level_prefab = Resources.Load<GameObject>(level_path);
        if (level_prefab != null)
        {
            SetGameOverCallBack(callBack);

            GameObject next_level = Instantiate(level_prefab);
            GameObject current_level = level_panel;
            init_gold = level_prefab.GetComponent<goldsave>().gold;

            next_level.transform.parent = back_area;
            next_level.transform.localPosition = current_level.transform.localPosition;
            next_level.transform.localScale= current_level.transform.localScale;
            next_level.transform.rotation= current_level.transform.rotation;
            next_level.transform.SetSiblingIndex(0);

            Destroy(current_level);

            level_panel = next_level;
            GameInit();
            return true;
        }
        else
        {
            return false;
        }
    }

    //重置关卡

    public bool editorStartLevel(string level_name, GameOverCallBack callBack = null)
    {
        ShowGame();
        bool succeed = editorChangeLevel(level_name, callBack);
        GameStart();
        return succeed;
    }

    //切换关卡
    public bool editorChangeLevel(string level_name, GameOverCallBack callBack = null)
    {
        string level_path = "playersavelevel/mylevel";
        GameObject level_prefab = Resources.Load<GameObject>(level_path);
        if (level_prefab != null && callBack != null)
        {
            SetGameOverCallBack(callBack);

            GameObject next_level = Instantiate(level_prefab);
            GameObject current_level = level_panel;
            init_gold = level_prefab.GetComponent<goldsave>().gold;

            next_level.transform.parent = back_area;
            next_level.transform.localPosition = current_level.transform.localPosition;
            next_level.transform.localScale = current_level.transform.localScale;
            next_level.transform.rotation = current_level.transform.rotation;
            next_level.transform.SetSiblingIndex(0);

            Destroy(current_level);

            level_panel = next_level;
            GameInit();
            return true;
        }
        else
        {
            return false;
        }
    }
    //编辑器开始关卡
    public void ResetLevel()
    {
        gold = init_gold;
        for (int i = 0; i < modules.GetLength(0); i++)
        {
            for (int j = 0; j < modules.GetLength(1); j++)
            {
                modules[i, j].ResetModule();
            }
        }
        AudioControl.instance.PlayBtnClick();
    }

    //显示游戏画面
    public void ShowGame()
    {
        back_area.gameObject.SetActive(true);
    }

    //隐藏游戏画面
    public void HideGame()
    {
        AudioControl.instance.PlayBtnClick();
        back_area.gameObject.SetActive(false);
    }
    #endregion

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            game_over?.Invoke(3);
            //MapControl map=MapControl.getInstance();
            //map.HideGame();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(mouse_state);
            Debug.Log(gold);
            for (int i = 0; i < signal_rev.Count; i++)
            {
                signal_rev[i].rev_color.print();
                signal_rev[i].out_state.print();
            }
        }
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeMouseStateToEmpty();
        }
        
        if(mouse_state!=DragType.Empty&&mouse_state!=DragType.Sold)
        {
            if (drag_item != null && drag_item.gameObject.activeSelf)
            {
                drag_item.position = Input.mousePosition;
            }

            if (Input.GetMouseButtonDown(1))
            {
                AudioControl.instance.PlayRotate();
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


    #region 鼠标状态转换函数
    public void ChangeMouseStateToSold()
    {
        AudioControl.instance.PlayBtnClick();
        ChangeMouseState(DragType.Sold);
    }

    public void ChangeMouseStateToEmpty()
    {
        if (!isUsed)
        {
            gold += GetModuleCost(mouse_state);
        }
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
    #endregion
    
    #region 金币控制函数
    private void SetGold(int gold)
    {
        gold_panel.GetComponentInChildren<Text>().text = "x"+gold.ToString();
    }

    private int GetGold()
    {
        string gold_text = gold_panel.GetComponentInChildren<Text>().text;
        return Convert.ToInt32(gold_text.Replace("x",""));
    }

    public int GetModuleCost(DragType type)
    {
        switch (type)
        {
            case DragType.TPipe:
                {
                    return 700;
                }
            case DragType.XPipe:
                {
                    return 1000;
                }
            case DragType.SPipe:
            case DragType.NotGate:
            case DragType.AndGate:
            case DragType.OrGate:
                {
                    return 500;
                }
            case DragType.Plat:
            case DragType.Clean:
                {
                    return 1500;
                }
            default:
                {
                    return 0;
                }
        }
    }
    #endregion

    public void ShowNote()
    {
        help_panel.gameObject.SetActive(true);
        AudioControl.instance.PlayBtnClick();
    }
}
