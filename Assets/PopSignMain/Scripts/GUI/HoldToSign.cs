using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    //using TMPro;
    
public class HoldToSign : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed;
    public bool isShot = true;
    [SerializeField] private GameObject hands;

    [SerializeField] private Sprite signSprite;
    [SerializeField] private Sprite shootSprite;

    private Image imageComponent;

    [SerializeField] private Slider slider;

    void Awake()
    {
        // Ensure the Image component is assigned here
        imageComponent = GetComponent<Image>();

        if (slider != null)
        {
            // Ensure slider value is initialized correctly
            slider.value = Mathf.Clamp(slider.value, 0, 1);
            slider.onValueChanged.AddListener(OnSliderValueChanged);

            Debug.Log("Slider Value in Awake: " + slider.value); // Debugging line
        }

        // Manually update visibility based on slider value right here in Awake
        UpdateButtonVisibility();
    }

    void Start()
    {
        // Ensure the visibility is set correctly at the start of the scene
        if (slider != null)
        {
            Debug.Log("Slider Value in Start: " + slider.value); // Debugging line
            UpdateButtonVisibility();
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
        // Prevent errors if imageComponent is not assigned yet
        if (imageComponent == null) return;

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
        // Log and update button visibility when slider value changes
        Debug.Log($"Slider value changed to: {value}");
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        // Ensure visibility based on slider value
        if (slider != null)
        {
            bool isVisible = slider.value > 0;
            Debug.Log($"Slider Value: {slider.value}, Button Visible: {isVisible}");
            gameObject.SetActive(isVisible);
        }
    }
}

