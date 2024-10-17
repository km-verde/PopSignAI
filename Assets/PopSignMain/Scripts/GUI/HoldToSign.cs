using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    //using TMPro;
    
    public class HoldToSign: MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public bool isPressed;
        public bool isShot = true;
        [SerializeField] private GameObject hands;
    
        public void OnPointerDown(PointerEventData data)
        {
            isPressed = true;
            // if(isShot)
            //     TfLiteManager.Instance.StartRecording();

        }
        public void OnPointerUp(PointerEventData data)
        {
            isPressed = false;
            isShot = true;
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
                //label.SetText("Shoot");
                GetComponent<Image>().color = new Color32(170,255,182,255);
                //170, 255, 182
            }
            else
            {
                //label.SetText("Hold\nTo\nSign");
                GetComponent<Image>().color = new Color32(97, 97, 97,255);
                //97, 97, 97
            }
        }
    }
