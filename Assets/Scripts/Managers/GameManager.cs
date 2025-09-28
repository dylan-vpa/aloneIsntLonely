using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxLives = 10;
    public int currentLives;
    
    [Header("Invincibility System")]
    public float invincibilityTime = 5f; // Tiempo de invencibilidad después de ser golpeado
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    
    [Header("Level Detection")]
    private bool isLevel2 = false; // Detecta automáticamente si es Level2
    
    [Header("UI")]
    public Text livesText;
    public Text gameOverText;
    
    [Header("Game State")]
    public bool gameOver = false;
    
    private static GameManager instance;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();
            }
            return instance;
        }
    }
    
    void Start()
    {
        currentLives = maxLives;
        UpdateUI();
        
        // Detectar automáticamente si es Level2
        DetectLevel();
        
        Debug.Log($"GameManager iniciado con {currentLives} vidas - Nivel: {(isLevel2 ? "Level2 (Boss)" : "Level1 (Enemigos)")}");
    }
    
    void DetectLevel()
    {
        // Buscar si hay un BossSpawner en la escena
        BossSpawner bossSpawner = FindFirstObjectByType<BossSpawner>();
        if (bossSpawner != null)
        {
            isLevel2 = true;
            Debug.Log("🎯 Level2 detectado - Modo Boss (Sin invencibilidad)");
        }
        else
        {
            isLevel2 = false;
            Debug.Log("🎯 Level1 detectado - Modo Normal (Con invencibilidad)");
        }
    }
    
    void Update()
    {
        // Manejar invencibilidad
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                Debug.Log("🛡️ Invencibilidad terminada - Puedes recibir daño nuevamente");
            }
        }
        
        // Reiniciar con R usando Input System
        if (Keyboard.current.rKey.wasPressedThisFrame && gameOver)
        {
            RestartGame();
        }
    }
    
    public void PlayerHit()
    {
        if (gameOver) return;
        
        // Verificar si el jugador es invencible (solo en Level1)
        if (!isLevel2 && isInvincible)
        {
            Debug.Log("🛡️ Jugador es invencible - No se puede hacer daño");
            return;
        }
        
        currentLives--;
        Debug.Log($"¡Jugador golpeado! Vidas restantes: {currentLives}");
        
        // Activar invencibilidad solo en Level1
        if (!isLevel2)
        {
            isInvincible = true;
            invincibilityTimer = invincibilityTime;
            Debug.Log($"🛡️ Invencibilidad activada por {invincibilityTime} segundos");
        }
        else
        {
            Debug.Log("⚔️ Level2 - Modo boss - Sin invencibilidad");
        }
        
        UpdateUI();
        
        if (currentLives <= 0)
        {
            GameOver();
        }
    }
    
    void UpdateUI()
    {
        if (livesText != null)
        {
            livesText.text = $"Vidas: {currentLives}";
        }
    }
    
    void GameOver()
    {
        gameOver = true;
        Debug.Log("GAME OVER!");
        
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER!";
            gameOverText.gameObject.SetActive(true);
        }
        
        // Pausar el juego
        Time.timeScale = 0f;
        
        // Cargar Level1 después de un delay
        Invoke(nameof(RestartToLevel1), 3f);
    }
    
    void RestartToLevel1()
    {
        // Reiniciar el juego y cargar Level1
        Time.timeScale = 1f;
        currentLives = maxLives;
        gameOver = false;
        isInvincible = false;
        invincibilityTimer = 0f;
        
        // Cargar Level1
        SceneTransitionManager transitionManager = SceneTransitionManager.Instance;
        if (transitionManager != null)
        {
            transitionManager.LoadLevel1();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        currentLives = maxLives;
        gameOver = false;
        isInvincible = false; // Reset invencibilidad
        invincibilityTimer = 0f;
        UpdateUI();
        
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
        
        Debug.Log("Juego reiniciado!");
    }
    
    // Método público para verificar si el jugador es invencible
    public bool IsPlayerInvincible()
    {
        return isInvincible;
    }
    
    // Método público para obtener tiempo restante de invencibilidad
    public float GetInvincibilityTimeLeft()
    {
        return invincibilityTimer;
    }
    
    // Método para cuando se completa un nivel
    public void OnLevelComplete()
    {
        Debug.Log("¡Nivel completado!");
        
        // Determinar qué nivel se completó
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        int levelNumber = 0;
        
        if (currentScene == "Level1")
        {
            levelNumber = 1;
        }
        else if (currentScene == "Level2")
        {
            levelNumber = 2;
        }
        
        // Cargar siguiente nivel o menú final
        if (levelNumber == 1)
        {
            // Level1 completado - Ir a Level2
            Debug.Log("Level1 completado! Cargando Level2...");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
        }
        else if (levelNumber == 2)
        {
            // Level2 completado - Ir a EndMenu
            Debug.Log("Level2 completado! Cargando EndMenu...");
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndMenu");
        }
    }
}
