using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using CJFinc.UItools;
using UnityEngine.UI;

[System.Serializable]
public class ColorStream
{
    public ColorStream()
    {
        isValid = false;
        isPolluted = false;
        r = false;
        g = false;
        b = false;
    }

    public ColorStream(bool r,bool g,bool b)
    {
        isValid = true;
        isPolluted = false;
        this.r = r;
        this.g = g;
        this.b = b;
    }
    
    public ColorStream(ColorStream stream)
    {
        isValid = stream.isValid;
        isPolluted = stream.isPolluted;
        r = stream.r;
        g = stream.g;
        b = stream.b;
    }

    public static bool operator==(ColorStream a,ColorStream b)
    {
        return a.isValid == b.isValid && a.isPolluted == b.isPolluted && a.r == b.r && a.g == b.g && a.b == b.b;
    }

    public static bool operator !=(ColorStream a, ColorStream b)
    {
        return !(a == b);
    }

    public bool isValid;
    public bool isPolluted;
    public bool r,g,b;

    public Color real_color
    {
        get
        {
            if (isPolluted)
            {
                return Color.gray;
            }
            else
            {
                Color real = new Color();
                real.r = this.r ? 1 : 0;
                real.g = this.g ? 1 : 0;
                real.b = this.b ? 1 : 0;
                real.a = 1;
                return real;
            }
        }
    }
}

