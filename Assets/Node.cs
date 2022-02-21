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
            lr.SetPosition(0, (from.transform.position + (from.transform.forward * 3.5f)));
            lr.SetPosition(1, (to.transform.position + (to.transform.forward * 3.5f)));
            lr.material.mainTextureOffset -= new Vector2(0.3f, 0) * Time.deltaTime;
            if (lr.material.mainTextureOffset.x <= -1.0f)
            {
                lr.material.mainTextureOffset = Vector2.zero;
            }
        }
    }
}
