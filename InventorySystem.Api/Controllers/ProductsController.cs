using Microsoft.AspNetCore.Mvc;
using InventorySystem.Domain;
using InventorySystem.Infrastructure;

namespace InventorySystem.Api.Controllers
{
    // [ApiController]: Indica que esta clase responde a peticiones Web
    // [Route]: Define la URL. [controller] toma el nombre de la clase (Products).
    // URL Final: http://localhost:xxxx/api/products
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _repository;

        // INYECCIÓN DE DEPENDENCIAS
        // El constructor pide un ProductRepository. 
        // Como lo configuramos en Program.cs, la API nos lo entrega listo para usar.
        public ProductsController(ProductRepository repository)
        {
            _repository = repository;
        }

        // 1. GET: api/products
        // Obtiene la lista completa de productos
        [HttpGet]
        public IActionResult GetAll()
        {
            try 
            {
                var list = _repository.GetAllProducts();
                return Ok(list); // Retorna HTTP 200 con el JSON de productos
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // 2. POST: api/products
        // Crea un producto nuevo
        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            try 
            {
                // Hardcodeamos el usuario creador porque aún no tenemos login por Token
                _repository.CreateProduct(product, "API_USER");
                
                return Ok(new { message = "Producto creado exitosamente", data = product });
            }
            catch (Exception ex)
            {
                // Si falla (ej: SKU duplicado), retornamos error 400
                return BadRequest(new { error = ex.Message });
            }
        }
        
        // 3. DELETE: api/products/{id}
        // Borra un producto
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _repository.DeleteProduct(id, "API_USER");
                return Ok(new { message = "Producto eliminado (Soft Delete)" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}