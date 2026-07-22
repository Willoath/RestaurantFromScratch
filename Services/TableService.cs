using RestaurantFromScratch.Data;
using RestaurantFromScratch.Models;
using SQLitePCL;

namespace RestaurantFromScratch.Services
{
    public class TableService
    {
        private readonly RestaurantContext _context;
        public TableService(RestaurantContext context) {
            _context = context;
        }
        public void AddTable(Table table)
        {
            _context.Tables.Add(table);
            _context.SaveChanges();
        }

    }
}
