# Documentación Técnica

## Arquitectura del Sistema

### Patrón de Diseño
El proyecto utiliza el patrón Singleton para la gestión de estado global y el patrón Manager para la coordinación de sistemas.

### Componentes Principales

#### GameManager
- **Propósito**: Control central del estado del juego
- **Responsabilidades**:
  - Gestión de vidas del jugador
  - Sistema de invencibilidad
  - Detección de finalización de niveles
  - Control de estado de Game Over

#### SceneTransitionManager
- **Propósito**: Manejo de transiciones entre escenas
- **Responsabilidades**:
  - Carga asíncrona de escenas
  - Efectos de transición (fade in/out)
  - Gestión de audio durante transiciones

#### AutoCinematicManager
- **Propósito**: Reproducción automática de videos
- **Responsabilidades**:
  - Detección automática de escena de cinemática
  - Configuración de VideoPlayer
  - Transiciones post-video

## Sistema de Input

### Input System Integration
- Uso del nuevo Input System de Unity
- Mapeo de controles configurable
- Soporte para múltiples dispositivos

### Controles Implementados
- **Movimiento**: WASD / Flechas direccionales
- **Disparo**: Espacio / Click izquierdo
- **Skip**: Tecla S (en cinemáticas)
- **Reinicio**: Tecla R (en Game Over)

## Sistema de Colisiones

### Detección de Colisiones
- Uso de Collider2D para detección
- Sistema de tags para identificación de objetos
- Física 2D para interacciones realistas

### Tipos de Colisiones
- **Jugador-Enemigo**: Daño al jugador
- **Proyectil-Enemigo**: Destrucción de enemigo
- **Jugador-Obstáculo**: Bloqueo de movimiento

## Sistema de Audio

### Gestión de Audio
- AudioSource para música de fondo
- Efectos de sonido para acciones
- Audio espacial para inmersión

### Implementación
- Música de fondo continua
- Efectos de disparo y colisión
- Audio de transición entre escenas

## Optimización

### Rendimiento
- Uso de Object Pooling para proyectiles
- Destrucción automática de objetos fuera de pantalla
- Optimización de sprites y texturas

### Memoria
- Carga asíncrona de escenas
- Liberación de recursos no utilizados
- Gestión eficiente de audio

## Configuración de Build

### Configuración de Escenas
Orden requerido en Build Settings:
1. MainMenu (índice 0)
2. cinematic1 (índice 1)
3. Level1 (índice 2)
4. cinematic2 (índice 3)
5. Level2 (índice 4)
6. cinematic3 (índice 5)
7. EndMenu (índice 6)

### Configuración de Plataforma
- Resolución objetivo: 1920x1080
- Escalado: Scale With Screen Size
- Modo de pantalla: Full Screen Windowed

## Debugging y Testing

### Sistema de Logs
- Logs informativos para debugging
- Categorización de mensajes por sistema
- Información de estado en tiempo real

### Herramientas de Debug
- Visualización de límites de pantalla
- Indicadores de estado de invencibilidad
- Información de colisiones en consola

## Extensibilidad

### Nuevos Niveles
- Sistema modular para agregar niveles
- Configuración de enemigos por nivel
- Sistema de progresión escalable

### Nuevas Mecánicas
- Arquitectura preparada para nuevas funcionalidades
- Sistema de eventos para comunicación entre componentes
- Interfaces para extensión de comportamientos
