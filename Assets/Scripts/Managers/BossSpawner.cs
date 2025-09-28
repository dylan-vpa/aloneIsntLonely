using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public float spawnDelay = 3f; // Delay antes de spawnear el boss
    
    [Header("Scene Bounds")]
    public float sceneWidth = 48f;
    public float sceneHeight = 32f;
    public float margin = 5f;
    
    // Límites calculados
    private float minX, maxX, minY, maxY;
    
    private Transform player;
    private bool bossSpawned = false;
    
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
        
        // Spawnear el boss después del delay
        Invoke(nameof(SpawnBoss), spawnDelay);
    }
    
    void CalculateBounds()
    {
        minX = -sceneWidth/2 + margin;
        maxX = sceneWidth/2 - margin;
        minY = -sceneHeight/2 + margin;
        maxY = sceneHeight/2 - margin;
        
        Debug.Log($"Límites de boss spawn calculados: X({minX}, {maxX}), Y({minY}, {maxY})");
    }
    
    void SpawnBoss()
    {
        if (bossSpawned) return;
        
        if (bossPrefab == null)
        {
            Debug.LogError("❌ No hay prefab del boss asignado!");
            return;
        }
        
        Vector3 spawnPosition = GetBossSpawnPosition();
        
        // Crear el boss
        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        boss.name = "Boss";
        
        // Agregar script para notificar cuando muere
        Boss bossScript = boss.GetComponent<Boss>();
        if (bossScript != null)
        {
            // Suscribirse al evento de muerte
            bossScript.OnBossDeath += OnBossDeath;
        }
        
        bossSpawned = true;
        Debug.Log($"Boss spawneado en {spawnPosition}");
    }
    
    Vector3 GetBossSpawnPosition()
    {
        // Spawnear el boss lejos del jugador
        Vector3 spawnPos;
        int attempts = 0;
        int maxAttempts = 50;
        
        do
        {
            // Generar posición aleatoria dentro de los límites
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            spawnPos = new Vector3(randomX, randomY, 0);
            
            // Verificar que esté lejos del jugador
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(spawnPos, player.position);
                if (distanceToPlayer >= 8f) // Mínimo 8 unidades del jugador
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
        
        Debug.Log($"Posición de boss generada: {spawnPos}");
        return spawnPos;
    }
    
    // Método llamado cuando el boss muere
    void OnBossDeath()
    {
        Debug.Log("🎉 ¡BOSS DERROTADO! ¡NIVEL COMPLETADO!");
        
        // Aquí puedes agregar lógica de victoria del nivel
        // Por ejemplo, cargar el siguiente nivel o mostrar pantalla de victoria
    }
    
    void OnDrawGizmosSelected()
    {
        // Dibujar área de spawn en el editor
        Gizmos.color = Color.red;
        Vector3 center = Vector3.zero;
        Vector3 size = new Vector3(sceneWidth - margin*2, sceneHeight - margin*2, 0);
        Gizmos.DrawWireCube(center, size);
        
        // Dibujar distancia mínima del jugador
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, 8f);
        }
    }
}
