using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IIdentityAuthRepository
    {
        // Login
        Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken ct = default);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken ct = default);
        Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken ct = default);

        // Register
        Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
        Task<(bool ok, IEnumerable<string> errors)> CreateAsync(ApplicationUser user, string password, CancellationToken ct = default);
        Task EnsureRoleExistsAsync(string roleName, CancellationToken ct = default);
        Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken ct = default);
    }
}
