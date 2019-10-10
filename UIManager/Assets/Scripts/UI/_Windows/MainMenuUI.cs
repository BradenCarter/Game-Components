using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

public class MainMenuUI : Menu<MainMenuUI>
{
    public void OnStart_Pressed()
    {
        Close();
    }

    public void OnOptions_Pressed()
    {
        Close();
        OptionsUI.Open();
    }
}