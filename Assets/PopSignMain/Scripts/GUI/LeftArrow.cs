using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftArrow : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private GameObject line;
    [SerializeField] private float scaler = 0.1f;
    private bool pressed = false;
    public void OnPointerDown(PointerEventData data)
    {
        line.GetComponent<DrawLine>().arrowDown = true;
        pressed = true;
    }
    public void OnPointerUp(PointerEventData data)
    {
        line.GetComponent<DrawLine>().arrowDown = false;
        pressed = false;
    }

    void Update()
    {
        if(pressed)
        {
            MoveLeft();
        }
    }

    public void MoveLeft()
    {
        if(line.GetComponent<DrawLine>().dir.y < -3.5f && (line.GetComponent<DrawLine>().dir.x >= -2.5 && line.GetComponent<DrawLine>().dir.x <= 2.5))
            scaler = 0.03f;
        else
            scaler = 0.1f;
        line.GetComponent<DrawLine>().dir = line.GetComponent<DrawLine>().dir - (Vector3.right * scaler);
    }
}
