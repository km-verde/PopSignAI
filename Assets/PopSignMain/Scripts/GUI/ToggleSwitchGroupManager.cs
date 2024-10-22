using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitchGroupManager : MonoBehaviour
{
    [SerializeField] private ToggleSwitch initialToggleSwitch;

    private List<ToggleSwitch> _toggleSwitches = new List<ToggleSwitch>();

    private void Awake()
    {
        ToggleSwitch[] toggleSwitches = GetComponentsInChildren<ToggleSwitch>();
        foreach (var toggleSwitch in toggleSwitches)
        {
            RegisterToggleButtonToGroup(toggleSwitch);
        }
    }

    private void RegisterToggleButtonToGroup(ToggleSwitch toggleSwitch)
    {
        if (_toggleSwitches.Contains(toggleSwitch))
            return;
            
        _toggleSwitches.Add(toggleSwitch);
            
        toggleSwitch.SetupForManager(this);
    }

    private void Start()
    {
        bool areAllToggledOff = true;
        foreach (var button in _toggleSwitches)
        {
            if (!button.CurrentValue) 
                continue;
                
            areAllToggledOff = false;
            break;
        }

        if (!areAllToggledOff)
            return;
            
        if (initialToggleSwitch != null)
            initialToggleSwitch.ToggleByGroupManager(true);
        else
            _toggleSwitches[0].ToggleByGroupManager(true);
    }

    public void ToggleGroup(ToggleSwitch toggleSwitch)
    {
        if (_toggleSwitches.Count <= 1)
            return;

        // check that at least one toggle is on
        bool isAnyOtherToggleOn = false;
        foreach (var button in _toggleSwitches)
        {
            if (button != null && button != toggleSwitch && button.CurrentValue)
            {
                isAnyOtherToggleOn = true;
                break;
            }
        }

        // if other toggles are not on, prevent from turning off
        if (!isAnyOtherToggleOn && toggleSwitch.CurrentValue)
        {
            return; // Prevent turning off if it's the last one on
        }

        // allow toggles to be manipulated as long as at least one other toggle remains true
        toggleSwitch.ToggleByGroupManager(!toggleSwitch.CurrentValue);
    }
}

