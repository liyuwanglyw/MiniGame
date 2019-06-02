using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

public class BaseModule : MonoBehaviour
{
    public bool isPolluted;
    public int direct;
    private int index_x, index_y;
    private int[,] d= new int[,] { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };
    
    private ColorStream[] allInputs = new ColorStream[4];
    public enum ModuleType
    {
        Valid,
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

    private ModuleType init_type;
    private ModuleType current_type;

    public ColorStream out_state
    {
        get
        {
            ColorStream output;
            switch (current_type)
            {
                case ModuleType.Valid:
                case ModuleType.Invalid:
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
        int sibling_index = transform.GetSiblingIndex();
        index_x = sibling_index / MapControl.n;
        index_y = sibling_index % MapControl.n;
    }

    public void InputState(int direct,ColorStream color)
    {
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
            case ModuleType.Valid:
            case ModuleType.Invalid:
            case ModuleType.SignalRev:
            default:
                {
                    break;
                }
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
        if(next_x<0||next_x>=MapControl.m)
        {
            return false;
        }

        if (next_y < 0 || next_y >= MapControl.n)
        {
            return false;
        }
        return true;
    }

    public BaseModule GetNextModule(int direct)
    {
        if(IsNextValid(direct))
        {
            return MapControl.modules[index_x + d[direct, 0], index_y + d[direct, 1]];
        }
        else
        {
            return null;
        }
    }
}
