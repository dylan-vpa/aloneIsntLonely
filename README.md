# Alone Isn't Lonely

Un juego de acción 2D desarrollado en Unity con sistema de cinemáticas integrado.

## Descripción del Proyecto

Alone Isn't Lonely es un juego de acción 2D que combina mecánicas de disparo con narrativa a través de cinemáticas. El jugador controla un personaje que debe superar dos niveles de dificultad creciente, con videos narrativos que conectan la experiencia.

## Características Principales

- **Sistema de Niveles**: Dos niveles con mecánicas diferenciadas
- **Sistema de Disparo**: Mecánica de combate con proyectiles
- **Cinemáticas Integradas**: Videos narrativos entre niveles
- **Sistema de Vidas**: Mecánica de supervivencia con invencibilidad temporal
- **Interfaz de Usuario**: Menús intuitivos y sistema de transiciones

## Estructura del Proyecto

### Escenas
- `MainMenu`: Menú principal del juego
- `cinematic1`: Cinemática de introducción
- `Level1`: Primer nivel con enemigos básicos
- `cinematic2`: Cinemática intermedia
- `Level2`: Segundo nivel con jefe final
- `cinematic3`: Cinemática de conclusión
- `EndMenu`: Menú de fin de juego

### Scripts Principales

#### Gestión de Escenas
- `SceneTransitionManager.cs`: Maneja las transiciones entre escenas
- `MenuManager.cs`: Controla la lógica de los menús
- `AutoCinematicManager.cs`: Gestiona la reproducción automática de videos

#### Mecánicas de Juego
- `GameManager.cs`: Controla el estado general del juego
- `PlayerMovement.cs`: Maneja el movimiento del jugador
- `PlayerShooting.cs`: Sistema de disparo del jugador
- `Enemy.cs`: Comportamiento de enemigos básicos
- `Boss.cs`: Lógica del jefe final

## Flujo del Juego

1. **Menú Principal** → Inicio del juego
2. **Cinemática 1** → Introducción narrativa
3. **Nivel 1** → Combate contra enemigos básicos
4. **Cinemática 2** → Transición narrativa
5. **Nivel 2** → Enfrentamiento con jefe final
6. **Cinemática 3** → Conclusión narrativa
7. **Menú Final** → Fin del juego

## Requisitos del Sistema

- Unity 2022.3 LTS o superior
- Universal Render Pipeline (URP)
- Input System Package
- TextMeshPro Package

## Configuración

### Build Settings
Las escenas deben estar configuradas en el siguiente orden:
1. MainMenu
2. cinematic1
3. Level1
4. cinematic2
5. Level2
6. cinematic3
7. EndMenu

### Videos
Los archivos de video deben estar ubicados en `Assets/Cinematics/Videos/`:
- `video1.mp4`: Cinemática de introducción
- `video2.mp4`: Cinemática intermedia
- `video3.mp4`: Cinemática final

## Desarrollo

### Estructura de Carpetas
```
Assets/
├── Characters/          # Scripts y sprites de personajes
├── Cinematics/         # Videos y scripts de cinemáticas
├── Scenes/            # Escenas del juego
├── Scripts/           # Scripts de gestión
├── UI/               # Interfaz de usuario
└── Art/              # Recursos artísticos
```

### Convenciones de Código
- Nombres de clases en PascalCase
- Nombres de métodos y variables en camelCase
- Comentarios en español
- Documentación de métodos públicos

## Licencia

Este proyecto es de uso educativo y personal.

## Contacto

Para consultas sobre el proyecto, contactar al desarrollador.
