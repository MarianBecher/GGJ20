using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnDelay = 3f;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private bool movesLeft = true;

    private float elapsedTime = 0f;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= spawnDelay)
        {
            elapsedTime = 0;
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
}
