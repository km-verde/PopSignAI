using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    
    public class HoldToSign: MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public bool isPressed;
    
        public void OnPointerDown(PointerEventData data)
        {
            isPressed = true;
            TfLiteManager.Instance.StartRecording();

        }
        public void OnPointerUp(PointerEventData data)
        {
            isPressed = false;
            Debug.Log("Pointer Up");
        }
    }
