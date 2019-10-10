using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

public class OptionsUI : Menu<OptionsUI>
{
    public override void OnBack_Pressed()
    {
        base.OnBack_Pressed();
        MainMenuUI.Open();
    }
}