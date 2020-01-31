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

    [Header("Sprite Images")]
    [SerializeField] private Sprite[] sprites;

    [Header("Zum testen")]
    [SerializeField] private float testThreshold = -10f;

    private bool isOnConveyor = true;
    private bool isPickedUp = false;
    private float elapsedTime = 0f;

    public bool IsPickedUp { get => isPickedUp; }
    public ItemType Type { get => type; }

    private void Awake()
    {
        Array values = Enum.GetValues(typeof(ItemType));
        type = (ItemType)values.GetValue(new System.Random().Next(values.Length));
        sprite.GetComponent<SpriteRenderer>().sprite = sprites[Math.Min((int)type, sprites.Length - 1)];
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
        elapsedTime += Time.deltaTime;
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

    public void Drop()
    {
        if (isPickedUp)
        {
            StartCoroutine(DecayCoroutine(graceTime));
            isPickedUp = false;
        }
    }

    private IEnumerator DecayCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
