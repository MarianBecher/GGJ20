using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType type;
    [SerializeField] private float timeToDecay = 10.0f;
    [SerializeField] private float graceTime = 2f;
    [SerializeField] private float conveyorSpeed = 3f;
    [SerializeField] private GameObject sprite;

    [Header("Zum testen")]
    [SerializeField] private float testThreshold = -10f;

    private bool isOnConveyor = true;
    private bool isPickedUp = false;

    public bool IsPickedUp { get => isPickedUp; }
    public ItemType Type { get => type; }

    private void Awake()
    {
        Array values = Enum.GetValues(typeof(ItemType));
        type = (ItemType)values.GetValue(new System.Random().Next(values.Length));
    }

    private void Start()
    {
        Destroy(gameObject, timeToDecay);
    }

    private void Update()
    {
        if (isOnConveyor)
        {
            Vector3 pos = transform.position;
            pos.x -= conveyorSpeed * Time.deltaTime;
            transform.position = pos;

            // Debug
            if (transform.position.x <= testThreshold)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Pickup()
    {
        if (!isPickedUp)
        {
            StopCoroutine("DecayCoroutine");
            isOnConveyor = false;
            isPickedUp = true;
        }
    }

    private bool IsOnTable()
    {
        // check die position mit der tischposition
        return false;
    }

    public void Drop()
    {
        if (isPickedUp)
        {
            if (!IsOnTable())
            {
                StartCoroutine(DecayCoroutine(graceTime));
            }
            isPickedUp = false;
        }
    }

    private IEnumerator DecayCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
