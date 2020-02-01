using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Item : Interactable
{
    [SerializeField] private ItemType type;
    [SerializeField] private float timeToDecay = 10.0f;
    [SerializeField] private float graceTime = 2f;
    [SerializeField] private float conveyorSpeed = 3f;
    [SerializeField] private bool conveyorMovesLeft = true;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject decayBar;

    [Header("Sprite Images")]
    [SerializeField] private Sprite[] sprites;

    [Header("Zum testen")]
    [SerializeField] private float testThreshold = -10f;

    private bool isOnConveyor = true;
    private bool isPickedUp = false;
    private float elapsedTime = 0f;
    private Image decayBarImage;

    public bool IsPickedUp { get => isPickedUp; }
    public ItemType Type { get => type; }

    protected override void Awake()
    {
        base.Awake();
        Array values = Enum.GetValues(typeof(ItemType));
        type = (ItemType)values.GetValue(new System.Random().Next(values.Length));
        sprite.GetComponent<SpriteRenderer>().sprite = sprites[Math.Min((int)type, sprites.Length - 1)];
        decayBarImage = decayBar.GetComponent<Image>();
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
            if(conveyorMovesLeft)
            {
                pos.x -= conveyorSpeed * Time.deltaTime;
            }
            else
            {
                pos.x += conveyorSpeed * Time.deltaTime;
            }
            transform.position = pos;

            // Debug
            if (transform.position.x <= testThreshold)
            {
                Destroy(gameObject);
            }
        }
        elapsedTime += Time.deltaTime;
        float percentage = elapsedTime / timeToDecay;
        decayBarImage.fillAmount = 1 - percentage;
        decayBarImage.color = new Vector4(percentage, 1 - percentage, 0, 1);
    }

    private void LateUpdate()
    {
        canvas.transform.rotation = Quaternion.identity;
    }

    public void Pickup()
    {
        if (!isPickedUp)
        {
            StopAllCoroutines();
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

    public void SetMoveToLeft()
    {
        conveyorMovesLeft = true;
    }

    public void SetMoveToRight()
    {
        conveyorMovesLeft = false;
    }
}
