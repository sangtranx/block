using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinBackgroundSO", menuName = "SO/SkinBackgroundSO")]
public class SkinBackgroundSO : ScriptableObject
{
    public List<SkinGameplayModel> lstSkin;

    public SkinGameplayModel GetSkinByType(SkinType skinType) => lstSkin.Find(ex => ex.SkinType == skinType);
}