using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    
    [Header("Input Settings")]
    public bool useWASD = true;
    public bool useArrowKeys = true;
    
    [Header("Scene Bounds")]
    public float sceneWidth = 48f;
    public float sceneHeight = 32f;
    public float margin = 5f;
    
    private Vector2 inputVector;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    // Límites calculados
    private float minX, maxX, minY, maxY;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        // Configurar el Rigidbody2D para movimiento 2D
        if (rb != null)
        {
            rb.gravityScale = 0f; // Sin gravedad para movimiento top-down
            rb.linearDamping = 0f; // Sin resistencia del aire
            rb.freezeRotation = true; // Congelar rotación
        }
        
        // Calcular límites de la escena
        CalculateBounds();
    }
    
    void CalculateBounds()
    {
        minX = -sceneWidth/2 + margin;
        maxX = sceneWidth/2 - margin;
        minY = -sceneHeight/2 + margin;
        maxY = sceneHeight/2 - margin;
        
        Debug.Log($"Límites del jugador calculados: X({minX}, {maxX}), Y({minY}, {maxY})");
    }
    
    void Update()
    {
        HandleInput();
        HandleMovement();
        ClampToSceneBounds();
        HandleAnimation();
    }
    
    void HandleInput()
    {
        inputVector = Vector2.zero;
        
        // Input con WASD
        if (useWASD)
        {
            if (Keyboard.current.wKey.isPressed) inputVector.y += 1f;
            if (Keyboard.current.sKey.isPressed) inputVector.y -= 1f;
            if (Keyboard.current.aKey.isPressed) inputVector.x -= 1f;
            if (Keyboard.current.dKey.isPressed) inputVector.x += 1f;
        }
        
        // Input con flechas
        if (useArrowKeys)
        {
            if (Keyboard.current.upArrowKey.isPressed) inputVector.y += 1f;
            if (Keyboard.current.downArrowKey.isPressed) inputVector.y -= 1f;
            if (Keyboard.current.leftArrowKey.isPressed) inputVector.x -= 1f;
            if (Keyboard.current.rightArrowKey.isPressed) inputVector.x += 1f;
        }
        
        // Normalizar el vector de input para movimiento diagonal consistente
        inputVector = inputVector.normalized;
    }
    
    void HandleMovement()
    {
        Vector2 targetVelocity = inputVector * moveSpeed;
        
        // Aplicar aceleración y desaceleración suave
        if (inputVector.magnitude > 0.1f)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
        }
        
        // Aplicar la velocidad al Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = currentVelocity;
        }
        else
        {
            // Fallback si no hay Rigidbody2D
            transform.Translate(currentVelocity * Time.deltaTime);
        }
    }
    
    void HandleAnimation()
    {
        // El personaje mantiene siempre la misma orientación (sin rotación)
        // Solo controlar animaciones si hay un Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", currentVelocity.magnitude);
            animator.SetFloat("Horizontal", inputVector.x);
            animator.SetFloat("Vertical", inputVector.y);
        }
    }
    
    // Método público para obtener la velocidad actual (útil para otros scripts)
    public Vector2 GetCurrentVelocity()
    {
        return currentVelocity;
    }
    
    // Método público para obtener el input actual (útil para otros scripts)
    public Vector2 GetInputVector()
    {
        return inputVector;
    }
    
    // Método para cambiar la velocidad de movimiento en tiempo de ejecución
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
    
    void ClampToSceneBounds()
    {
        // Limitar posición del jugador
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
        
        // Solo bloquear movimiento hacia el borde que se está tocando
        Vector2 limitedVelocity = currentVelocity;
        
        // Si está en el borde izquierdo y tratando de ir más a la izquierda
        if (pos.x <= minX && currentVelocity.x < 0)
        {
            limitedVelocity.x = 0;
        }
        // Si está en el borde derecho y tratando de ir más a la derecha
        else if (pos.x >= maxX && currentVelocity.x > 0)
        {
            limitedVelocity.x = 0;
        }
        
        // Si está en el borde inferior y tratando de ir más abajo
        if (pos.y <= minY && currentVelocity.y < 0)
        {
            limitedVelocity.y = 0;
        }
        // Si está en el borde superior y tratando de ir más arriba
        else if (pos.y >= maxY && currentVelocity.y > 0)
        {
            limitedVelocity.y = 0;
        }
        
        // Aplicar la velocidad limitada
        if (rb != null)
        {
            rb.linearVelocity = limitedVelocity;
        }
    }
}
