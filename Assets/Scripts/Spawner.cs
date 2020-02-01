using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float defaultSpawnDelay = 3f;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private bool movesLeft = true;

    private float spawnDelay;
    private float elapsedTime = 0f;

    private void Awake()
    {
        RandomSpawnDelay();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= spawnDelay)
        {
            elapsedTime = 0;
            RandomSpawnDelay();
            GameObject item = GameObject.Instantiate(itemPrefab, transform.position, Quaternion.identity);
            if(movesLeft)
            {
                item.GetComponent<Item>().SetMoveToLeft();
            }
            else
            {
                item.GetComponent<Item>().SetMoveToRight();
            }
        }
    }

    private void RandomSpawnDelay()
    {
        spawnDelay = Random.Range(defaultSpawnDelay * 0.75f, defaultSpawnDelay * 1.25f);
    }
}
