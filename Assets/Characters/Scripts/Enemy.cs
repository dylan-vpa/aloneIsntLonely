using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 3;
    public int currentHealth;
    
    [Header("Movement")]
    public float moveSpeed = 4f; // Aún más rápido
    public float detectionRange = 50f; // Rango infinito (muy alto)
    public float followSmoothness = 10f; // Muy agresivo
    public float avoidanceRadius = 0.5f; // Mínimo radio de evitación
    public float acceleration = 8f; // Más aceleración hacia el jugador
    
    [Header("Shadow Settings")]
    public bool useShadow = false; // Desactivado por defecto
    public float shadowOffset = 0.2f;
    public float shadowScale = 0.8f;
    public Color shadowColor = new Color(0, 0, 0, 0.3f);
    
    [Header("Scene Bounds")]
    public float sceneWidth = 48f;
    public float sceneHeight = 32f;
    public float margin = 5f;
    
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    // Variables para sombra
    private GameObject shadow;
    private SpriteRenderer shadowRenderer;
    private float shadowRotation = 0f;
    
    // Evento para notificar cuando el enemigo muere
    public System.Action OnEnemyDeath;
    
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Buscar al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        Debug.Log($"Enemy creado con {currentHealth} vidas");
        
        // Crear sombra si está habilitada
        if (useShadow)
        {
            CreateShadow();
        }
    }
    
    void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
            ClampToSceneBounds();
            UpdateShadow();
        }
    }
    
    void MoveTowardsPlayer()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // SIEMPRE perseguir al jugador - SIN LÍMITE DE DISTANCIA
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        
        // Evitar otros enemigos (mínimo)
        Vector2 avoidanceDirection = Vector2.zero;
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius);
        
        foreach (Collider2D enemy in nearbyEnemies)
        {
            if (enemy.gameObject != gameObject && enemy.CompareTag("Enemy"))
            {
                Vector2 awayFromEnemy = (transform.position - enemy.transform.position).normalized;
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                avoidanceDirection += awayFromEnemy * (1f - distance / avoidanceRadius) * 0.1f; // Muy poca evitación
            }
        }
        
        // Priorizar persecución del jugador SIEMPRE
        Vector2 finalDirection = (directionToPlayer + avoidanceDirection).normalized;
        
        // Movimiento agresivo hacia el jugador
        Vector2 targetVelocity = finalDirection * moveSpeed;
        
        // Aceleración extra si está lejos del jugador
        if (distanceToPlayer > 3f)
        {
            targetVelocity *= 1.3f; // Más aceleración cuando está lejos
        }
        else if (distanceToPlayer > 1f)
        {
            targetVelocity *= 1.1f; // Aceleración moderada cuando está cerca
        }
        
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, followSmoothness * Time.deltaTime);
        
        // Rotar hacia el jugador SIEMPRE
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 8f * Time.deltaTime);
        }
        
        // Debug ocasional para no spamear la consola
        if (Random.Range(0f, 1f) < 0.01f) // 1% de probabilidad
        {
            Debug.Log($"Enemy persiguiendo jugador - Distancia: {distanceToPlayer:F1}");
        }
    }
    
    void ClampToSceneBounds()
    {
        // Calcular límites con margen
        float minX = -sceneWidth/2 + margin;
        float maxX = sceneWidth/2 - margin;
        float minY = -sceneHeight/2 + margin;
        float maxY = sceneHeight/2 - margin;
        
        // Limitar posición
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
        
        // Si está en el borde, parar movimiento hacia ese lado
        if (pos.x <= minX || pos.x >= maxX)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        if (pos.y <= minY || pos.y >= maxY)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca con otro enemigo, parar un momento
        if (collision.gameObject.CompareTag("Enemy"))
        {
            rb.linearVelocity = Vector2.zero;
            Debug.Log("Enemy chocó con otro enemy");
        }
        
        // Si choca con obstáculo, parar
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.linearVelocity = Vector2.zero;
            Debug.Log("Enemy chocó con obstáculo");
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy recibió {damage} daño. Vidas restantes: {currentHealth}");
        
        // Efecto visual de daño
        StartCoroutine(FlashRed());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    System.Collections.IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
    
    void Die()
    {
        Debug.Log("Enemy muerto!");
        
        // Notificar que el enemigo murió
        OnEnemyDeath?.Invoke();
        
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
        // El enemigo tocó al jugador
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.PlayerHit();
        }
        }
    }
    
    void CreateShadow()
    {
        // Crear objeto de sombra
        shadow = new GameObject("EnemyShadow");
        shadow.transform.SetParent(transform);
        shadowRenderer = shadow.AddComponent<SpriteRenderer>();
        
        // Configurar sombra
        shadowRenderer.sprite = spriteRenderer.sprite;
        shadowRenderer.color = shadowColor;
        shadowRenderer.sortingOrder = -1; // Detrás del enemigo
        shadow.transform.localScale = Vector3.one * shadowScale;
        
        // Posición inicial de la sombra
        shadow.transform.localPosition = new Vector3(shadowOffset, -shadowOffset, 0);
        
        // Rotación inicial aleatoria
        shadowRotation = Random.Range(0f, 360f);
    }
    
    void UpdateShadow()
    {
        if (shadow != null && useShadow)
        {
            // Actualizar posición de la sombra con offset dinámico
            Vector3 shadowPos = new Vector3(
                shadowOffset + Mathf.Sin(Time.time * 2f) * 0.1f,
                -shadowOffset + Mathf.Cos(Time.time * 1.5f) * 0.1f,
                0
            );
            shadow.transform.localPosition = shadowPos;
            
            // Rotar sombra ligeramente para efecto dinámico
            shadowRotation += Time.deltaTime * 30f;
            shadow.transform.rotation = Quaternion.Euler(0, 0, shadowRotation);
            
            // Cambiar escala ligeramente
            float scaleVariation = 1f + Mathf.Sin(Time.time * 3f) * 0.05f;
            shadow.transform.localScale = Vector3.one * shadowScale * scaleVariation;
        }
    }
    
    void OnDestroy()
    {
        if (shadow != null)
        {
            Destroy(shadow);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Dibujar rango de detección en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Dibujar radio de evitación
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
}
