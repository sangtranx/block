using Game.Helpers;
using UnityEngine;

public enum TypeGamePooler
{
    Character,
    MergeableCharacter,
    UI,
    Resource,
}
public class Poolers : MonoBehaviour
{
    //[SerializeField] CharacterPooler characterPooler;
    //[SerializeField] MergeableCharacterPooler mergeableCharacterPooler;
    //[SerializeField] ResourcePooler resourcePooler;
    [SerializeField] UIPooler uiPooler;
    [SerializeField] EffectPooler effectPooler;
    //public CharacterPooler CharacterPooler { get => characterPooler; }
    //public MergeableCharacterPooler MergeableCharacterPooler { get => mergeableCharacterPooler; }
    //public ResourcePooler ResourcePooler { get => resourcePooler; }
    public UIPooler UIPooler { get => uiPooler; }
    public EffectPooler EffectPooler { get => effectPooler; }
}