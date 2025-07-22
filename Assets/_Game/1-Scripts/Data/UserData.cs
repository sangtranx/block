using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Data;
using UnityEngine.Serialization;

[Serializable]
public class UserData
{
     public bool isClaimInLevel;
    public List<ResourceData> lstResourceData = new List<ResourceData>();
    
    public UserData()
    {
        NewThis();
    }
    void NewThis()
    {
        lstResourceData.Add(new ResourceData(TypeResource.Coin, 0));
        lstResourceData.Add(new ResourceData(TypeResource.Score, 0));
        lstResourceData.Add(new ResourceData(TypeResource.Level, 1));
        lstResourceData.Add(new ResourceData(TypeResource.Point, 0));
        lstResourceData.Add(new ResourceData(TypeResource.DefeatChest, 0));

        //item
        lstResourceData.Add(new ResourceData(TypeResource.Bom3x3, 0));
        lstResourceData.Add(new ResourceData(TypeResource.Rocket, 0));
        lstResourceData.Add(new ResourceData(TypeResource.Fire, 0));
        lstResourceData.Add(new ResourceData(TypeResource.Thunder, 0));
        lstResourceData.Add(new ResourceData(TypeResource.ReSpawn, 0));
    }
    public ResourceData GetResourceByType(TypeResource typeResource)
    {
        return lstResourceData.Find(ex => ex.typeResource == typeResource);
    }

    public void SetClaimInLevel(bool isClaim)
    {
        isClaimInLevel = isClaim;
        SaveDB();
    }

    public void AddResourceByType(TypeResource typeResource, long amount)
    {
        if (typeResource == TypeResource.Coin)
        {
            //SoundManager.Instance.OnUISoundPlay(4);
            // AudioController.Instance.Play(AudioName.Sound_AddCoin);
        }
        var resource = GetResourceByType(typeResource);
        GameEvent.onResourceGonnaChange?.Invoke(resource.amount);
        resource.amount += amount;
        GameEvent.onUpdateResource?.Invoke();
        SaveDB();
    }

    public bool SubResourceByType(TypeResource typeResource, long amount)
    {
        var resource = GetResourceByType(typeResource);
        GameEvent.onResourceGonnaChange?.Invoke(resource.amount);
        if (resource.amount >= amount)
        {
            resource.amount -= amount;
            if (typeResource == TypeResource.Coin)
            {
                //SoundManager.Instance.OnUISoundPlay(7);
                AudioController.Instance.Play(AudioName.Sound_SubCoin);
            }
            
            SaveDB();
            return true;
        }
        return false;
    }

    private void SaveDB()
    {
        DBController.Instance.USER_DATA = this;
    }
}

[Serializable]
public class ResourceData
{
    public TypeResource typeResource;
    public long amount;

    public ResourceData(TypeResource typeResource, int amount)
    {
        this.typeResource = typeResource;
        this.amount = amount;
        if (amount < 0) amount = 0;
    }
}
