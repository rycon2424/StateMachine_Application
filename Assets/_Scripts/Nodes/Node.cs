using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public NodeAble from;
    public NodeAble to;
    [Space]
    public LineRenderer lr;
    public List<Conditions> cons = new List<Conditions>();

    private void Update()
    {
        if (lr != null)
        {
            lr.SetPosition(0, (from.transform.position + (from.transform.forward * 3.5f)));
            if (to)
                lr.SetPosition(1, (to.transform.position + (to.transform.forward * 3.5f)));
            lr.material.mainTextureOffset -= new Vector2(0.3f, 0) * Time.deltaTime;
            lr.material.color = from.block.imagecolor;
            if (lr.material.mainTextureOffset.x <= -1.0f)
            {
                lr.material.mainTextureOffset = Vector2.zero;
            }
            if (!to)
            {
                from.block.connections.Remove(this);
                Destroy(this.gameObject);
            }
        }
    }

    public void LoadConditions(List<Conditions> loadedCons)
    {
        cons.AddRange(loadedCons);
    }
}