public class BaseModule : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
    //格子的一些常用参数
    public bool isPolluted;
    
    public int direct;

    //格子的坐标
    private int index_x, index_y;
    private int[,] d= new int[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };

    //当前格子的建筑
    public enum ModuleType
    {
        Empty,
        Plat,
        Invalid,
        SignalGen,
        SignalRev,
        BridgeIn,
        BridgeOut,
        SPipe,
        TPipe,
        XPipe,
        NotGate,
        AndGate,
        OrGate,
        CleanMachine
    }
    public ModuleType init_type;
    private Stack<ModuleType> all_types;
    public ModuleType current_type
    {
        get
        {
            if (all_types == null)
            {
                return init_type;
            }
            else
            {
                return all_types.Peek();
            }
        }
    }
    public bool isValid
    {
        get
        {
            return ((current_type == ModuleType.Empty || current_type == ModuleType.Plat)&&map.mouse_state!=DragType.Plat)
                ||(current_type==ModuleType.Invalid&&map.mouse_state==DragType.Plat);
        }
    }
    
    //如果是BridgeIn，需要关联BridgeOut
    public BaseModule bridge_out;
    public ColorStream gen_color;
    public ColorStream rev_color;

    //地图控制对象
    private MapControl map;
    //格子状态控制对象
    private UIStateItem state
    {
        get
        {
            return GetComponentInChildren<UIStateItem>();
        }
    }
    //控制格子颜色
    private BaseSlider pipe_control
    {
        get
        {
            return GetComponentInChildren<BaseSlider>();
        }

    }

    //当前格子的信号状态
    private ColorStream[] allInputs = new ColorStream[4];
    public ColorStream out_state
    {
        get
        {
            ColorStream output;
            switch (current_type)
            {
                case ModuleType.Empty:
                case ModuleType.Plat:
                default:
                    {
                        output = new ColorStream();
                        break;
                    }
                case ModuleType.SignalGen:
                case ModuleType.BridgeOut:
                    {
                        output=new ColorStream(allInputs[0]);
                        break;
                    }
                case ModuleType.SignalRev:
                case ModuleType.BridgeIn:
                case ModuleType.SPipe:
                case ModuleType.TPipe:
                case ModuleType.XPipe:
                    {
                        output=new ColorStream(allInputs[direct]);
                        break;
                    }
                case ModuleType.NotGate:
                    {
                        ColorStream input = allInputs[direct];
                        output = new ColorStream(!input.r,!input.g, !input.b);
                        output.isPolluted = input.isPolluted;
                        break;
                    }
                case ModuleType.AndGate:
                    {
                        ColorStream input_a = allInputs[direct];
                        ColorStream input_b = allInputs[(direct + 3) % 4];
                        output = new ColorStream(input_a.r&&input_b.r,input_a.g&&input_b.g,
                            input_a.b&&input_b.b);
                        output.isValid = input_a.isValid && input_b.isValid;
                        output.isPolluted = input_a.isPolluted||input_b.isPolluted;
                        break;
                    }
                case ModuleType.OrGate:
                    {
                        ColorStream input_a = allInputs[direct];
                        ColorStream input_b = allInputs[(direct + 3) % 4];
                        output = new ColorStream(input_a.r || input_b.r, input_a.g || input_b.g,
                            input_a.b || input_b.b);
                        output.isValid = input_a.isValid && input_b.isValid;
                        output.isPolluted = input_a.isPolluted || input_b.isPolluted;
                        break;
                    }
                case ModuleType.CleanMachine:
                    {
                        output = new ColorStream(allInputs[direct]);
                        output.isPolluted = false;
                        break;
                    }
            }
            if(isPolluted)
            {
                output.isPolluted = true;
            }
            return output;
        }
    }


    private void Awake()
    {
    }

    private void Start()
    {

        for(int i=0;i<allInputs.Length;i++)
        {
            allInputs[i] = new ColorStream();
        }

        all_types = new Stack<ModuleType>();
        all_types.Push(init_type);

        map = MapControl.getInstance();

        int sibling_index = transform.GetSiblingIndex();
        index_x = sibling_index / map.n;
        index_y = sibling_index % map.n;
        if(current_type==ModuleType.SignalGen)
        {
            allInputs[0] = gen_color;
            OutputState();
        }

        if (current_type == ModuleType.SignalRev)
        {
            Debug.Log(rev_color.real_color);
            Debug.Log(direct);
            FillPipe();
        }
    }

    public void InputState(int direct,ColorStream color)
    {
        direct %= 4;
        switch(current_type)
        {
            case ModuleType.SignalGen:
            case ModuleType.BridgeOut:
                {
                    if (direct == 4)
                    {
                        allInputs[0] = new ColorStream(color);
                        OutputState();
                    }
                    break;
                }
            case ModuleType.SignalRev:
                {
                    if (direct == this.direct)
                    {
                        allInputs[direct] = new ColorStream(color);
                    }
                    break;
                }
            case ModuleType.BridgeIn:
            case ModuleType.SPipe:
            case ModuleType.TPipe:
            case ModuleType.XPipe:
            case ModuleType.NotGate:
            case ModuleType.CleanMachine:
                {
                    if(direct==this.direct)
                    {
                        allInputs[direct] = new ColorStream(color);
                        OutputState();
                    }
                    break;
                }
            case ModuleType.AndGate:
            case ModuleType.OrGate:
                {
                    if (direct == this.direct || direct == (this.direct + 3) % 4)
                    {
                        allInputs[direct] = new ColorStream(color);
                        OutputState();
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        
    }

    public virtual void OutputState()
    {
        if (out_state.isValid)
        {
            FillPipe();
            
            Invoke("Spread", pipe_control.flow_time);
        }
    }

    public void OutputState(int d)
    {
        if(current_type==ModuleType.SignalRev)
        {

        }
        if (out_state.isValid)
        {
            switch (current_type)
            {
                case ModuleType.SignalGen:
                case ModuleType.BridgeOut:
                case ModuleType.SPipe:
                case ModuleType.NotGate:
                case ModuleType.AndGate:
                case ModuleType.OrGate:
                case ModuleType.CleanMachine:
                    {
                        if (d == (direct + 2) % 4)
                        {
                            BaseModule next = GetNextModule(d);
                            if (next != null)
                            {
                                next.InputState(direct, out_state);
                            }
                        }
                        break;
                    }
                case ModuleType.TPipe:
                    {
                        if (d == (direct + 1) % 4 || d == (direct + 3) % 4)
                        {
                            BaseModule next_a = GetNextModule(d);
                            if (next_a != null)
                            {
                                next_a.InputState((d+2)%4, out_state);
                            }
                        }
                        break;
                    }
                case ModuleType.XPipe:
                    {
                        for (int i = 1; i < 4; i++)
                        {
                            if (d == (direct + i) % 4)
                            {
                                BaseModule next = GetNextModule(d);
                                if (next != null)
                                {
                                    next.InputState((d + 2) % 4, out_state);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case ModuleType.BridgeIn:
                    {
                        bridge_out.InputState(4, out_state);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    public virtual void InitState()
    {
        for(int i=0;i<4;i++)
        {
            allInputs[i] = new ColorStream();
        }
    }

    public bool IsNextValid(int direct)
    {
        int next_x = index_x + d[direct, 0];
        int next_y = index_y + d[direct, 1];
        if(next_x<0||next_x>=map.m)
        {
            return false;
        }

        if (next_y < 0 || next_y >= map.n)
        {
            return false;
        }
        return true;
    }

    public BaseModule GetNextModule(int direct)
    {   
        if(IsNextValid(direct))
        {
            return map.modules[index_x + d[direct, 0], index_y + d[direct, 1]];
        }
        else
        {
            return null;
        }
    }

    public void SetModule(ModuleType type,int direct)
    {
        //删除原有module对象,从对象池中获得组件预设，并进行更换
        Destroy(transform.GetChild(0).gameObject);
        Debug.Log(type + "Module");
        Transform module = MapControl.getInstance().pool.Spawn(type + "Module", transform);
        
        //改变组件预设的位置、大小、角度
        module.GetComponent<RectTransform>().localPosition = Vector3.zero;
        Vector2 rect_size = transform.GetComponent<RectTransform>().sizeDelta;
        module.GetComponent<RectTransform>().sizeDelta = new Vector2(- 2, - 2);
        module.rotation = map.drag_item.rotation;
        
        this.direct = direct;
    }

    public void UpdateModule()
    {
        switch (current_type)
        {
            case ModuleType.TPipe:
            case ModuleType.SPipe:
            case ModuleType.XPipe:
            case ModuleType.OrGate:
            case ModuleType.CleanMachine:
                {
                    BaseModule next = GetNextModule(direct);
                    if (next != null)
                    {
                        next.OutputState((direct+2)%4);
                    }
                    break;
                }
            case ModuleType.NotGate:
            case ModuleType.AndGate:
                {
                    BaseModule next_a = GetNextModule(direct);
                    if (next_a != null)
                    {
                        next_a.OutputState((direct + 2) % 4);
                    }
                    BaseModule next_b = GetNextModule((direct + 3) % 4);
                    if (next_b != null)
                    {
                        next_b.OutputState((direct + 1) % 4);
                    }
                    break;
                }
            case ModuleType.Plat:
            default:
                break;
        }
    }

    #region 鼠标事件响应
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (map.mouse_state != DragType.Empty&&map.mouse_state!=DragType.Sold)
        {
            if (isValid)
            {
                state.SetStateActive();
            }
            else
            {
                state.SetStateDisabled();
            }
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(map.mouse_state==DragType.Sold)
            {
                SoldModule();
            }
            else if (map.mouse_state != DragType.Empty&&isValid)
            {
                //根据鼠标拖拽的对象更改组件
                ModuleType module_type = ModuleType.Empty;
                switch (map.mouse_state)
                {
                    case DragType.TPipe:
                        {
                            module_type = ModuleType.TPipe;
                            break;
                        }
                    case DragType.SPipe:
                        {
                            module_type = ModuleType.SPipe;
                            break;
                        }
                    case DragType.XPipe:
                        {
                            module_type = ModuleType.XPipe;
                            break;
                        }
                    case DragType.AndGate:
                        {
                            module_type = ModuleType.AndGate;
                            break;
                        }
                    case DragType.OrGate:
                        {
                            module_type = ModuleType.OrGate;
                            break;
                        }
                    case DragType.NotGate:
                        {
                            module_type = ModuleType.NotGate;
                            break;
                        }
                    case DragType.Clean:
                        {
                            module_type = ModuleType.CleanMachine;
                            break;
                        }
                    case DragType.Plat:
                        {
                            module_type = ModuleType.Plat;
                            break;
                        }
                    default:
                        break;
                }
                AddModule(module_type);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state.SetStateDefault();
    }
    #endregion

    public void FillPipe()
    {
        if (current_type == ModuleType.SignalRev)
        {
            Color[] colors = new Color[4];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = rev_color.real_color;
            }
            pipe_control.FillPipe(colors);
        }

        pipe_control.CleanPipe();
        if (out_state.isValid)
        {
            switch (current_type)
            {
                case ModuleType.SignalGen:
                case ModuleType.BridgeOut:
                case ModuleType.SPipe:
                case ModuleType.TPipe:
                case ModuleType.XPipe:
                case ModuleType.BridgeIn:
                    {
                        Color[] colors = new Color[4];
                        for (int i = 0; i < colors.Length; i++)
                        {
                            colors[i] = out_state.real_color;
                        }
                        pipe_control.FillPipe(colors);
                        break;
                    }

                case ModuleType.AndGate:
                case ModuleType.OrGate:
                    {
                        Color[] colors = new Color[3];
                        colors[0] = allInputs[direct].real_color;
                        colors[1] = allInputs[(direct + 3) % 4].real_color;
                        colors[2] = out_state.real_color;
                        pipe_control.FillPipe(colors);
                        break;
                    }
                case ModuleType.NotGate:
                case ModuleType.CleanMachine:
                    {
                        Color[] colors = new Color[2];
                        colors[0] = allInputs[direct].real_color;
                        colors[1] = out_state.real_color;
                        pipe_control.FillPipe(colors);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    private void Spread()
    {
        switch (current_type)
        {
            case ModuleType.SignalGen:
            case ModuleType.BridgeOut:
            case ModuleType.SPipe:
            case ModuleType.NotGate:
            case ModuleType.AndGate:
            case ModuleType.OrGate:
            case ModuleType.CleanMachine:
                {
                    BaseModule next = GetNextModule((direct + 2) % 4);
                    if (next != null)
                    {
                        next.InputState(direct, out_state);
                    }
                    break;
                }
            case ModuleType.TPipe:
                {
                    BaseModule next_a = GetNextModule((direct + 1) % 4);
                    if (next_a != null)
                    {
                        next_a.InputState((direct + 3) % 4, out_state);
                    }
                    BaseModule next_b = GetNextModule((direct + 3) % 4);
                    if (next_b != null)
                    {
                        next_b.InputState((direct + 1) % 4, out_state);
                    }
                    break;
                }
            case ModuleType.XPipe:
                {
                    for (int i = 1; i < 4; i++)
                    {
                        BaseModule next = GetNextModule((direct + i) % 4);
                        if (next != null)
                        {
                            next.InputState((direct + i + 2) % 4, out_state);
                        }
                    }
                    break;
                }
            case ModuleType.BridgeIn:
                {
                    bridge_out.InputState(4, out_state);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void AddModule(ModuleType module_type)
    {
        all_types.Push(module_type);
        SetModule(module_type, map.direct);
        UpdateModule();

    }

    private void SoldModule()
    {
        switch (current_type)
        {
            case ModuleType.Plat:
            case ModuleType.SPipe:
            case ModuleType.TPipe:
            case ModuleType.XPipe:
            case ModuleType.NotGate:
            case ModuleType.AndGate:
            case ModuleType.OrGate:
            case ModuleType.CleanMachine:
                {
                    Debug.Log(1);
                    all_types.Pop();
                    SetModule(current_type, 0);
                    UpdateModule();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void ResetModule()
    {
        //没有改变过状态
        if(all_types.Count==1)
        {
            return;
        }
        while(all_types.Count>0)
        {
            all_types.Pop();
        }
        AddModule(init_type);
    }
}
