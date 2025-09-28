using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Transition Settings")]
    public Image fadeImage;
    public float fadeSpeed = 2f;
    public float transitionDelay = 1f;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip transitionSound;
    
    private static SceneTransitionManager instance;
    public static SceneTransitionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<SceneTransitionManager>();
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }
    
    public void LoadLevel1()
    {
        LoadScene("Level1");
    }
    
    public void LoadLevel2()
    {
        LoadScene("Level2");
    }
    
    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }
    
    public void LoadEndMenu()
    {
        LoadScene("EndMenu");
    }
    
    public void LoadCinematic(int videoIndex)
    {
        string sceneName = "Cinematic" + (videoIndex + 1);
        LoadScene(sceneName);
    }
    
    public void LoadIntroCinematic()
    {
        LoadScene("Cinematic1");
    }
    
    public void LoadIntermedioCinematic()
    {
        LoadScene("Cinematic2");
    }
    
    public void LoadFinalCinematic()
    {
        LoadScene("Cinematic3");
    }
    
    IEnumerator TransitionToScene(string sceneName)
    {
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Reproducir sonido de transición
        if (audioSource != null && transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }
        
        // Cargar escena
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // Fade in
        yield return StartCoroutine(FadeIn());
    }
    
    IEnumerator FadeOut()
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
    
    IEnumerator FadeIn()
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
    
    // Método para ser llamado desde GameManager cuando se gana
    public void OnLevelComplete(int levelNumber)
    {
        if (levelNumber == 1)
        {
            // Level1 completado - Ir a video intermedio
            LoadCinematic(1);
        }
        else if (levelNumber == 2)
        {
            // Level2 completado - Ir a video final
            LoadCinematic(2);
        }
    }
}
