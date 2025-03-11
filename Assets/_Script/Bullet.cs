using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 3f;
    private Vector3 direction;
    private bool hasHit = false;
    private TrailRenderer trailRenderer;

    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }
    }

    public void Initialize(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
        Destroy(gameObject, lifetime);

        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }
    }

    void Update()
    {
        if (!hasHit)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasHit)
        {
            hasHit = true;
            //Debug.Log($"Bullet hit: {other.gameObject.name}, Tag: {other.gameObject.tag}");

            Destroy(gameObject);
        }
    }
}
