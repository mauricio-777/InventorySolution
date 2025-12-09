using Microsoft.AspNetCore.Mvc;
using InventorySystem.Domain;
using InventorySystem.Infrastructure;

namespace InventorySystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _repository;

        public ProductsController(ProductRepository repository)
        {
            _repository = repository;
        }

        // 1. GET: api/products (Leer todos)
        [HttpGet]
        public IActionResult GetAll()
        {
            try 
            {
                var list = _repository.GetAllProducts();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // 2. POST: api/products (Crear nuevo)
        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            try 
            {
                _repository.CreateProduct(product, "API_USER");
                return Ok(new { message = "Producto creado", data = product });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // 3. PUT: api/products/{id} (Actualizar)
        // La ruta recibe el ID, ejemplo: api/products/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            try
            {
                // Validación de seguridad básica: 
                // Aseguramos que el ID de la URL coincida con el ID del JSON (si viene)
                if (id != product.Id && product.Id != 0)
                {
                    return BadRequest(new { error = "El ID de la URL no coincide con el cuerpo" });
                }

                // Aseguramos que el objeto tenga el ID correcto para que el Repo sepa cuál tocar
                product.Id = id;

                // Llamamos al repositorio del Core
                _repository.UpdateProduct(product, "API_USER");
                
                return Ok(new { message = $"Producto {id} actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // 4. DELETE: api/products/{id} (Eliminar)
        // Ejemplo: api/products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _repository.DeleteProduct(id, "API_USER");
                return Ok(new { message = $"Producto {id} eliminado (Soft Delete)" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}