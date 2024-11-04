using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float lifeTime;
    [SerializeField] AudioClip hurtSFX;

    float direction;
    bool hit;

    BoxCollider2D boxCollider;
    Animator animator;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (hit)
        {
            return;
        }

        float movementSpeed = Time.deltaTime * speed * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifeTime += Time.deltaTime;
        if (lifeTime > 5)
        {
            Deactivate();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            AudioSource.PlayClipAtPoint(hurtSFX, Camera.main.transform.position);
        }
        animator.SetTrigger("Explode");
    }

    public void SetDirection(float _direction)
    {
        lifeTime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;


        float localScalex = transform.localScale.x;
        if (Mathf.Sign(localScalex) != _direction)
        {
            localScalex = -localScalex;
        }

        transform.localScale = new Vector3(localScalex, transform.localScale.y, transform.localScale.z);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
