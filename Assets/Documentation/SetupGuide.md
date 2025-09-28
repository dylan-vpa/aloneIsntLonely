# Guía de Configuración

## Requisitos Previos

### Software Necesario
- Unity 2022.3 LTS o superior
- Visual Studio Code o Visual Studio
- Git (opcional, para control de versiones)

### Paquetes de Unity Requeridos
- Universal Render Pipeline (URP)
- Input System
- TextMeshPro
- Video Player

## Configuración del Proyecto

### 1. Configuración de Render Pipeline
1. Abrir **Edit > Project Settings > Graphics**
2. Seleccionar **UniversalRenderPipelineGlobalSettings** como Scriptable Render Pipeline Settings
3. Configurar **Quality Settings** para URP

### 2. Configuración de Input System
1. Abrir **Edit > Project Settings > Player**
2. En **Configuration**, cambiar **Active Input Handling** a **Input System Package (New)**
3. Reiniciar Unity si es necesario

### 3. Configuración de Escenas
1. Abrir **File > Build Settings**
2. Agregar las escenas en el siguiente orden:
   - MainMenu
   - cinematic1
   - Level1
   - cinematic2
   - Level2
   - cinematic3
   - EndMenu

## Configuración de Cinemáticas

### 1. Preparación de Videos
1. Colocar archivos de video en `Assets/Cinematics/Videos/`
2. Nombres requeridos:
   - `video1.mp4` (introducción)
   - `video2.mp4` (intermedio)
   - `video3.mp4` (final)

### 2. Configuración de Escenas de Cinemática
Para cada escena de cinemática (cinematic1, cinematic2, cinematic3):

1. **Crear Canvas**:
   - Right click en Hierarchy > UI > Canvas
   - Configurar Canvas Scaler: Scale With Screen Size
   - Reference Resolution: 1920x1080

2. **Crear VideoPlayer**:
   - Right click en Hierarchy > Video > Video Player
   - Source: Video Clip
   - Play On Awake: Activado
   - Loop: Desactivado

3. **Crear RawImage**:
   - Right click en Canvas > UI > RawImage
   - Stretch: Fill

4. **Crear Render Texture**:
   - Assets > Create > Render Texture
   - Size: 1920x1080
   - Asignar al VideoPlayer y RawImage

5. **Configurar AutoCinematicManager**:
   - Crear GameObject vacío: "CinematicManager"
   - Agregar script: AutoCinematicManager.cs
   - Asignar referencias:
     - Video Player: VideoPlayer
     - Video Display: RawImage
     - Cinematic Videos: [video1.mp4, video2.mp4, video3.mp4]

## Configuración de Niveles

### 1. Configuración de Level1
1. Configurar límites de pantalla
2. Agregar enemigos con script Enemy.cs
3. Configurar obstáculos estáticos
4. Asignar GameManager al jugador

### 2. Configuración de Level2
1. Configurar jefe con script Boss.cs
2. Ajustar dificultad y patrones de ataque
3. Configurar sistema de vidas
4. Asignar GameManager al jugador

## Configuración de Audio

### 1. Música de Fondo
1. Colocar archivo de música en `Assets/Audio/Ambient/`
2. Configurar AudioSource en la escena
3. Asignar clip de música
4. Configurar Loop: Activado

### 2. Efectos de Sonido
1. Colocar archivos de SFX en `Assets/Audio/SFX/`
2. Configurar AudioSource para efectos
3. Asignar clips a scripts correspondientes

## Testing y Debugging

### 1. Verificación de Flujo
1. Ejecutar desde MainMenu
2. Verificar transición a cinematic1
3. Verificar reproducción de video1
4. Verificar transición a Level1
5. Repetir para todos los niveles

### 2. Debugging Común
- Verificar que todas las escenas estén en Build Settings
- Verificar que los videos estén en la carpeta correcta
- Verificar que los scripts estén asignados correctamente
- Revisar la consola de Unity para errores

## Resolución de Problemas

### Problema: Video no se reproduce
- Verificar que el archivo de video esté en formato compatible
- Verificar que el VideoPlayer esté configurado correctamente
- Verificar que el Render Texture esté asignado

### Problema: Transición no funciona
- Verificar que las escenas estén en Build Settings
- Verificar que los nombres de escena coincidan exactamente
- Verificar que el SceneTransitionManager esté configurado

### Problema: Controles no responden
- Verificar que el Input System esté configurado
- Verificar que el PlayerInput esté asignado
- Verificar que los controles estén mapeados correctamente
