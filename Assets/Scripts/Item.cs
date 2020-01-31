using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public ItemType type;

	public float timeToDecay = 10.0f;

    void Start()
    {
        Destroy(gameObject, timeToDecay);
    }
}
