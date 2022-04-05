using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeAble : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject nodePrefab;
    [SerializeField] bool hovered;
    [Space]
    public LineRenderer currentNode;

    [HideInInspector] public Block block;

    void Awake()
    {
        block = GetComponent<Block>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        NodeManager.currentHoveringNode = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        NodeManager.currentHoveringNode = null;
    }

    private void Update()
    {
        if (currentNode != null)
        {
            Vector3 calculatedTempPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            calculatedTempPos = new Vector3(calculatedTempPos.x, calculatedTempPos.y, 0);
            currentNode.SetPosition(1, calculatedTempPos);

            if (Input.GetMouseButtonUp(1))
            {
                if (NodeManager.currentHoveringNode != null && NodeManager.currentHoveringNode != this)
                {
                    CreateConnection(NodeManager.currentHoveringNode, currentNode.GetComponent<Node>());
                    currentNode = null;
                }
                else
                {
                    Destroy(currentNode.gameObject);
                    currentNode = null;
                }
            }
        }
        if (hovered == false)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            Inspector.instance.CleanInspector();

            currentNode = Instantiate(nodePrefab, transform).GetComponent<LineRenderer>();
            currentNode.material.color = GetComponent<Image>().color;
            currentNode.transform.localPosition = Vector3.zero;
            currentNode.SetPosition(0, transform.position + new Vector3(0, 0, 0));
        }
    }

    public void CreateConnection(NodeAble to, Node cur = null, List<Conditions> extraCons = null)
    {
        if (currentNode == null)
        {
            currentNode = Instantiate(nodePrefab, transform).GetComponent<LineRenderer>();
            currentNode.material.color = GetComponent<Image>().color;
            currentNode.transform.localPosition = Vector3.zero;

            Node nodeToLoad = currentNode.GetComponent<Node>();

            NodeManager.ConnectNodes(currentNode.GetComponent<Node>(), this, to);

            nodeToLoad.LoadConditions(extraCons);
        }
        else
        {
            NodeManager.ConnectNodes(cur, this, to);
        }

        block.connections.Add(currentNode.GetComponent<Node>());
        currentNode = null;
    }

}
