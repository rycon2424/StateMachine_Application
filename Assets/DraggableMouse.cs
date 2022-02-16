using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableMouse : Draggable, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == EnumToID())
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        }
    }
}
