using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public RawImage videoDisplay;
    public AudioSource audioSource;
    
    [Header("Video Clips")]
    public VideoClip[] cinematicVideos;
    public string[] videoNames = {"Intro", "Intermedio", "Final"};
    
    [Header("UI Elements")]
    public GameObject skipButton;
    public Text videoTitleText;
    public Image fadeImage;
    
    [Header("Settings")]
    public float fadeSpeed = 2f;
    public float skipDelay = 2f; // Tiempo antes de mostrar botón skip
    
    private int currentVideoIndex = 0;
    private bool isPlaying = false;
    private bool canSkip = false;
    
    void Start()
    {
        SetupVideoPlayer();
        StartCoroutine(ShowSkipButtonAfterDelay());
    }
    
    void SetupVideoPlayer()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
        
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.loopPointReached += OnVideoFinished;
        }
        
        if (skipButton != null)
        {
            skipButton.SetActive(false);
        }
    }
    
    public void PlayVideo(int videoIndex)
    {
        if (videoIndex < 0 || videoIndex >= cinematicVideos.Length)
        {
            Debug.LogError($"Índice de video inválido: {videoIndex}");
            return;
        }
        
        currentVideoIndex = videoIndex;
        StartCoroutine(PlayVideoSequence());
    }
    
    System.Collections.IEnumerator PlayVideoSequence()
    {
        isPlaying = true;
        canSkip = false;
        
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Configurar video
        if (videoPlayer != null && cinematicVideos[currentVideoIndex] != null)
        {
            videoPlayer.clip = cinematicVideos[currentVideoIndex];
            videoPlayer.Prepare();
            
            // Mostrar título del video
            if (videoTitleText != null)
            {
                videoTitleText.text = videoNames[currentVideoIndex];
            }
        }
        
        // Fade in
        yield return StartCoroutine(FadeIn());
        
        // Reproducir video
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }
    
    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log($"Video preparado: {videoNames[currentVideoIndex]}");
    }
    
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log($"Video terminado: {videoNames[currentVideoIndex]}");
        StartCoroutine(OnVideoComplete());
    }
    
    System.Collections.IEnumerator OnVideoComplete()
    {
        isPlaying = false;
        
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Determinar siguiente acción basada en el video
        switch (currentVideoIndex)
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
    
    public void SkipVideo()
    {
        if (!canSkip || !isPlaying) return;
        
        Debug.Log("Saltando video...");
        StartCoroutine(OnVideoComplete());
    }
    
    System.Collections.IEnumerator ShowSkipButtonAfterDelay()
    {
        yield return new WaitForSeconds(skipDelay);
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
    
    // Métodos públicos para ser llamados desde otros scripts
    public void PlayIntroVideo()
    {
        PlayVideo(0);
    }
    
    public void PlayIntermedioVideo()
    {
        PlayVideo(1);
    }
    
    public void PlayFinalVideo()
    {
        PlayVideo(2);
    }
}
