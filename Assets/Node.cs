using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public NodeAble from;
    public NodeAble to;
    [Space]
    public LineRenderer lr;
    //public GameObject arrow;

    private void Update()
    {
        if (lr != null)
        {
            lr.SetPosition(0, (from.transform.position + (from.transform.forward * 2.5f)));
            lr.SetPosition(1, (to.transform.position + (to.transform.forward * 2.5f)));
            //arrow.transform.position = (to.transform.position - from.transform.forward) / 2;
        }
    }
}
