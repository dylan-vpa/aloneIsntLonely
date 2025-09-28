using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float lifetime = 5f; // Tiempo antes de destruirse
    public int damage = 1;

    [Header("Visual Effects")]
    public GameObject hitEffect;
    public bool destroyOnHit = true;

    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        Debug.Log("🚀 Proyectil iniciado!");
        
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f; // sin gravedad
            rb.linearDamping = 0f; // sin resistencia
            rb.freezeRotation = true; // no rotar por física
            Debug.Log("✅ Rigidbody2D configurado correctamente");
        }
        else
        {
            Debug.LogWarning("⚠️ No hay Rigidbody2D, usando Transform");
        }

        Destroy(gameObject, lifetime); // autodestruirse después del tiempo de vida
        Debug.Log($"⏰ Proyectil se destruirá en {lifetime} segundos");
    }

    void Update()
    {
        if (rb != null)
        {
            Vector2 velocity = direction * speed;
            rb.linearVelocity = velocity;
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    // Método para establecer la dirección del disparo
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        Debug.Log($"🎯 Dirección establecida: {direction}");

        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Debug.Log($"🎯 Rotación establecida: {angle} grados");
        }
    }

    // Colisiones
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignorar al jugador
        if (other.CompareTag("Player")) return;

        // Si golpea a un enemigo
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"Proyectil golpeó enemigo: {other.name}");
            
            // Obtener el script Enemy y hacer daño
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        
        // Si golpea al boss
        if (other.CompareTag("Boss"))
        {
            Debug.Log($"Proyectil golpeó boss: {other.name}");
            
            // Obtener el script Boss y hacer daño
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
        }

        // Si golpea a un obstáculo
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Proyectil golpeó obstáculo");
        }

        // Efecto visual
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        if (destroyOnHit) Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        if (destroyOnHit) Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
