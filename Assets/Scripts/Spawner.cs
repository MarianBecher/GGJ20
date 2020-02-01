using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float defaultSpawnDelay = 3f;
    [SerializeField] private Item itemPrefab;
    [SerializeField] private bool movesLeft = true;
    [SerializeField] private WorkingBench preferedBench;

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
            _SpawnRandomItem();
            RandomSpawnDelay();
        }
    }

    private Item _SpawnRandomItem()
    {

        Item item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        item.SetItemType(_GetRandomItemType());

        if (movesLeft)
        {
            item.GetComponent<Item>().SetMoveToLeft();
        }
        else
        {
            item.GetComponent<Item>().SetMoveToRight();
        }
        return item;
    }

    private ItemType _GetRandomItemType()
    {
        bool cheat = Random.value < 0.5f;
        
        if(cheat && preferedBench)
        {
            ItemType[] missingParts = preferedBench.MissingParts;
            return missingParts[Random.Range(0, missingParts.Length)];
        }
        else
        {
            System.Array values = System.Enum.GetValues(typeof(ItemType));
            return (ItemType)values.GetValue(Random.Range(0, values.Length));
        }

    }

    private void RandomSpawnDelay()
    {
        spawnDelay = Random.Range(defaultSpawnDelay * 0.75f, defaultSpawnDelay * 1.25f);
    }
}
