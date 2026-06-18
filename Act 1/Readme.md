# 📋 Checklist del Proyecto

## 1. Historias de Usuario (Funcionalidad Requerida)

La aplicación debe cumplir con las siguientes historias de usuario. Marca las casillas conforme se vayan implementando en el código.

### Colecciones y Ordenamiento

* [ ] **Agregar y Listar:** El usuario puede registrar un nuevo componente de PC y visualizar la lista completa del inventario sin un límite de capacidad predefinido (**Uso de `List<T>`**).

* [ ] **Deshacer acción:** El usuario puede deshacer su modificación más reciente, manteniendo intactas las acciones anteriores (**Uso de `Stack<T>`**).

* [ ] **Procesamiento en orden de llegada:** El sistema puede procesar solicitudes de armados en el orden exacto en que fueron creadas (**Uso de `Queue<T>`**).

* [ ] **Reordenamiento de prioridad:** El usuario puede mover un pedido específico al frente de la cola de trabajo sin reconstruir toda la estructura (**Uso de `LinkedList<T>`**).

* [ ] **Vista de cuadrícula:** El sistema puede mostrar un mapa de ubicaciones físicas (estantes/pasillos) para localizar los componentes (**Uso de arreglo bidimensional `T[,]`**).

### Manejo de Errores y Registro (Logging)

* [ ] **Recuperación de errores:** Si el usuario busca un identificador inexistente, el sistema muestra un mensaje claro indicando el error y continúa ejecutándose sin interrumpirse (**Uso de `try/catch/finally` y Excepciones Personalizadas**).

* [ ] **Registro de actividad:** La aplicación mantiene un historial de las acciones realizadas, categorizadas por severidad: Información, Advertencia y Error (**Uso de Serilog**).

### Búsqueda y Filtrado Eficiente

* [ ] **Búsqueda indexada:** El usuario puede recuperar un componente instantáneamente utilizando su identificador único (SKU) sin escanear toda la colección (**Uso de `Dictionary<TKey, TValue>`**).

* [ ] **Valores únicos:** El usuario puede visualizar atributos específicos disponibles (ej. Marcas) sin elementos duplicados (**Uso de `HashSet<T>`**).

* [ ] **Iteración completa:** El sistema puede recorrer y mostrar todo el inventario mediante un solo comando de listado (**Uso de `IEnumerable<T>` y `yield return`**).

* [ ] **Búsqueda condicional:** El usuario puede filtrar el inventario basándose en condiciones personalizadas (**Uso de expresiones Lambda / `Predicate<T>`**).

### Datos Externos y Validación

* [ ] **Consumo de API:** El sistema enriquece los datos locales conectándose a una API pública sin bloquear la ejecución de la aplicación (**Uso de `HttpClient` y `async/await`**).

* [ ] **Resiliencia Offline (Fallback):** Si la conexión de red falla, la aplicación maneja el error internamente y utiliza datos de respaldo locales en lugar de interrumpirse.

* [ ] **Validación estricta de entrada:** El sistema verifica que los identificadores y datos críticos cumplan con un formato específico antes de procesarlos (**Uso de `Regex`**).

---

## 2. Requisitos Técnicos y de Ingeniería

Para considerar la entrega completada, el código debe incluir obligatoriamente las siguientes implementaciones técnicas:

* [ ] **Tipos de Datos:** Uso de `enum` y al menos un `readonly struct`.

* [ ] **Genéricos:** Implementación de al menos una clase o tipo genérico personalizado.

* [ ] **Patrón de Diseño:** Implementación de un patrón de Repositorio (mediante una interfaz) o un patrón Factory.

* [ ] **Logging Estructurado:** Configuración de Serilog utilizando plantillas estructuradas.

  ```csharp
  Log.Information("Componente agregado {Id}", id);
  ```

  Evitar la concatenación de cadenas.

* [ ] **Manejo de JSON:** Deserialización de respuestas JSON extrayendo únicamente los campos necesarios y mapeándolos directamente al objeto de dominio.

* [ ] **Sintaxis C#:** Uso de al menos un miembro expression-bodied (`=>`) y la utilización de clases `partial` o `sealed`.

* [ ] **Validación Avanzada:** Uso de al menos una técnica de validación avanzada:

  * Parámetro `out`
  * Tipos anulables (`nullable`) con operador `??`
  * Pattern matching en un bloque `switch`

---

## 3. Stretch Goals (Objetivos Adicionales)

Como equipo, hemos seleccionado los siguientes objetivos adicionales para implementar:

* [ ] **Restricción Genérica (Generic Constraint):** Añadir la restricción:

  ```csharp
  where T : Component
  ```

  a la clase genérica personalizada para garantizar la seguridad de tipos.

  > Se documentará en el Pull Request por qué esta restricción es adecuada para el dominio.

* [ ] **Segundo destino de Logs (Serilog Sink):** Configurar un segundo sink para Serilog:

  ```csharp
  .WriteTo.File("logs/app.log")
  ```

  con el fin de almacenar un registro persistente de la actividad en un archivo de texto, verificando que esto no requiera cambios en las llamadas de log existentes.
