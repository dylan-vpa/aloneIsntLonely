using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Boss Stats")]
    public int maxHealth = 25;
    public int currentHealth;
    
    [Header("Movement")]
    public float moveSpeed = 2f; // Más lento que enemigos normales
    public float followSmoothness = 3f; // Movimiento más suave
    public float detectionRange = 50f; // Rango infinito
    
    [Header("Boss Size")]
    public float bossScale = 2.5f; // Mucho más grande
    
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
    
    // Evento para notificar cuando el boss muere
    public System.Action OnBossDeath;
    
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Hacer el boss mucho más grande
        transform.localScale = Vector3.one * bossScale;
        
        // Buscar al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        Debug.Log($"Boss creado con {currentHealth} vidas y escala {bossScale}x");
        
        // Crear sombra para el boss
        CreateShadow();
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
        
        // SIEMPRE perseguir al jugador
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        
        // Movimiento más lento pero constante hacia el jugador
        Vector2 targetVelocity = directionToPlayer * moveSpeed;
        
        // Aceleración suave
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, followSmoothness * Time.deltaTime);
        
        // Rotar hacia el jugador
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 3f * Time.deltaTime);
        }
        
        // Debug ocasional
        if (Random.Range(0f, 1f) < 0.005f) // 0.5% de probabilidad
        {
            Debug.Log($"Boss persiguiendo jugador - Distancia: {distanceToPlayer:F1}, Salud: {currentHealth}/{maxHealth}");
        }
    }
    
    void ClampToSceneBounds()
    {
        // Calcular límites con margen
        float minX = -sceneWidth/2 + margin;
        float maxX = sceneWidth/2 - margin;
        float minY = -sceneHeight/2 + margin;
        float maxY = sceneHeight/2 - margin;
        
        // Limitar posición del boss
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
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Boss recibió {damage} daño. Vidas restantes: {currentHealth}/{maxHealth}");
        
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
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }
    
    void Die()
    {
        Debug.Log("🎉 ¡BOSS DERROTADO! ¡VICTORIA!");
        
        // Notificar al GameManager que el nivel está completo
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.OnLevelComplete();
        }
        
        // Notificar que el boss murió
        OnBossDeath?.Invoke();
        
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // El boss tocó al jugador - SIN invencibilidad
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
        shadow = new GameObject("BossShadow");
        shadow.transform.SetParent(transform);
        shadowRenderer = shadow.AddComponent<SpriteRenderer>();
        
        // Configurar sombra
        shadowRenderer.sprite = spriteRenderer.sprite;
        shadowRenderer.color = new Color(0, 0, 0, 0.4f); // Más oscura para el boss
        shadowRenderer.sortingOrder = -1; // Detrás del boss
        shadow.transform.localScale = Vector3.one * 1.2f; // Sombra más grande
        
        // Posición inicial de la sombra
        shadow.transform.localPosition = new Vector3(0.3f, -0.3f, 0);
        
        // Rotación inicial aleatoria
        shadowRotation = Random.Range(0f, 360f);
    }
    
    void UpdateShadow()
    {
        if (shadow != null)
        {
            // Actualizar posición de la sombra con movimiento más lento
            Vector3 shadowPos = new Vector3(
                0.3f + Mathf.Sin(Time.time * 1f) * 0.15f,
                -0.3f + Mathf.Cos(Time.time * 0.8f) * 0.15f,
                0
            );
            shadow.transform.localPosition = shadowPos;
            
            // Rotar sombra ligeramente
            shadowRotation += Time.deltaTime * 15f;
            shadow.transform.rotation = Quaternion.Euler(0, 0, shadowRotation);
            
            // Cambiar escala ligeramente
            float scaleVariation = 1f + Mathf.Sin(Time.time * 2f) * 0.03f;
            shadow.transform.localScale = Vector3.one * 1.2f * scaleVariation;
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
    }
}
