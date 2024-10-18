using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

    public class HoldToSign: MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public bool isPressed;
        public bool isShot = true;

        [SerializeField] private TMP_Text label;
        [SerializeField] private GameObject hands;
    
        public void OnPointerDown(PointerEventData data)
        {
            isPressed = true;
            if(isShot)
                TfLiteManager.Instance.StartRecording();

        }

        public void OnPointerUp(PointerEventData data)
        {
            isPressed = false;
            isShot = true;
            Debug.Log("Pointer Up");
        }
        
        void Update()
        {
            if(isShot && !Input.GetMouseButton(0))
            {
               hands.GetComponent<HandsMediaPipe>().lockOutTimeLeft -= Time.deltaTime; 
            }
            if((hands.GetComponent<HandsMediaPipe>().handInFrame || !isShot)
                && hands.GetComponent<HandsMediaPipe>().lockOutTimeLeft <= 0)
            {
                label.SetText("Shoot");
            }
            else
            {
                label.SetText("Hold\nTo\nSign");
            }
        }
    }
