using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeAble : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool hovered;
    [SerializeField] GameObject nodePrefab;
    [Space]
    [SerializeField] LineRenderer currentNode;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    private void Update()
    {
        if (currentNode != null)
        {

            currentNode.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)));
            if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Reset");
                currentNode = null;
            }
        }
        if (hovered == false)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            currentNode = Instantiate(nodePrefab, transform).GetComponent<LineRenderer>();
            currentNode.transform.localPosition = Vector3.zero;
            currentNode.SetPosition(0, transform.position + new Vector3(0, 0, 0));
        }
    }
}
