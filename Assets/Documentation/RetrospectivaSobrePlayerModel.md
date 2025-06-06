# 🧠 Reflexión sobre la Arquitectura de Stats en Onna

Desde que empecé a trabajar en **Onna**, sabía que el sistema de stats iba a ser uno de los pilares más importantes, porque el juego tiene dos capas bien distintas de progresión:

- 🧬 **Mutaciones intra-run**, que son mejoras temporales.
- 💎 **Mejoras de metaprogresión**, que se compran en la tienda y persisten entre runs.

---

## 🎯 Objetivo

Evitar lógica duplicada o acoplada al `PlayerModel`, y diseñar una arquitectura flexible para manejar stats sin importar el contexto (hub o run).

---

## 🧩 Solución

Diseñé un sistema basado en interfaces:

```csharp
public interface IStatSource {
    float Get(StatDefinition stat);
}

public interface IStatTarget {
    void AddFlatBonus(StatDefinition stat, float value);
    void AddPercentBonus(StatDefinition stat, float percent);
}
```
Y luego creé el PlayerStatContext, que se encarga de decidir si el PlayerModel usa RuntimeStats (en run) o MetaStatBlock (en el hub).

## 🧠 Lo que logré
El PlayerModel no sabe qué clase concreta le da los stats. Solo accede a interfaces.

Las mutaciones pueden ser ScriptableObjects desacoplados, como:

```csharp
statTarget.AddPercentBonus(statRefs.attackSpeed, 10f);
```

* La tienda mejora directamente el MetaStatBlock, y en la próxima run esas mejoras se reflejan en RuntimeStats.
* Puedo testear cualquier parte del sistema con mocks sin levantar una escena de Unity.
* Escalar a 50+ mutaciones o 30+ upgrades permanentes no requiere reescribir nada.

## ✅ Conclusión
Separé completamente el gameplay de la lógica de stats.
Ahora tengo un sistema testable, flexible y preparado para escalar.