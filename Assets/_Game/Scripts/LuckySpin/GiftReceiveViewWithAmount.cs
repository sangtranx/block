using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GiftReceiveViewWithAmount : GiftReceiveView
{
    [SerializeField] TextMeshProUGUI txtAmount;
    public override void SetView(Gift gift)
    {
        base.SetView(gift);
        txtAmount.text = gift.Amount.ToString();
    }
}
