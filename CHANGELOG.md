# Changelog

## [1.0.0] - 2024-12-19

### Agregado
- Sistema completo de cinemáticas integrado
- Flujo de juego: MainMenu → Cinematic1 → Level1 → Cinematic2 → Level2 → Cinematic3 → EndMenu
- Script AutoCinematicManager para reproducción automática de videos
- Documentación profesional completa en español
- Sistema de transiciones suaves entre escenas

### Modificado
- MenuManager.cs: StartGame() ahora carga Cinematic1 en lugar de Level1
- SimpleButton.cs: Actualizado para usar sistema de cinemáticas
- GameManager.cs: OnLevelComplete() actualizado para cargar cinemáticas entre niveles

### Eliminado
- Archivos de documentación temporal (.txt)
- Archivos de configuración obsoletos
- Documentación redundante

### Documentación
- README.md: Documentación principal del proyecto
- GameDesign.md: Documentación de diseño del juego
- TechnicalDocumentation.md: Documentación técnica detallada
- SetupGuide.md: Guía de configuración paso a paso
- ProjectStructure.md: Estructura y organización del proyecto

### Estructura del Proyecto
- Organización limpia de carpetas
- Convenciones de nomenclatura establecidas
- Documentación centralizada en Assets/Documentation/
- Eliminación de archivos innecesarios

### Configuración Requerida
- Build Settings con orden correcto de escenas
- Videos en Assets/Cinematics/Videos/ (video1.mp4, video2.mp4, video3.mp4)
- Configuración de escenas de cinemática con AutoCinematicManager
- Universal Render Pipeline y Input System configurados
