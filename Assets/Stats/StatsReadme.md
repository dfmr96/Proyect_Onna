# 🧠 ONИA – Sistema de Stats Modular

Este sistema te permite gestionar los stats del jugador de forma flexible, segura y escalable, utilizando `StatDefinition`, `StatBlock`, `RuntimeStats` y `StatRegistry`.

---

## ✅ Flujo para crear nuevos Stats

### 1. Crear un nuevo `StatDefinition`
Desde cualquier `StatBlock` (como `CharacterBaseStats.BaseStats`):

1. En el Inspector, llená los campos:
    - `New Stat Name`: Nombre del stat (ej. `BleedChance`)
    - `New Stat Value`: Valor base inicial
2. Presioná el botón **"Add New Stat"**.

Esto creará:
- El `StatDefinition` en `Assets/Stats/Definitions/`
- Una nueva entrada en el `StatBlock`

---

### 2. Actualizar el `StatRegistry`

1. Abrí el asset `StatRegistry.asset`.
2. Clic derecho sobre el asset > **Auto-Fill From Project**.

Esto escaneará todos los assets `StatDefinition` del proyecto y los añadirá a la lista.

---

### 3. Enlazar el nuevo stat en `StatReferences` (opcional pero recomendado)

1. Abrí el asset `StatReferences.asset`.
2. Clic derecho sobre el asset > **Auto Link From Registry**.

> Si el nombre del nuevo stat coincide con un campo en la clase `StatReferences.cs`, será asignado automáticamente.

---

## ✏️ ¿Querés usarlo en código?

Usá `StatReferences` para evitar strings mágicos:

```csharp
float speed = runtimeStats.Get(statRefs.MovementSpeed);
