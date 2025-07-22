using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinGameplaySO", menuName = "SO/SkinGameplaySO")]
public class SkinGameplaySO : ScriptableObject
{
    public List<SkinGameplayModel> lstSkin;

    public SkinGameplayModel GetSkinByType(SkinType skinType) => lstSkin.Find(ex => ex.SkinType == skinType);
}
[Serializable]
public class SkinGameplayModel
{
    public SkinType SkinType;
    public Sprite sprItem;
}
