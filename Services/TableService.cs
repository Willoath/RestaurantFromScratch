using RestaurantFromScratch.Data;
using RestaurantFromScratch.Dtos;
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
        public List<AvailableTableDto> GetAvailableTables(AvailableTablesQueryDto query)
        {
            return _context.Tables.Where(t => t.IsActive && t.Seats >= query.NumberOfGuests && !_context.Reservations.Any(r => r.TableId == t.Id && query.ReservationStart < r.ReservationEnd &&
        query.ReservationEnd > r.ReservationStart)).Select(t => new AvailableTableDto
        {
            Id = t.Id,
            TableNumber = t.TableNumber,
            Seats = t.Seats
        }).ToList();
        }

        public Table? GetTableById(int id)
        {
            return _context.Tables.FirstOrDefault(t => t.Id == id);
        }
        public bool UpdateTable(int id, Table updatedTable)
        {
            var existingTable = _context.Tables.FirstOrDefault(t => t.Id == id);
            if (existingTable == null)
            {
                return false;
            }

            existingTable.TableNumber = updatedTable.TableNumber;
            existingTable.Seats = updatedTable.Seats;
            existingTable.IsActive = updatedTable.IsActive;
            _context.SaveChanges();
            return true;

        }
        public bool DeleteTable(int id)
        {
            var table = _context.Tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                return false;
            }
            _context.Tables.Remove(table);
            _context.SaveChanges();
            return true;
        }

    }

}
