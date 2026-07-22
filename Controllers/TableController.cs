using Microsoft.AspNetCore.Mvc;
using RestaurantFromScratch.Services;
using RestaurantFromScratch.Models;

namespace RestaurantFromScratch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly TableService _tableService;

        public TableController(TableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public List<Table> GetAllTables()
        {
            return _tableService.GetAllTables();
        }

    }

    } 
