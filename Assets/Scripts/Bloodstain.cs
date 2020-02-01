using UnityEngine;

public class Bloodstain : MonoBehaviour
{
    [SerializeField]
    private Sprite[] pool;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = pool[Random.Range(0, pool.Length -1)];
        this.transform.localRotation = Quaternion.Euler(0,0,Random.Range(0,360));
    }
}
