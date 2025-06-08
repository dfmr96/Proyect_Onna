# 🧠 PlayerModel: Arquitectura Flexible y Desacoplada de Stats para Onna

Este documento resume cómo estructurar el `PlayerModel` para que funcione correctamente tanto en el **hub** como en la **run**, usando interfaces (`IStatSource` / `IStatTarget`) y una clase intermediaria (`PlayerStatContext`) que permite desacoplar completamente los datos de stats del modelo de gameplay.

---

## 🎯 Objetivo

- Separar completamente la lógica de gameplay (`PlayerModel`) del sistema de stats.
- Usar interfaces (`IStatSource`, `IStatTarget`) para no depender de clases concretas como `RuntimeStats` o `MetaStatBlock`.
- Facilitar el testing, extensión y reutilización de código entre modos de juego.
- Permitir cambiar de contexto entre **hub** y **run** de forma flexible.

---

## 🧩 Interfaces base

```csharp
public interface IStatSource
{
    float Get(StatDefinition stat);
}

public interface IStatTarget
{
    void AddFlatBonus(StatDefinition stat, float value);
    void AddPercentBonus(StatDefinition stat, float percent);
}
```
## 🧱 PlayerStatContext
Esta clase administra qué fuente de stats está activa en un momento dado.

```csharp
public class PlayerStatContext
{
private IStatSource source;
private IStatTarget target;

    private RuntimeStats runtimeStats;
    private MetaStatBlock metaStats;

    public void SetupForRun(StatBlock baseStats, MetaStatBlock metaStats, StatReferences statRefs)
    {
        runtimeStats = new RuntimeStats(baseStats, metaStats, statRefs);
        this.metaStats = metaStats;

        source = runtimeStats;
        target = runtimeStats;

        RunData.SetStats(runtimeStats); // opcional
    }

    public void SetupForHub(MetaStatBlock metaStats)
    {
        this.metaStats = metaStats;
        source = metaStats;
        target = metaStats;
    }

    public IStatSource Source => source;
    public IStatTarget Target => target;

    public RuntimeStats Runtime => runtimeStats;
    public MetaStatBlock Meta => metaStats;
}
```
## 🧱 PlayerModel desacoplado

```csharp
public class PlayerModel : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField] private StatReferences statRefs;

    private PlayerStatContext statContext;
    private float currentTime;

    public void InjectStatContext(PlayerStatContext context)
    {
        statContext = context;
        currentTime = statContext.Runtime != null ? statContext.Runtime.CurrentEnergyTime : float.PositiveInfinity;
    }

    public float Speed => statContext.Source.Get(statRefs.movementSpeed);
    public float MaxHealth => statContext.Source.Get(statRefs.maxVitalTime);
    public float CurrentHealth => currentTime;

    private void Update()
    {
        if (statContext.Runtime != null)
        {
            float drain = statContext.Source.Get(statRefs.passiveDrainRate) * Time.deltaTime;
            ApplyDamage(drain, false);
        }
    }

    public void ApplyDamage(float timeTaken, bool applyResistance)
    {
        float resistance = applyResistance ? Mathf.Clamp01(statContext.Source.Get(statRefs.damageResistance)) : 0f;
        float finalDamage = timeTaken * (1f - resistance);

        currentTime -= finalDamage;
        ClampEnergy();

        if (currentTime <= 0f)
            Die();
    }

    public void RecoverTime(float timeRecovered)
    {
        currentTime = Mathf.Min(currentTime + timeRecovered, MaxHealth);
        ClampEnergy();
    }

    private void ClampEnergy()
    {
        if (statContext.Runtime != null)
            statContext.Runtime.SetCurrentEnergyTime(currentTime, MaxHealth);
    }

    private void Die()
    {
        Debug.Log("☠️ Player died");
    }
}
```
## 🚀 Bootstrapper

```csharp
public class PlayerModelBootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private GameMode currentMode;

    [Header("Stats Setup")]
    [SerializeField] private StatBlock baseStats;
    [SerializeField] private MetaStatBlock metaStats;
    [SerializeField] private StatReferences statRefs;

    private void Awake()
    {
        var statContext = new PlayerStatContext();

        if (currentMode == GameMode.Run)
            statContext.SetupForRun(baseStats, metaStats, statRefs);
        else
            statContext.SetupForHub(metaStats);

        playerModel.InjectStatContext(statContext);
    }
}
```
## 📄 GameMode.cs
```csharp
public enum GameMode
{
    Hub,
    Run
}
```
## 🔎 Propiedad de los datos

| Objeto                         | ¿Propietario del dato? | ¿Qué contiene?                                                        | ¿Quién lo referencia?                                 |
|-------------------------------|------------------------|------------------------------------------------------------------------|--------------------------------------------------------|
| `MetaStatBlock` (Bootstrapper) | ✅ **Sí**              | Mejora permanente (progresión)                                        | `PlayerStatContext`, `RuntimeStats`, tienda, UI, etc. |
| `StatBlock` (Bootstrapper)     | ✅ **Sí**              | Stats base iniciales (sin mejoras)                                    | `RuntimeStats` via `PlayerStatContext`                |
| `RuntimeStats`                 | ❌ **No (copia)**      | Copia de `StatBlock` + referencia a `MetaStatBlock` + bonuses runtime | `PlayerModel`, efectos, mutaciones                    |
| `PlayerStatContext`            | ❌ **No**              | Administra `IStatSource` y `IStatTarget` activos                      | `PlayerModel`                                         |
| `PlayerModel`                  | ❌ **No**              | Solo accede vía interfaces                                            | -                                                     |

## ✅ Beneficios
🔌 PlayerModel no depende de RuntimeStats ni MetaStatBlock.

♻️ Reutilización de PlayerStatContext en otros sistemas.

🧪 Testeable con mocks y simulaciones.

🔄 Cambio de contexto flexible (hub/run).

📦 Separación de concerns clara y mantenible.

## 🧩 Futuras mejoras
* Crear interfaz IStatContext para mayor testabilidad.
* Permitir switching dinámico de contexto para previews de tienda.
* Integrar MetaStatBlock con sistema de guardado/carga.
* Expandir para buffs globales o reliquias.