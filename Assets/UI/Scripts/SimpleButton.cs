using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Button Settings")]
    public string buttonType = "start"; // start, quit, restart, skip
    public float hoverScale = 1.1f;
    public float clickScale = 0.95f;
    public float animationSpeed = 5f;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    
    [Header("Visual Effects")]
    public Image buttonImage;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color clickColor = Color.gray;
    
    private Vector3 originalScale;
    private Color originalColor;
    private bool isHovering = false;
    
    void Start()
    {
        originalScale = transform.localScale;
        
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
        
        // Configurar audio source si no está asignado
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    
    void Update()
    {
        // Animación de hover
        if (isHovering)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * hoverScale, animationSpeed * Time.deltaTime);
            if (buttonImage != null)
            {
                buttonImage.color = Color.Lerp(buttonImage.color, hoverColor, animationSpeed * Time.deltaTime);
            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, animationSpeed * Time.deltaTime);
            if (buttonImage != null)
            {
                buttonImage.color = Color.Lerp(buttonImage.color, normalColor, animationSpeed * Time.deltaTime);
            }
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // Efecto visual de click
        StartCoroutine(ClickEffect());
        
        // Sonido de click
        PlaySound(clickSound);
        
        // Ejecutar acción según el tipo de botón
        ExecuteButtonAction();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        PlaySound(hoverSound);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
    
    void ExecuteButtonAction()
    {
        switch (buttonType.ToLower())
        {
            case "start":
                StartGame();
                break;
            case "quit":
                QuitGame();
                break;
            case "restart":
                RestartGame();
                break;
            case "skip":
                SkipVideo();
                break;
            default:
                Debug.LogWarning($"Tipo de botón no reconocido: {buttonType}");
                break;
        }
    }
    
    void StartGame()
    {
        Debug.Log("Iniciando juego...");
        
        // Buscar MenuManager
        MenuManager menuManager = FindFirstObjectByType<MenuManager>();
        if (menuManager != null)
        {
            menuManager.StartGame();
        }
        else
        {
            // Si no hay MenuManager, cargar Cinematic1 directamente
            SceneTransitionManager transitionManager = SceneTransitionManager.Instance;
            if (transitionManager != null)
            {
                transitionManager.LoadIntroCinematic();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Cinematic1");
            }
        }
    }
    
    void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
    
    void RestartGame()
    {
        Debug.Log("Reiniciando juego...");
        
        MenuManager menuManager = FindFirstObjectByType<MenuManager>();
        if (menuManager != null)
        {
            menuManager.RestartGame();
        }
        else
        {
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
    }
    
    void SkipVideo()
    {
        Debug.Log("Saltando video...");
        
        CinematicManager cinematicManager = FindFirstObjectByType<CinematicManager>();
        if (cinematicManager != null)
        {
            cinematicManager.SkipVideo();
        }
    }
    
    System.Collections.IEnumerator ClickEffect()
    {
        // Efecto visual de click
        Vector3 clickScale = originalScale * this.clickScale;
        transform.localScale = clickScale;
        
        if (buttonImage != null)
        {
            buttonImage.color = clickColor;
        }
        
        yield return new WaitForSeconds(0.1f);
        
        // Volver a escala normal
        transform.localScale = originalScale;
        
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
    }
    
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
