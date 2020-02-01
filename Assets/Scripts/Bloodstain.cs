using UnityEngine;

public class Bloodstain : MonoBehaviour
{
    [SerializeField]
    private Sprite[] pool;
    [SerializeField]
    private float _despawnTime = 10.0f;
    [SerializeField]
    private float _fadeDuration = 2.0f;
    private SpriteRenderer _renderer;
    private float _spawnTime = 0;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = pool[Random.Range(0, pool.Length -1)];
        this.transform.localRotation = Quaternion.Euler(0,0,Random.Range(0,360));
        _spawnTime = Time.time;
    }

    private void Update()
    {
        float lifeTime = Time.time - _spawnTime; 
        
        if (lifeTime > (_despawnTime + _fadeDuration))
        {
            Destroy(this.gameObject);
        }
        else if (lifeTime > _despawnTime)
        {
            float t = (lifeTime - _despawnTime) / _fadeDuration;
            float alpha = (1 - t);
            Color c = _renderer.color;
            c.a = alpha;
            _renderer.color = c;
        }
    }
}
