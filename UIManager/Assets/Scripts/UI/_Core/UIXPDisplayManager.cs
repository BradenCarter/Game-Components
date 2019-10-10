using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIXPDisplayManager 
{
    public int currentXP { get; private set; }
    public int currentLevel { get; private set; }
    public int currentLevelXP { get; private set; }

    public int currentLevelXPDelta { get; private set; }
    public event Action OnValueChanged;

    public void UpdateToServer()
    {
        //currentXP = PlayerManager.Instance.XP;
        //currentLevel = PlayerManager.Instance.Level;
        //currentLevelXP = PlayerManager.Instance.GetCurrentExperienceDelta();
        //currentLevelXPDelta = PlayerManager.Instance.CurrentLevelXPDelta;

        OnValueChanged?.Invoke();
    }
}
