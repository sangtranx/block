using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTextController : MonoBehaviour
{
    [SerializeField] GameObject comboTextPrefab;
    [SerializeField] GameObject instanPlace;
    public void Spawn(int value)
    {
        //instanPlace = GameObject.FindGameObjectWithTag("TextInstancePlace");
        if(value <= 0) return;
        AudioController.Instance.Play(AudioName.Sound_Clear);
        var f = Instantiate(comboTextPrefab, new Vector2(0, 1), Quaternion.identity, instanPlace.transform);
        var ui = f.GetComponent<ComboTextUI>();
        ui.Set(value);
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn(Random.Range(1, 4));
        }
    }
}
