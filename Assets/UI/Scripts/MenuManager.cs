using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Screens")]
    public GameObject mainMenuScreen;
    public GameObject endMenuScreen;
    public GameObject loadingScreen;
    
    [Header("UI Elements")]
    public Text loadingText;
    public Image fadeImage;
    
    [Header("Settings")]
    public float fadeSpeed = 2f;
    public float loadingDelay = 1f;
    
    private bool isTransitioning = false;
    
    void Start()
    {
        // Mostrar menú principal al inicio
        ShowMainMenu();
    }
    
    public void ShowMainMenu()
    {
        if (mainMenuScreen != null)
        {
            mainMenuScreen.SetActive(true);
        }
        if (endMenuScreen != null)
        {
            endMenuScreen.SetActive(false);
        }
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
    
    public void ShowEndMenu()
    {
        if (mainMenuScreen != null)
        {
            mainMenuScreen.SetActive(false);
        }
        if (endMenuScreen != null)
        {
            endMenuScreen.SetActive(true);
        }
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
    
    public void StartGame()
    {
        if (isTransitioning) return;
        
        Debug.Log("Iniciando juego...");
        SceneManager.LoadScene("Level1");
    }
    
    public void RestartGame()
    {
        if (isTransitioning) return;
        
        Debug.Log("Reiniciando juego...");
        SceneManager.LoadScene("Level1");
    }
    
    public void QuitGame()
    {
        if (isTransitioning) return;
        
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
    
    System.Collections.IEnumerator LoadSceneWithFade(string sceneName)
    {
        isTransitioning = true;
        
        // Mostrar pantalla de carga
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
        
        // Fade out (solo si hay fadeImage)
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }
        
        // Cargar escena
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            if (loadingText != null)
            {
                loadingText.text = $"Cargando... {Mathf.RoundToInt(asyncLoad.progress * 100)}%";
            }
            yield return null;
        }
        
        // Fade in (solo si hay fadeImage)
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeIn());
        }
        
        isTransitioning = false;
    }
    
    System.Collections.IEnumerator FadeOut()
    {
        if (fadeImage == null) yield break;
        
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        
        while (color.a < 1f)
        {
            color.a += fadeSpeed * Time.deltaTime;
            fadeImage.color = color;
            yield return null;
        }
    }
    
    System.Collections.IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;
        
        Color color = fadeImage.color;
        
        while (color.a > 0f)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            fadeImage.color = color;
            yield return null;
        }
        
        fadeImage.gameObject.SetActive(false);
    }
    
    // Método para ser llamado desde otras escenas
    public void LoadEndMenu()
    {
        ShowEndMenu();
    }
}
