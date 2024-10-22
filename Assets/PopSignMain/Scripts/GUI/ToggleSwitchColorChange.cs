using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitchColorChange : ToggleSwitch
{
    // elements to recolor
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image handleImage;

    // if want element to be recolored
    [SerializeField] private bool recolorBackground;
    [SerializeField] private bool recolorHandle;

    // color components
    [SerializeField] private Color backgroundColorOff = Color.white;
    [SerializeField] private Color backgroundColorOn = Color.white;

    [SerializeField] private Color handleColorOff = Color.white;
    [SerializeField] private Color handleColorOn = Color.white;

    private bool _isBackgroundImageNotNull;
    private bool _isHandleImageNotNull;
    
    protected override void OnValidate()
    {
        base.OnValidate();
            
        CheckForNull();
        ChangeColors();
    }

    private void OnEnable()
    {
        transitionEffect += ChangeColors;
    }
        
    private void OnDisable()
    {
        transitionEffect -= ChangeColors;
    }

    protected override void Awake() 
    {
        base.Awake();
            
        CheckForNull();
        ChangeColors();
    }

    private void CheckForNull()
    {
        _isBackgroundImageNotNull = backgroundImage != null;
        _isHandleImageNotNull = handleImage != null;
    }


    private void ChangeColors()
    {
        if (recolorBackground && _isBackgroundImageNotNull)
            backgroundImage.color = Color.Lerp(backgroundColorOff, backgroundColorOn, sliderValue); 
            
        if (recolorHandle && _isHandleImageNotNull)
            handleImage.color = Color.Lerp(handleColorOff, handleColorOn, sliderValue); 
    }
}
