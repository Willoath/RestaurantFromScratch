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
        [HttpGet("{id}")]
        public ActionResult<Table> GetTableById(int id)
        {
            var table = _tableService.GetTableById(id);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table);
        }
        [HttpPost]
        public ActionResult<Table> AddTable([FromBody] Table table)
        {
            _tableService.AddTable(table);
            return CreatedAtAction(nameof(GetTableById), new { id = table.Id }, table);

        }
        [HttpPut("{id}")]
        public ActionResult UpdateTable(int id, [FromBody] Table updatedTable)
        {
            var updated = _tableService.UpdateTable(id, updatedTable);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}