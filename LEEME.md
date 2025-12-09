comando para hacer correr el proyecto:

dotnet run --project InventorySystem.Api

URL Base: http://localhost:5112/api/products (El puerto puede variar, revísalo en tu terminal).

1. Prueba de Lectura (GET)
Objetivo: Verificar que la API puede leer de la base de datos SQLite.

Método: GET

URL: /api/products

Body: (Vacío)

Resultado Esperado: Código 200 OK y un JSON con la lista de productos (o [] si está vacía).

2. Prueba de Creación (POST)
Objetivo: Verificar que la API puede escribir nuevos datos validando reglas.

Método: POST

URL: /api/products

Body (JSON):

JSON

{
  "name": "Laptop Gamer",
  "sku": "TECH-999",
  "category": 1,
  "isPerishable": false
}
(Nota: Category 1 = Electronics).

Resultado Esperado: Código 200 OK y mensaje de éxito.

3. Prueba de Error / Validación (POST Negativo)
Objetivo: Verificar que el sistema no permite duplicados (Integridad de Datos).

Acción: Envía exactamente la misma petición de la "Prueba 2" otra vez.

Resultado Esperado: Código 400 Bad Request o 500 Internal Server Error con un mensaje indicando que el SKU ya existe.

4. Prueba de Eliminación (DELETE)
Objetivo: Verificar el borrado lógico (Soft Delete).

Método: DELETE

URL: /api/products/1 (O el ID que quieras borrar)

Body: (Vacío)

Resultado Esperado: Código 200 OK.

5. Prueba de Verificación Final
Acción: Ejecuta la Prueba 1 (GET) de nuevo.

Resultado Esperado: El producto "Laptop Gamer" debe aparecer, pero el producto con ID 1 (si lo borraste en el paso 4) ya no debería salir en la 