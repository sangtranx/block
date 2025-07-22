using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupSettingsSO", menuName = "SO/PopupSettingsSO")]
public class PopupSettingsSO : ScriptableObject
{
    public List<SpriteButtons> spriteButtons;
    public SpriteButtons GetSpriteButtonSettingsByType(TypeBtnSettings typeBtnSettings) => spriteButtons.Find(ex => ex.typeBtnSettings == typeBtnSettings);
}

public enum TypeButtonState
{
    On = 0,
    Off = 1,
}

public enum TypeBtnSettings
{
    Music = 0,
    Sound = 1,
}

[Serializable]
public class SpriteButton
{
    public TypeButtonState typeButton;
    public Sprite sprBtn;
}

[Serializable]
public class SpriteButtons
{
    public TypeBtnSettings typeBtnSettings;
    public List<SpriteButton> lstSprButton;

    public SpriteButton GetSpriteButtonByType(TypeButtonState typeButton) => lstSprButton.Find(ex => ex.typeButton == typeButton);
}

