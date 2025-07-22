using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftRewardProbability : MonoBehaviour
{
    [Serializable]
    public class GiftRewardProbabilityData
    {
        [SerializeField] Gift gift;
        [SerializeField] float probability;
        float angle;
        public float Probability { get => probability; }
        public Gift Gift { get => gift; }
        public float Angle { get => angle; }

        public void CalculateAngleByIndex(int index, int maxCountGift)
        {
            angle = 360f / maxCountGift * index;
        }
    }
    [SerializeField] List<GiftRewardProbabilityData> lstGiftRewardProbabilityData;
    public void CalculateAngleGift()
    {
        for (int i = 0; i < lstGiftRewardProbabilityData.Count; i++)
        {
            lstGiftRewardProbabilityData[i].CalculateAngleByIndex(i, lstGiftRewardProbabilityData.Count);
        }
    }
    public (Gift gift, float angle) RandomGiftData()
    {
        float probability = UnityEngine.Random.Range(0, 10001) * 1f / 10000;
        float tmpValue = 0;
        for (int i = 0; i < lstGiftRewardProbabilityData.Count; i++)
        {
            tmpValue += lstGiftRewardProbabilityData[i].Probability / 100f;
            if (tmpValue > probability)
            {
                return (lstGiftRewardProbabilityData[i].Gift, lstGiftRewardProbabilityData[i].Angle);
            }
        }
        int rdIndex = UnityEngine.Random.Range(0, lstGiftRewardProbabilityData.Count);
        return (lstGiftRewardProbabilityData[rdIndex].Gift, lstGiftRewardProbabilityData[rdIndex].Angle);
    }
}
