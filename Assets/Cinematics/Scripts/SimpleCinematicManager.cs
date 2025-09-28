using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleCinematicManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image cinematicImage;
    public Text cinematicText;
    public GameObject skipButton;
    public Image fadeImage;
    
    [Header("Settings")]
    public float displayTime = 3f; // Tiempo que se muestra cada imagen
    public float fadeSpeed = 2f;
    public float skipDelay = 1f;
    
    [Header("Cinematic Content")]
    public string[] cinematicTexts = {
        "¡Bienvenido a la aventura!",
        "Preparándote para la batalla final...",
        "¡Gracias por jugar!"
    };
    
    private int currentCinematic = 0;
    private bool canSkip = false;
    
    void Start()
    {
        // Mostrar botón skip después del delay
        Invoke(nameof(EnableSkip), skipDelay);
        
        // Determinar qué cinemática mostrar basada en la escena
        string sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName == "Cinematic1")
        {
            currentCinematic = 0;
            ShowCinematic();
        }
        else if (sceneName == "Cinematic2")
        {
            currentCinematic = 1;
            ShowCinematic();
        }
        else if (sceneName == "Cinematic3")
        {
            currentCinematic = 2;
            ShowCinematic();
        }
    }
    
    void ShowCinematic()
    {
        if (cinematicText != null)
        {
            cinematicText.text = cinematicTexts[currentCinematic];
        }
        
        // Fade in
        StartCoroutine(FadeIn());
        
        // Auto avanzar después del tiempo
        Invoke(nameof(AutoAdvance), displayTime);
    }
    
    void AutoAdvance()
    {
        AdvanceToNext();
    }
    
    public void SkipCinematic()
    {
        if (!canSkip) return;
        
        AdvanceToNext();
    }
    
    void AdvanceToNext()
    {
        StartCoroutine(AdvanceSequence());
    }
    
    System.Collections.IEnumerator AdvanceSequence()
    {
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Cargar siguiente escena
        switch (currentCinematic)
        {
            case 0: // Intro - Ir a Level1
                SceneManager.LoadScene("Level1");
                break;
            case 1: // Intermedio - Ir a Level2
                SceneManager.LoadScene("Level2");
                break;
            case 2: // Final - Ir a menú final
                SceneManager.LoadScene("EndMenu");
                break;
        }
    }
    
    void EnableSkip()
    {
        canSkip = true;
        if (skipButton != null)
        {
            skipButton.SetActive(true);
        }
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
}
