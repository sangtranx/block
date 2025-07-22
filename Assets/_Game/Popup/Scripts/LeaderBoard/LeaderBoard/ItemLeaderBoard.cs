using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemLeaderBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtNumber;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private Image imgAvt;
    [SerializeField] private Image imgIconAvt;

    public void Init(ItemLeaderBoardModel model)
    {
        txtNumber.text = $"{model.number}";
        txtName.text = $"{model.name}";
        txtScore.text = $"{model.score}";
        imgAvt.sprite = model.sprAvt;
    }

    public void SetIconAvt(Sprite sprIcon)
    {
        imgIconAvt.sprite = sprIcon;
    }    

    public void SetAvt(Sprite sprIcon)
    {
        imgAvt.sprite = sprIcon;
    }    
}

[Serializable]
public class ItemLeaderBoardModel
{
    public int number;
    public string name;
    public double score;
    public Sprite sprAvt;
}
