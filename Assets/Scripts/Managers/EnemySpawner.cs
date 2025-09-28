using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public int totalEnemies = 5;
    
    [Header("Scene Bounds")]
    public float sceneWidth = 48f;
    public float sceneHeight = 32f;
    public float margin = 5f;
    
    // Límites calculados
    private float minX, maxX, minY, maxY;
    
    [Header("Spawn Area")]
    public float minSpawnDistance = 3f; // Distancia mínima del jugador
    
    [Header("Gradual Spawn")]
    public int enemiesPerWave = 2; // Enemigos por oleada (2+2+1 = 5 total)
    public float waveDelay = 3f; // Delay entre oleadas
    public float initialDelay = 5f; // Delay inicial antes de empezar
    
    [Header("Respawn System")]
    public bool respawnOnDeath = false; // NO respawn - cuando mueren 5, ganas
    public float respawnDelay = 2f; // Delay para respawn
    
    [Header("Shadow Settings")]
    public int maxShadows = 5; // Todos los enemigos tienen sombras
    
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;
    private int currentWave = 0;
    private Transform player;
    
    void Start()
    {
        // Calcular límites de spawn
        CalculateBounds();
        
        // Buscar al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        // Iniciar spawn gradual después del delay inicial
        Invoke(nameof(StartGradualSpawn), initialDelay);
    }
    
    void CalculateBounds()
    {
        minX = -sceneWidth/2 + margin;
        maxX = sceneWidth/2 - margin;
        minY = -sceneHeight/2 + margin;
        maxY = sceneHeight/2 - margin;
        
        Debug.Log($"Límites de spawn calculados: X({minX}, {maxX}), Y({minY}, {maxY})");
    }
    
    void StartGradualSpawn()
    {
        Debug.Log("🌊 Iniciando spawn gradual de enemigos...");
        SpawnWave();
    }
    
    void SpawnWave()
    {
        if (enemiesSpawned >= totalEnemies)
        {
            Debug.Log("✅ Todos los enemigos han sido spawneados!");
            return;
        }
        
        currentWave++;
        int enemiesInThisWave = Mathf.Min(enemiesPerWave, totalEnemies - enemiesSpawned);
        
        Debug.Log($"🌊 Oleada {currentWave}: Spawneando {enemiesInThisWave} enemigos...");
        
        for (int i = 0; i < enemiesInThisWave; i++)
        {
            SpawnEnemy();
        }
        
        Debug.Log($"✅ Oleada {currentWave} completada! Total: {enemiesSpawned}/{totalEnemies}, Vivos: {enemiesAlive}");
        
        // Programar siguiente oleada si quedan enemigos
        if (enemiesSpawned < totalEnemies)
        {
            Invoke(nameof(SpawnWave), waveDelay);
        }
    }
    
    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("❌ No hay prefab de enemigo asignado!");
            return;
        }
        
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        // Crear el enemigo
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.name = $"Enemy_{enemiesSpawned + 1}";
        
        // Agregar script para notificar cuando muere
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            // Suscribirse al evento de muerte
            enemyScript.OnEnemyDeath += OnEnemyDeath;
            
            // Solo los primeros 5 enemigos tienen sombras
            if (enemiesSpawned < maxShadows)
            {
                enemyScript.useShadow = true;
                Debug.Log($"Enemy {enemiesSpawned + 1} tendrá sombra");
            }
            else
            {
                enemyScript.useShadow = false;
                Debug.Log($"Enemy {enemiesSpawned + 1} sin sombra");
            }
        }
        
        enemiesSpawned++;
        enemiesAlive++;
        Debug.Log($"Enemy {enemiesSpawned} spawneado en {spawnPosition}. Vivos: {enemiesAlive}");
    }
    
    // Método llamado cuando un enemigo muere
    void OnEnemyDeath()
    {
        enemiesAlive--;
        Debug.Log($"Enemy muerto! Vivos: {enemiesAlive}");
        
        // Verificar si ganaste (todos los enemigos muertos)
        if (enemiesAlive <= 0)
        {
            Debug.Log("🎉 ¡GANASTE! Todos los enemigos fueron derrotados!");
            
            // Notificar al GameManager que el nivel está completo
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.OnLevelComplete();
            }
        }
        
        // Respawn si está habilitado y no hemos alcanzado el máximo
        if (respawnOnDeath && enemiesSpawned < totalEnemies)
        {
            Invoke(nameof(RespawnEnemy), respawnDelay);
        }
    }
    
    void RespawnEnemy()
    {
        if (enemiesSpawned < totalEnemies)
        {
            SpawnEnemy();
            Debug.Log("🔄 Enemy respawneado!");
        }
    }
    
    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPos;
        int attempts = 0;
        int maxAttempts = 50;
        
        do
        {
            // Generar posición aleatoria dentro de los límites calculados
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            spawnPos = new Vector3(randomX, randomY, 0);
            
            // Verificar que esté lejos del jugador
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(spawnPos, player.position);
                if (distanceToPlayer >= minSpawnDistance)
                {
                    break;
                }
            }
            else
            {
                break;
            }
            
            attempts++;
        } while (attempts < maxAttempts);
        
        Debug.Log($"Posición de spawn generada: {spawnPos} (límites: X({minX}, {maxX}), Y({minY}, {maxY}))");
        return spawnPos;
    }
    
    void OnDrawGizmosSelected()
    {
        // Dibujar área de spawn en el editor
        Gizmos.color = Color.yellow;
        Vector3 center = Vector3.zero;
        Vector3 size = new Vector3(sceneWidth - margin*2, sceneHeight - margin*2, 0);
        Gizmos.DrawWireCube(center, size);
        
        // Dibujar distancia mínima del jugador
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, minSpawnDistance);
        }
    }
}
