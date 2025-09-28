# Documentación de Diseño del Juego

## Concepto General

Alone Isn't Lonely es un juego de acción 2D que explora temas de soledad y superación personal a través de mecánicas de combate y narrativa cinematográfica.

## Mecánicas de Juego

### Sistema de Movimiento
- Movimiento en 8 direcciones usando Input System
- Velocidad de movimiento configurable
- Detección de límites de pantalla

### Sistema de Combate
- Disparo de proyectiles en dirección del movimiento
- Sistema de colisiones con enemigos
- Mecánica de vidas con sistema de invencibilidad temporal

### Progresión de Dificultad
- **Nivel 1**: Enemigos básicos con patrón de movimiento simple
- **Nivel 2**: Jefe final con mecánicas avanzadas y mayor resistencia

## Diseño de Niveles

### Nivel 1: Encuentro Inicial
- Enemigos con movimiento horizontal
- Obstáculos estáticos
- Objetivo: Supervivencia y aprendizaje de mecánicas

### Nivel 2: Confrontación Final
- Jefe con múltiples fases de ataque
- Patrones de movimiento complejos
- Objetivo: Dominio de mecánicas y resolución narrativa

## Sistema de Cinemáticas

### Propósito Narrativo
- Introducción al mundo del juego
- Desarrollo del conflicto principal
- Resolución y conclusión de la historia

### Implementación Técnica
- Reproducción automática de videos
- Transiciones suaves entre escenas
- Sistema de skip opcional

## Interfaz de Usuario

### Menú Principal
- Botón de inicio de juego
- Opciones de salida
- Diseño minimalista y accesible

### HUD de Juego
- Contador de vidas
- Indicadores de estado
- Información contextual

### Menú de Fin
- Opción de reinicio
- Retorno al menú principal
- Celebración de logros

## Balance de Dificultad

### Factores de Dificultad
- Velocidad de enemigos
- Frecuencia de aparición
- Duración de invencibilidad
- Patrones de ataque del jefe

### Progresión del Jugador
- Curva de aprendizaje gradual
- Mecánicas introducidas progresivamente
- Recompensa por dominio de controles

## Consideraciones de Accesibilidad

### Controles
- Soporte para múltiples dispositivos de entrada
- Configuración de controles personalizable
- Feedback visual y auditivo

### Dificultad
- Opciones de dificultad ajustables
- Sistema de vidas configurable
- Mecánicas de asistencia opcionales
