using DG.Tweening;
using TMPro;
using UnityEngine;

public class PointFloatingUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txValue;
    public void Set(int value, float time)
    {
        txValue.text = $"+{value.ToString("N0")}";
        txValue.transform.DOMoveY(transform.position.y + 0.3f, time).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
