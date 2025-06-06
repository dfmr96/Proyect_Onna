# 🧠 Sistema de Interfaces de Stats - Onna

Este documento resume las interfaces clave para lectura y modificación de stats en tiempo de ejecución y metaprogresión.

---

## 📘 IStatSource

```csharp
public interface IStatSource
{
    float Get(StatDefinition stat);
}
```

### Función:
Interfaz de solo lectura. Permite acceder al valor actual de un stat.

### Implementaciones:
- `RuntimeStats`: suma base + meta + bonus temporales.
- `MetaStatBlock`: devuelve solo mejoras permanentes.
- `MetaStatReader` (opcional): suma base + meta sin bonus runtime.

---

## 🛠️ IStatTarget

```csharp
public interface IStatTarget
{
    void AddFlatBonus(StatDefinition stat, float value);
    void AddPercentBonus(StatDefinition stat, float percent);
}
```

### Función:
Permite aplicar mejoras sobre cualquier contenedor de stats, como efectos de mutación o tienda.

### Implementaciones:
- `RuntimeStats`: usa `AddRuntimeBonus(...)`.
- `MetaStatBlock`: usa `Set(...)` directamente sobre el valor persistente.

---

## 🎯 Aplicaciones

| Contexto        | Uso                      | Objeto             |
|------------------|---------------------------|----------------------|
| Gameplay (Run)   | Mutaciones temporales     | `RuntimeStats`       |
| Hub (Tienda)     | Mejora permanente         | `MetaStatBlock`      |
| UI Preview       | Leer sin modificar        | `IStatSource`        |

---

## 🧪 Ejemplo de un efecto reutilizable

```csharp
public class SpeedBoostEffect : ScriptableObject
{
    [SerializeField] private StatReferences statRefs;
    [SerializeField] private float percent;

    public void Apply(IStatTarget target)
    {
        target.AddPercentBonus(statRefs.movementSpeed, percent);
    }
}
```

Y se aplica así:

```csharp
effect.Apply(runtimeStats);     // Durante una run
effect.Apply(metaStatBlock);    // Desde la tienda
```

---

## ✅ Beneficios

- Desacopla efectos de su destino.
- Permite compartir lógica entre tienda y mutaciones.
- Evita ifs tipo `if (isHub) ... else if (isRun) ...`.
- Mejora testeo, extensibilidad y claridad del diseño.
