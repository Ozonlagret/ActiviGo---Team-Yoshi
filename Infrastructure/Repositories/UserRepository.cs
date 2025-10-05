using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ActiviGoDbContext _db;
        public UserRepository(ActiviGoDbContext db) => _db = db;

        public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
            _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == email, ct);
    }
}