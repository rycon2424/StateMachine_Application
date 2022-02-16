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
            transform.position = Input.mousePosition;
        }
    }
}
