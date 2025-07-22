using Game.Helpers;
using UnityEngine;

public class UIPooler : GamePooler<TypeResource>
{
    [SerializeField] private Transform tfmParrentUI;
    public override void GetFromPool(GameObject obj)
    {
        base.GetFromPool(obj);
        obj.transform.SetParent(tfmParrentUI, false);
    }

    public override void ReturnToPool(GameObject obj)
    {
        base.ReturnToPool(obj);
    }
}
