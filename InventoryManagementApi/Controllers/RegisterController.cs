using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using InventoryManagement.Domain.Interfaces.Services;

namespace InventoryManagmentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerServices;

        public RegisterController(IRegisterService registerServices)
        {
            _registerServices = registerServices;
        }

        //private readonly ILogger<RegisterController> _logger;

        //public RegisterController(ILogger<RegisterController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct([FromBody] string productName)
        {
            try
            {
                // Chamar o método da sua classe de serviço para registrar o novo produto
                int productId = _registerServices.RegisterNewProduct(productName);

                // Retornar um código de status HTTP 201 Created e o ID do novo produto
                return CreatedAtRoute("GetProductById", new { id = productId }, null);
            }
            catch (ArgumentException ex)
            {
                // Retornar um código de status HTTP 400 Bad Request se o nome do produto for inválido
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Retornar um código de status HTTP 409 Conflict se um produto com o mesmo nome já existir
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                // Retornar um código de status HTTP 500 Internal Server Error se ocorrer um erro inesperado
                return StatusCode(500, ex.Message);
            }
        }
    }
}

