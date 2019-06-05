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

    public bool isValid;
    public bool isPolluted;
    public bool r,g,b;
}

public class BaseModule : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
    //格子的一些常用参数
    public bool isPolluted;
    
    public int direct;

    //格子的坐标
    private int index_x, index_y;
    private int[,] d= new int[,] { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };

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
             return all_types.Peek();
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

    public BaseModule bridge_out;//如果是BridgeIn，需要关联BridgeOut

    //地图控制对象
    private MapControl map;
    //格子状态控制对象
    private UIStateItem state;
    //格子建筑的图片
    private Image module_sprite;

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
                        output.isPolluted = input_a.isPolluted||input_b.isPolluted;
                        break;
                    }
                case ModuleType.OrGate:
                    {
                        ColorStream input_a = allInputs[direct];
                        ColorStream input_b = allInputs[(direct + 3) % 4];
                        output = new ColorStream(input_a.r || input_b.r, input_a.g || input_b.g,
                            input_a.b || input_b.b);
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
        all_types = new Stack<ModuleType>();
        all_types.Push(init_type);

        map = MapControl.getInstance();
        state = GetComponentInChildren<UIStateItem>();
        module_sprite = transform.GetChild(0).GetComponent<Image>();

        int sibling_index = transform.GetSiblingIndex();
        index_x = sibling_index / map.n;
        index_y = sibling_index % map.n;
    }

    public void InputState(int direct,ColorStream color)
    {
        switch(current_type)
        {
            case ModuleType.SignalGen:
            case ModuleType.BridgeOut:
                {
                    if (direct == 4)
                    {
                        allInputs[0] = new ColorStream(color);
                    }
                    break;
                }
            case ModuleType.SignalRev:
            case ModuleType.BridgeIn:
            case ModuleType.SPipe:
            case ModuleType.TPipe:
            case ModuleType.XPipe:
            case ModuleType.NotGate:
                {
                    if(direct==this.direct)
                    {
                        allInputs[direct] = new ColorStream(color);
                    }
                    break;
                }
            case ModuleType.AndGate:
            case ModuleType.OrGate:
                {
                    if (direct == this.direct || direct == (this.direct + 3) % 4)
                    {
                        allInputs[direct] = new ColorStream(color);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

        if(direct<0||direct>3)
        {
            return;
        }
        allInputs[direct]=new ColorStream(color);
        OutputState();
    }

    public virtual void OutputState()
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
                    BaseModule next = GetNextModule((direct+2)%4);
                    if(next!=null)
                    {
                        next.InputState(direct,out_state);
                    }
                    break;
                }
            case ModuleType.TPipe:
                {
                    BaseModule next_a = GetNextModule((direct + 1) % 4);
                    if (next_a != null)
                    {
                        next_a.InputState(direct, out_state);
                    }
                    BaseModule next_b = GetNextModule((direct + 3) % 4);
                    if (next_b != null)
                    {
                        next_b.InputState(direct, out_state);
                    }
                    break;
                }
            case ModuleType.XPipe:
                {
                    for(int i=1;i<4;i++)
                    {
                        BaseModule next = GetNextModule((direct + i) % 4);
                        if (next != null)
                        {
                            next.InputState(direct, out_state);
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
        all_types.Push(type);

        string sprite_name=type.ToString();
        string resource_locate = string.Format("ModuleSprites/{0}",sprite_name);

        this.module_sprite.sprite = Resources.Load<Sprite>(resource_locate);
        this.direct = direct;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (map.mouse_state != DragType.Empty)
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
            if (map.mouse_state != DragType.Empty&&isValid)
            {
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
                module_sprite.transform.rotation = map.drag_item.rotation;
                SetModule(module_type, map.direct);
            }
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        state.SetStateDefault();
        
    }
}
