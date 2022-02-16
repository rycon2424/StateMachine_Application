using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationControl : MonoBehaviour
{
    [SerializeField] float maxZoom = 3.0f;
    [SerializeField] float minZoom = 0.5f;
    [SerializeField] RectTransform editView;

    void Start()
    {
        
    }

    void Update()
    {
        float mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll > 0 && editView.localScale.x < maxZoom)
        {
            editView.localScale *= 1.1f;
        }
        if (editView.localScale.x > maxZoom)
        {
            editView.localScale = new Vector3(maxZoom, maxZoom, maxZoom);
        }

        if (mouseScroll < 0 && editView.localScale.x > minZoom)
        {
            editView.localScale /= 1.1f;
        }
        if (editView.localScale.x < minZoom)
        {
            editView.localScale = new Vector3(minZoom, minZoom, minZoom);
        }
    }

}
