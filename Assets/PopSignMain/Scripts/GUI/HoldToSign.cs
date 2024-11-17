using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    //using TMPro;

// still not saving properly, look at ToggleSwitch.cs for toggle behavior
public class HoldToSign : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed;
    public bool isShot = true;
    [SerializeField] private GameObject hands;

    [SerializeField] private Sprite signSprite;
    [SerializeField] private Sprite shootSprite;

    private Image imageComponent;

    // refers to toggle
    [SerializeField] private Slider slider;

    void Awake()
    {
        imageComponent = GetComponent<Image>();

        if (slider != null)
        {
            bool isVisible = slider.value > 0;
            gameObject.SetActive(isVisible);

            if (isVisible)
            {
                imageComponent.sprite = signSprite; // Initialize with the "signSprite"
            }

            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void Start()
    {
        if (slider != null)
        {
            OnSliderValueChanged(slider.value);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        isPressed = false;
        isShot = true;
    }

    void Update()
    {
        // Handle shot logic
        if (isShot && !Input.GetMouseButton(0))
        {
            hands.GetComponent<HandsMediaPipe>().lockOutTimeLeft -= Time.deltaTime;
        }

        // Change sprite based on conditions
        if ((hands.GetComponent<HandsMediaPipe>().handInFrame || !isShot) &&
            hands.GetComponent<HandsMediaPipe>().lockOutTimeLeft <= 0)
        {
            imageComponent.sprite = shootSprite;
        }
        else
        {
            imageComponent.sprite = signSprite;
        }
    }

    public void ToggleShotInput(bool isEnabled)
    {
        isShot = isEnabled;
    }

    private void OnSliderValueChanged(float value)
    {

        gameObject.SetActive(value > 0);
    }
}

