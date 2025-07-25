using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFloatingController : MonoBehaviour
{
    [SerializeField] GameObject pointFloatingPrefab;
    [SerializeField] GameObject instanPlace;
    public void Spawn(int value, Vector2 startPos, float time = 0.5f)
    {
        // instanPlace = GameObject.FindGameObjectWithTag("TextInstancePlace");
        //var f = Instantiate(pointFloatingPrefab, startPos, Quaternion.identity, instanPlace.transform);
        //var ui = f.GetComponent<PointFloatingUI>();
        //ui.Set(value, time);
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Spawn(Random.Range(100, 50000), new Vector2(Random.Range(-2, 2), Random.Range(-4, 4)), 0.9f);
        //}
    }
}
