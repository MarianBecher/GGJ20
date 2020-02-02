using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UberAudio;

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
    [SerializeField] private Collider2D collider;
    [SerializeField] private float shredderThreshold = 0f;

    [Header("Sprite Images")]
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Sprite[] highlights;
    private bool isDestroying = false;


    private bool isOnConveyor = true;
    private bool isPickedUp = false;
    private Image decayBarImage;
    private bool isFrozen = false;
    private float decayTimeElapsed = 0.0f;

    public bool IsPickedUp { get => isPickedUp; }
    public ItemType Type { get => type; }

    protected override void Awake()
    {
        base.Awake();
        Array values = Enum.GetValues(typeof(ItemType));
        type = (ItemType)values.GetValue(new System.Random().Next(values.Length));
        sprite.GetComponent<SpriteRenderer>().sprite = sprites[Math.Min((int)type, sprites.Length - 1)];
        highlight.GetComponent<SpriteRenderer>().sprite = highlights[Math.Min((int)type, sprites.Length - 1)];
        decayBarImage = decayBar.GetComponent<Image>();
    }

    public void SetItemType(ItemType type)
    {
        this.type = type;
        sprite.GetComponent<SpriteRenderer>().sprite = GetSprite(type);
        highlight.GetComponent<SpriteRenderer>().sprite = highlights[Math.Min((int)type, sprites.Length - 1)];
    }

    public Sprite GetSprite(ItemType type)
    {
        return sprites[Math.Min((int)type, sprites.Length - 1)];
    }

    private void Update()
    {
        if (!isFrozen)
        {
            decayTimeElapsed += Time.deltaTime;

            if (decayTimeElapsed >= timeToDecay)
            {
                Destroy(gameObject);
            }
        }

        if (isOnConveyor)
        {
            Vector3 pos = transform.position;
            if (conveyorMovesLeft)
            {
                pos.x -= conveyorSpeed * Time.deltaTime;
            }
            else
            {
                pos.x += conveyorSpeed * Time.deltaTime;
            }
            transform.position = pos;

            if (!isDestroying && Mathf.Abs(transform.position.x) <= shredderThreshold)
            {
                isDestroying = true;
                GetComponent<Animator>().SetTrigger("Shredder");
                collider.enabled = false;
                StartCoroutine(DelayedDestroy(1.0f));
            }
        }
        float percentage = decayTimeElapsed / timeToDecay;
        decayBarImage.fillAmount = 1 - percentage;
        decayBarImage.color = new Vector4(percentage, 1 - percentage, 0, 1);
    }

    private IEnumerator DelayedDestroy(float time)
    {
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.Play("Shredder");
        Destroy(gameObject);

    }

    private void LateUpdate()
    {
        canvas.transform.rotation = Quaternion.identity;
    }

    public void Pickup()
    {
        if (!isPickedUp)
        {
            isOnConveyor = false;
            isPickedUp = true;
            collider.enabled = false;
            AudioManager.Instance.Play("PickUp");
            Unfreeze();
        }
    }

    public void Drop()
    {
        if (isPickedUp)
        {
            AudioManager.Instance.Play("Drop");
            isPickedUp = false;
            collider.enabled = true;
        }
    }

    public void SetMoveToLeft()
    {
        conveyorMovesLeft = true;
    }

    public void SetMoveToRight()
    {
        conveyorMovesLeft = false;
    }

    public void Freeze()
    {
        isFrozen = true;
    }

    public void Unfreeze()
    {
        isFrozen = false;
    }
}
