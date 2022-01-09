using System;

namespace ConsoleApp
{
    public class DbRepository : IDisposable
    {
        private readonly MyDbContext _context;

        public DbRepository(MyDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public void StoreBehaviour(Behaviour behaviour)
        {
            try
            {
                _context.Behaviours.Add(behaviour.ToDto());
                _context.SaveChanges();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                //Already in DB
            }
        }

        public Behaviour LoadBehaviour(string name)
        {
            return _context.Behaviours.Find(name).ToBehaviour();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool DeleteDB()
        {
            return _context.Database.EnsureDeleted();
        }
    }
}