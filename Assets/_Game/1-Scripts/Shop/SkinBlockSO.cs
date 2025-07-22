using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "SkinBlockSO", menuName = "SO/SkinBlockSO")]
public class SkinBlockSO : ScriptableObject
{ 
    public List<SkinBlock> lstSkinBlock;
    public SkinBlock GetSkinByType(SkinType skinType) => lstSkinBlock.Find(ex => ex.SkinType == skinType);
    
}

[Serializable]
public class SkinBlock
{
    public SkinType SkinType;
    /// <summary>
    /// Một bộ skin block sẽ có sự đa dạng ở mỗi ô của block
    /// </summary>
    public List<SpirteSkinBlock> lstSprIcon;
    public List<SpirteSkinBlock> lstCell;
    public SpirteSkinBlock GetSpriteById(int id) => lstSprIcon.Find(ex => ex.id == id);
    public SpirteSkinBlock GetSpriteByIdCell(int id) => lstCell.Find(ex => ex.id == id);
}

[Serializable]
public class SpirteSkinBlock
{
    public int id;
    public Sprite sprBlock;
}