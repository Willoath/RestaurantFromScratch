using RestaurantFromScratch.Data;
using RestaurantFromScratch.Models;
using SQLitePCL;

namespace RestaurantFromScratch.Services
{
    public class TableService
    {
        private readonly RestaurantContext _context;
        public TableService(RestaurantContext context)
        {
            _context = context;
        }
        public void AddTable(Table table)
        {
            _context.Tables.Add(table);
            _context.SaveChanges();
        }
        public List<Table> GetAllTables()
        {
            return _context.Tables.ToList();
        }

        public Table? GetTableById(int id)
        {
            return _context.Tables.FirstOrDefault(t => t.Id == id);
        }
        public void UpdateTable(int id, Table updatedTable)
        {
            var existingTable = _context.Tables.FirstOrDefault(t => t.Id == id);
            if (existingTable == null)
            {
                return;
            }

            existingTable.TableNumber = updatedTable.TableNumber;
            existingTable.Seats = updatedTable.Seats;
            existingTable.IsActive = updatedTable.IsActive;
            _context.SaveChanges();


        }

    }
}
