using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.1f; // Tiempo entre disparos (muy rápido)
    private float nextFireTime = 0f;

    void Start()
    {
        Debug.Log("PlayerShoot iniciado!");
        
        // Si no asignaste firePoint en el inspector, crear uno automático
        if (firePoint == null)
        {
            GameObject fp = new GameObject("FirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = Vector3.down * 0.5f; // Un poco abajo del personaje
            firePoint = fp.transform;

            Debug.Log("⚡ FirePoint creado automáticamente en " + firePoint.position);
        }
        
        Debug.Log($"FirePoint asignado: {firePoint != null}");
        Debug.Log($"Prefab asignado: {projectilePrefab != null}");
    }

    void Update()
    {

        // Disparar con CLICK IZQUIERDO (en la posición del mouse)
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextFireTime)
        {
            Debug.Log("CLICK IZQUIERDO DETECTADO!");
            
            // Obtener posición del mouse en el mundo
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            mouseScreenPos.z = Camera.main.nearClipPlane;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0; // Mantener en el plano 2D
            
            Debug.Log($"Mouse screen: {mouseScreenPos}, Mouse world: {mouseWorldPos}");

            ShootAtPosition(mouseWorldPos);
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot(Vector2 direction)
    {
        Debug.Log("🎯 Iniciando disparo...");
        
        if (projectilePrefab == null)
        {
            Debug.LogError("❌ No has asignado el prefab del proyectil en PlayerShoot");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("❌ FirePoint es null!");
            return;
        }

        Debug.Log($"🔫 Creando proyectil en posición: {firePoint.position}");
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
            Debug.Log("✅ Proyectil configurado con script Projectile");
        }
        else
        {
            Debug.LogError("❌ El proyectil no tiene script Projectile!");
        }

        Debug.Log("🔫 Disparo completado en dirección: " + direction);
    }
    
    void ShootAtPosition(Vector3 position)
    {
        Debug.Log("🎯 Disparando en posición: " + position);
        
        if (projectilePrefab == null)
        {
            Debug.LogError("❌ No has asignado el prefab del proyectil en PlayerShoot");
            return;
        }

        // Crear el proyectil directamente en la posición del mouse
        GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        Debug.Log($"🔫 Proyectil creado en posición: {position}");
        
        // Destruir el proyectil después de 0.5 segundos
        Destroy(projectile, 0.5f);
        Debug.Log("✅ Proyectil creado exitosamente! Se destruirá en 0.5 segundos");
    }
}
