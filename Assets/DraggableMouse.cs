using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableMouse : Draggable, IDragHandler
{
    [SerializeField] int offsetFromBackground = 15;
    void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -offsetFromBackground);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == EnumToID())
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -offsetFromBackground);
        }
    }
}
