# 🧩 Patrones de Diseño Aplicados en el Sistema de Stats de Onna

Este documento describe los patrones de diseño utilizados en la arquitectura del sistema de stats de Onna. Cada patrón justifica por qué el sistema está construido de esta manera y qué beneficios aporta en términos de flexibilidad, testabilidad y mantenimiento.

---

## 🔷 1. Strategy Pattern

**Dónde:**  
`PlayerStatContext` selecciona dinámicamente entre `RuntimeStats` y `MetaStatBlock`, ambos implementando `IStatSource` / `IStatTarget`.

**Qué hace:**  
Permite cambiar el "algoritmo" de acceso y modificación de stats sin modificar el `PlayerModel`.

**Beneficio:**  
`PlayerModel` no necesita saber si está usando stats de metaprogresión o temporales. Solo interactúa con una interfaz.

---

## 🔷 2. Dependency Injection

**Dónde:**  
Método `PlayerModel.InjectStatContext(...)`.

**Qué hace:**  
En lugar de crear sus propias dependencias, el `PlayerModel` recibe desde afuera un `PlayerStatContext` ya configurado.

**Beneficio:**  
- Alta testabilidad.
- Bajo acoplamiento.
- Fácil de cambiar entre contexto de hub y run.
- Compatible con contenedores como VContainer o Zenject.

---

## 🔷 3. Interface Segregation Principle (ISP)

**Dónde:**  
Interfaces `IStatSource` y `IStatTarget`.

**Qué hace:**  
Define interfaces pequeñas y específicas en lugar de una grande que haga todo.

**Beneficio:**  
Los consumidores (como efectos, UI o `PlayerModel`) solo dependen de lo que realmente usan, sin acoplarse a implementaciones concretas o a métodos innecesarios.

---

## 🔷 4. Facade Pattern (ligero)

**Dónde:**  
`PlayerStatContext`.

**Qué hace:**  
Actúa como una interfaz simplificada sobre `RuntimeStats` y `MetaStatBlock`, ocultando la complejidad de selección y acceso.

**Beneficio:**  
Los objetos consumidores no necesitan saber cómo están implementados los stats ni cuál está activo.  
Solo interactúan con un punto de acceso claro y limpio.

---

## 🔷 5. Composition Over Inheritance

**Dónde:**  
`PlayerModel` no hereda de ninguna clase de stats ni usa subclases.

**Qué hace:**  
Usa composición: contiene una instancia de `PlayerStatContext` que maneja toda la lógica de stats.

**Beneficio:**  
- Mayor flexibilidad.
- Fácil de extender o reemplazar sin romper jerarquías.
- Menor acoplamiento.

---

## 🔷 6. Single Responsibility Principle (SRP)

**Dónde:**

- `PlayerModel` → lógica de gameplay (daño, curación, muerte).
- `PlayerStatContext` → selección y acceso a stats.
- `RuntimeStats` / `MetaStatBlock` → almacenamiento y cálculo de stats.

**Qué hace:**  
Cada clase tiene una única razón para cambiar.

**Beneficio:**  
Mejor organización, facilidad para testear o extender cada parte por separado.

---

## ✅ Conclusión

La arquitectura actual del sistema de stats en Onna no es solo limpia, sino que está fundamentada en varios principios de diseño sólidos.  
Estos patrones permiten un sistema **escalable, mantenible y flexible**, ideal para un juego con:

- Progresión permanente.
- Mejora dinámica durante la run.
- Mutaciones.
- Sistemas de tienda y hub.

