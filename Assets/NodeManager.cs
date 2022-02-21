using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeAble currentHoveringNode;

    public static void ConnectNodes(Node node, NodeAble from, NodeAble to)
    {
        node.from = from;
        node.to = to;
        node.lr = from.currentNode;
    }
}
