using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Range(0, 1f)]
    protected float sliderValue;
    public bool CurrentValue { get; private set; }

    private bool _previousValue;
    private Slider _slider;

    [SerializeField, Range(0, 1f)] private float animationDuration = 0.25f;
    [SerializeField] private AnimationCurve slideEase =
    AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine _animateSliderCoroutine;

    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;

    private ToggleSwitchGroupManager _toggleSwitchGroupManager;
    protected Action transitionEffect;

    [SerializeField] private string playerPrefsKey = "";

    protected virtual void OnValidate()
    {
        SetupToggleComponents();

        if (string.IsNullOrEmpty(playerPrefsKey))
        {
            playerPrefsKey = $"{gameObject.name}_ToggleSwitchState";
        }
        _slider.value = sliderValue;
    }

    private void SetupToggleComponents()
    {
        if (_slider != null)
            return;

        SetupSliderComponent();
    }

    private void SetupSliderComponent()
    {
        _slider = GetComponent<Slider>();

        if (_slider == null)
        {
            Debug.Log("No slider found!", this);
                return;
        }

        _slider.interactable = false;
        var sliderColors = _slider.colors;
        sliderColors.disabledColor = Color.white;
        _slider.colors = sliderColors;
        _slider.transition = Selectable.Transition.None;
    }

    public void SetupForManager(ToggleSwitchGroupManager manager)
    {
        _toggleSwitchGroupManager = manager;
    }

    protected virtual void Awake()
    {
        SetupSliderComponent();
        if (string.IsNullOrEmpty(playerPrefsKey))
        {
            playerPrefsKey = $"{gameObject.name}_ToggleSwitchState";
        }
        LoadState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    private void Toggle()
    {
        if (_toggleSwitchGroupManager != null)
            _toggleSwitchGroupManager.ToggleGroup(this);
        else
            SetStateAndStartAnimation(!CurrentValue);
    }

    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        _previousValue = CurrentValue;
        CurrentValue = state;

        if (_previousValue != CurrentValue)
        {
            if (CurrentValue)
                onToggleOn?.Invoke();
            else
                onToggleOff?.Invoke();
        }

        SaveState(); 

        if (_animateSliderCoroutine != null)
            StopCoroutine(_animateSliderCoroutine);

        _animateSliderCoroutine = StartCoroutine(AnimateSlider());
    }
    
    private IEnumerator AnimateSlider()
    {
        float startValue = _slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;

                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                _slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                transitionEffect?.Invoke();
                        
                yield return null;
            }
        }

        _slider.value = endValue;
    }

    private void SaveState()
    {
        if (!string.IsNullOrEmpty(playerPrefsKey))
        {
            PlayerPrefs.SetInt(playerPrefsKey, CurrentValue ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    private void LoadState()
    {
        if (!string.IsNullOrEmpty(playerPrefsKey) && PlayerPrefs.HasKey(playerPrefsKey))
        {
            int savedValue = PlayerPrefs.GetInt(playerPrefsKey);
            bool isOn = savedValue == 1;
            SetStateAndStartAnimation(isOn);
        }
    }
}
