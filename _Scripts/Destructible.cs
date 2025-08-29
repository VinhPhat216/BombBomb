using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private float destructionTime;

    [Range(0f, 1f)]
    [SerializeField] private float itemSpawnChance;
    [SerializeField] private GameObject[] spawnableItems;

    private void Start()  
    {
        Destroy(gameObject,destructionTime);
    }

    private void OnDestroy()
    {
        int randomIndex = Random.Range(0,spawnableItems.Length);
        Instantiate(spawnableItems[randomIndex],transform.position,Quaternion.identity);
    }
}
