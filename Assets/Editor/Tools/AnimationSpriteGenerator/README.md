# Animation Sprite Generator

Herramienta para capturar sprites de GameObjects animados en frames específicos con vista previa interactiva.

## Características

- **Vista previa interactiva** en el Editor de Unity
- Soporte para cualquier GameObject con componente **Animator**
- Selección de **capas** y **estados de animación**
- Control **frame-by-frame** con slider normalizado (0.0 - 1.0)
- **Auto-framing** y **auto-scaling** de objetos
- Background transparente o color personalizado
- Resolución configurable
- Nombrado automático: `{ObjectName}_{StateName}_F{Frame}.png`

## Cómo Usar

### 1. Abrir la Ventana

En Unity Editor:
```
Window > Animation Sprite Generator
```

### 2. Configurar Target Object

1. **GameObject**: Arrastra el objeto con Animator que quieres capturar
2. **Render Camera**: Asigna una cámara de la escena (puede ser cualquier cámara, se configurará automáticamente)

### 3. Seleccionar Animación

1. **Layer**: Elige la capa del Animator (Base Layer, UpperBody, etc.)
2. **Animation State**: Selecciona el estado de animación (Idle, Attack, Reload, etc.)

### 4. Navegar por la Animación

- Usa el **slider "Frame (Normalized)"** para moverte por la animación (0.0 = inicio, 1.0 = fin)
- Botones rápidos: **Start (0%)**, **Mid (50%)**, **End (100%)**
- La vista previa se actualiza automáticamente (configurable con "Auto Refresh Preview")

### 5. Capturar el Frame

1. Ajusta el slider al frame deseado
2. Click en **"Capture Current Frame"**
3. El PNG se guardará en la carpeta de salida configurada

## Configuración (Settings)

- **Resolution**: Tamaño del sprite generado (default: 512x512)
- **Output Folder**: Carpeta donde se guardan los PNGs (default: `Assets/AnimationSprites`)
- **Background Color**: Color de fondo (default: transparente)

## Ejemplos de Uso

### Capturar pose de apuntar
1. GameObject: Runner/Cosme (personaje jugador)
2. Layer: UpperBody
3. State: Pistol_Idle
4. Frame: 0.5 (mitad de la animación)
5. Capture → `Runner_Pistol_Idle_F050.png`

### Capturar frame de recarga
1. GameObject: Player con arma
2. Layer: Base Layer
3. State: Reload
4. Frame: 0.75 (momento de insertar cargador)
5. Capture → `Player_Reload_F075.png`

### Capturar enemigo atacando
1. GameObject: EnemyMelee
2. Layer: Base Layer
3. State: Attack1
4. Frame: 0.3 (peak del ataque)
5. Capture → `EnemyMelee_Attack1_F030.png`

## Casos de Uso

- **UI Icons**: Crear iconos de habilidades, ataques, poses
- **Documentation**: Documentar animaciones del proyecto
- **Marketing**: Generar assets para promotional material
- **Reference**: Crear referencias visuales para animadores
- **Debugging**: Verificar frames específicos de animaciones

## Notas Técnicas

- La herramienta **NO reproduce** la animación, solo la samplea en el frame específico
- Soporta **Animator Controllers** con múltiples capas
- **Auto-framing** calcula el bounds del objeto y ajusta la cámara automáticamente
- Los sprites se guardan con **transparencia** (ARGB32)
- Compatible con objetos que tienen **múltiples Renderers** (hijos)

## Troubleshooting

### "No Animator component found"
- Asegúrate de que el GameObject tenga un componente **Animator**
- Verifica que el Animator tenga un **Controller** asignado

### "No animation states found"
- Verifica que el Animator Controller tenga estados definidos
- Revisa que la capa seleccionada no esté vacía

### Preview no se actualiza
- Verifica que "Auto Refresh Preview" esté activado
- Click en "Refresh Preview" manualmente
- Asegúrate de que la cámara esté asignada

### Objeto no visible en preview
- La cámara ajusta automáticamente, pero verifica que los Renderers estén activos
- Revisa que el objeto no esté en una capa (Layer) que la cámara no renderiza

## Diferencias con PropSpriteGenerator

| Feature | PropSpriteGenerator | AnimationSpriteGenerator |
|---------|---------------------|--------------------------|
| Uso | Props estáticos | GameObjects animados |
| Control | Batch processing | Frame-by-frame interactivo |
| Preview | No | Sí, en tiempo real |
| Animación | No soporta | Control total de frames |
| UI | Context menu | Editor Window completo |

Ambas herramientas conviven en el proyecto para diferentes necesidades.
