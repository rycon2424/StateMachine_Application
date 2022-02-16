using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public enum Pointer { Leftclick, Rightclick, Middlemouse };
    public Pointer pointer;

    protected int EnumToID()
    {
        switch (pointer)
        {
            case Pointer.Leftclick:
                return -1;
            case Pointer.Rightclick:
                return -2;
            case Pointer.Middlemouse:
                return -3;
            default:
                break;
        }
        return 0;
    }
}
