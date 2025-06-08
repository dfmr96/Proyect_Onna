# 🧠 ONИA – Sistema de Stats Modular para Unity

Este sistema modular de stats está diseñado para ser **flexible, extensible y fácil de mantener**, ideal para juegos que requieren múltiples tipos de estadísticas base, progresión y mejoras durante la partida.

---

## 🏗️ Arquitectura General

El sistema se compone de los siguientes elementos clave:

### 📦 `StatDefinition`
- Representa la definición única de un stat (como `MaxHealth`, `Speed`, `AttackPower`).
- Es un `ScriptableObject` guardado como asset en `Assets/Stats/Definitions`.

---

### 📄 `StatBlock` (BaseStats)
- `ScriptableObject` que almacena los stats base de un personaje o entidad.
- Usa un contenedor serializado (`StatContainerLogic`) para mantener una lista editable y lookup en tiempo de ejecución.
- Permite agregar nuevos stats directamente desde el Inspector.

---

### 💾 `MetaStatBlock`
- Clase serializable usada para representar mejoras por progresión fuera de partida (metaprogressión).
- Utiliza internamente un `StatContainerLogic`.
- Puede serializarse fácilmente como `Dictionary<string, float>` para usar con Cloud Save.

---

### ⚙️ `RuntimeStats`
- Clase utilizada durante el gameplay que combina los siguientes valores:

  ```csharp
  Total = baseStats + metaStats + runtimeBonuses
