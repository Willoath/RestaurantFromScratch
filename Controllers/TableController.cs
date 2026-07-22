using Microsoft.AspNetCore.Mvc;
using RestaurantFromScratch.Services;

namespace RestaurantFromScratch.Controllers
{
    public class TableController : ControllerBase
    {
        private readonly TableService _tableService;

        public TableController(TableService tableService)
        {
            _tableService = tableService;
        }
    }
}
