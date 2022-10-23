using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Thread.Sleep(5000);
            var result = _categoryService.GetAll();
            if (result.Success)
            {
                return Ok(result); // OK HTTP 200 Statü koduna denk gelir yani işlem başarılı demek
            }

            return BadRequest(result); // istek başarısız olursa burası çalışır
        }
    }
}
