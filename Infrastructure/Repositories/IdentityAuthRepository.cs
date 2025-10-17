using Application.Interfaces.Repository;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public sealed class IdentityAuthRepository : IIdentityAuthRepository
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly RoleManager<IdentityRole<int>> _roles;

        public IdentityAuthRepository(
            UserManager<ApplicationUser> users,
            SignInManager<ApplicationUser> signIn,
            RoleManager<IdentityRole<int>> roles)
        {
            _users = users;
            _signIn = signIn;
            _roles = roles;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken ct = default)
            => await _users.FindByEmailAsync(email);

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken ct = default)
            => (await _signIn.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true)).Succeeded;

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken ct = default)
        {
            var roles = await _users.GetRolesAsync(user);
            return roles ?? new List<string>(); // aldrig null
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
            => await _users.FindByEmailAsync(email) is not null;

        public async Task<(bool ok, IEnumerable<string> errors)> CreateAsync(ApplicationUser user, string password, CancellationToken ct = default)
        {
            var res = await _users.CreateAsync(user, password);
            return res.Succeeded
                ? (true, Array.Empty<string>())
                : (false, res.Errors.Select(e => e.Description));
        }

        public async Task EnsureRoleExistsAsync(string roleName, CancellationToken ct = default)
        {
            if (!await _roles.RoleExistsAsync(roleName))
                await _roles.CreateAsync(new IdentityRole<int>(roleName));
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken ct = default)
            => await _users.AddToRoleAsync(user, roleName);
    }
}
