# Estructura del Proyecto

## Organización de Carpetas

```
Assets/
├── Characters/                 # Personajes del juego
│   ├── Prefabs/              # Prefabs de personajes
│   ├── Scripts/              # Scripts de personajes
│   └── Sprites/              # Sprites de personajes
├── Cinematics/               # Sistema de cinemáticas
│   ├── Images/              # Imágenes de cinemáticas
│   ├── Prefabs/             # Prefabs de video
│   ├── Scripts/             # Scripts de cinemáticas
│   └── Videos/              # Archivos de video
├── Documentation/            # Documentación del proyecto
├── Prefabs/                 # Prefabs generales
│   ├── Enemies/            # Prefabs de enemigos
│   ├── Environment/        # Prefabs de ambiente
│   └── Items/              # Prefabs de objetos
├── Scenes/                  # Escenas del juego
│   ├── Cinematic/          # Escenas de cinemáticas
│   ├── Levels/             # Escenas de niveles
│   └── Menus/              # Escenas de menús
├── Scripts/                 # Scripts de gestión
│   ├── Managers/           # Scripts de gestión
│   ├── Enemies/            # Scripts de enemigos
│   ├── Player/             # Scripts del jugador
│   └── UI/                 # Scripts de interfaz
├── UI/                      # Interfaz de usuario
│   ├── Menus/              # Imágenes de menús
│   ├── Prefabs/            # Prefabs de UI
│   └── Scripts/            # Scripts de UI
└── Art/                     # Recursos artísticos
    ├── Backgrounds/        # Fondos de escenas
    ├── Effects/            # Efectos visuales
    ├── Sprites/            # Sprites generales
    └── UI/                 # Recursos de UI
```

## Descripción de Componentes

### Characters/
Contiene todos los elementos relacionados con personajes del juego:
- **Scripts**: Lógica de movimiento, disparo y comportamiento
- **Prefabs**: Objetos reutilizables de personajes
- **Sprites**: Recursos gráficos de personajes

### Cinematics/
Sistema completo de cinemáticas:
- **Videos**: Archivos de video para las cinemáticas
- **Scripts**: Gestión automática de reproducción
- **Images**: Imágenes relacionadas con cinemáticas

### Scenes/
Organización de escenas por categoría:
- **Cinematic**: Escenas de videos narrativos
- **Levels**: Escenas de juego jugable
- **Menus**: Escenas de interfaz

### Scripts/
Scripts organizados por funcionalidad:
- **Managers**: Control central del juego
- **Enemies**: Lógica de enemigos
- **Player**: Lógica del jugador
- **UI**: Interfaz de usuario

## Convenciones de Nomenclatura

### Archivos de Script
- Nombres en PascalCase
- Sufijo descriptivo del tipo (Manager, Controller, etc.)
- Ejemplo: `GameManager.cs`, `PlayerMovement.cs`

### Escenas
- Nombres en camelCase
- Prefijo descriptivo del tipo
- Ejemplo: `mainMenu`, `level1`, `cinematic1`

### Prefabs
- Nombres en PascalCase
- Sufijo "Prefab"
- Ejemplo: `PlayerPrefab`, `EnemyPrefab`

### Carpetas
- Nombres en PascalCase
- Nombres descriptivos y concisos
- Evitar abreviaciones innecesarias

## Archivos de Configuración

### Build Settings
- Orden específico de escenas requerido
- Configuración de plataforma objetivo
- Configuración de resolución

### Project Settings
- Input System configurado
- Universal Render Pipeline activo
- Configuración de audio

## Mantenimiento

### Limpieza Regular
- Eliminar archivos temporales
- Limpiar metadatos innecesarios
- Optimizar recursos gráficos

### Organización
- Mantener estructura de carpetas consistente
- Documentar cambios en estructura
- Revisar convenciones de nomenclatura
