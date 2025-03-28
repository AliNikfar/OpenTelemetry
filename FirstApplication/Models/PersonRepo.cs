using Microsoft.EntityFrameworkCore;

namespace FirstApplication.Models
{
    public class PersonRepo
    {
        private readonly PersonDbContext _db;

        public PersonRepo(PersonDbContext db)
        {
            _db = db;
        }
        public async Task<List<Person>> GetAll()
         {
            // add thread sleep to simulate load waiting
            Thread.Sleep(2000);
            return await _db.Persons.ToListAsync();
         }
    }
}
