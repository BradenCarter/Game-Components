using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIValueDisplayManager
{
    public UIValueDisplayManager(Action refreshAction)
    {
        this.refreshAction = refreshAction;
    }

    public int value;
    private event Action refreshAction;
    public event Action<int> OnValueChanged;

    public void UpdateToServer()
    {
        refreshAction?.Invoke();
        OnValueChanged?.Invoke(value);
    }
}
