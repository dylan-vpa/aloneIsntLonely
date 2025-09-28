using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public string targetTag = "Player";
    
    [Header("Follow Settings")]
    public float followSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);
    public bool centerOnTarget = true;
    
    [Header("Camera Bounds")]
    public bool useBounds = true; // Activado para limitar la cámara
    public float sceneWidth = 48f;
    public float sceneHeight = 32f;
    public float margin = 5f;
    public bool autoAdjustBounds = true; // Ajustar límites automáticamente
    
    // Límites calculados
    private float minX, maxX, minY, maxY;
    
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        
        // Buscar el target automáticamente si no está asignado
        FindTarget();
        
        // Configurar la cámara para pixel art
        if (cam != null)
        {
            cam.orthographic = true;
            cam.orthographicSize = 5f;
        }
        
        // Calcular límites de la escena
        CalculateSceneBounds();
    }
    
    void FindTarget()
    {
        if (target == null)
        {
            GameObject targetObj = GameObject.FindGameObjectWithTag(targetTag);
            if (targetObj != null)
            {
                target = targetObj.transform;
                Debug.Log("CameraFollow: Target encontrado - " + target.name);
            }
            else
            {
                Debug.LogWarning("CameraFollow: No se encontró el target con tag '" + targetTag + "'");
            }
        }
    }
    
    void LateUpdate()
    {
        // Buscar el target si no está asignado
        if (target == null)
        {
            FindTarget();
            if (target == null) return; // Si no encuentra el target, no hacer nada
        }
        
        // Verificar que el target sigue siendo válido
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            FindTarget();
            if (target == null) return;
        }
        
        // Centrar exactamente en el personaje
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        
        // Aplicar límites de la cámara
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        }
        
        // Seguimiento suave - el personaje siempre estará centrado
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);
    }
    
    // Método para cambiar el target
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    // Método para configurar los límites
    public void SetBounds(float minX, float maxX, float minY, float maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }
    
    // Método para calcular límites de la escena
    void CalculateSceneBounds()
    {
        // Calcular límites basados en el tamaño de la cámara
        float cameraSize = cam.orthographicSize;
        float cameraWidth = cameraSize * 2f * cam.aspect;
        float cameraHeight = cameraSize * 2f;
        
        // Límites para que la cámara nunca muestre vacío
        minX = -sceneWidth/2 + cameraWidth/2;
        maxX = sceneWidth/2 - cameraWidth/2;
        minY = -sceneHeight/2 + cameraHeight/2;
        maxY = sceneHeight/2 - cameraHeight/2;
        
        Debug.Log($"Límites de cámara calculados: X({minX}, {maxX}), Y({minY}, {maxY})");
        Debug.Log($"Tamaño de cámara: {cameraWidth}x{cameraHeight}");
    }
}
