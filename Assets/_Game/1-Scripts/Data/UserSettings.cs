using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeVolume
{
    Music = 0,
    Sound = 1,
}


[Serializable]
public class UserSettings
{
    public float music;
    public float sound;

    public UserSettings()
    {
        music = 1f;
        sound = 1f;
    }

    public void ChangeVolumeByType(TypeVolume typeVolume, float volume)
    {
        switch (typeVolume)
        {
            case TypeVolume.Music:
                music = volume;
                break;
            case TypeVolume.Sound:
                sound = volume;
                break;
        }
        DBController.Instance.USER_SETTINGS = this;
    }
}
