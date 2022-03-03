using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationControl : MonoBehaviour
{
    [SerializeField] float maxZoom = 3.0f;
    [SerializeField] float minZoom = 0.5f;
    [SerializeField] float camSpeed = 3.0f;
    [SerializeField] GameObject block;
    [SerializeField] Transform editView;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        GridMovement();
    }

    void GridMovement()
    {
        if (Input.GetMouseButton(2))
        {
            float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
            float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
            mainCam.transform.position += new Vector3(mouseX, mouseY, 0) * camSpeed;
        }

        float mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll < 0 && mainCam.orthographicSize < maxZoom)
        {
            mainCam.orthographicSize *= 1.1f;
        }
        if (mainCam.orthographicSize > maxZoom)
        {
            mainCam.orthographicSize = maxZoom;
        }

        if (mouseScroll > 0 && mainCam.orthographicSize > minZoom)
        {
            mainCam.orthographicSize /= 1.1f; 
        }
        if (mainCam.orthographicSize < minZoom)
        {
            mainCam.orthographicSize = minZoom;
        }
    }

    public void SpawnBlock()
    {
        Vector3 point = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
        GameObject spawnedBlock = Instantiate(block, editView);
        spawnedBlock.GetComponent<RectTransform>().position = point;
    }

}
