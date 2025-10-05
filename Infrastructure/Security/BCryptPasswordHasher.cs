using Application.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 12;

        public string Hash(string password) =>
            BCryptNet.HashPassword(password, workFactor: WorkFactor);

        public bool Verify(string password, string passwordHash) =>
            BCryptNet.Verify(password, passwordHash);
    }
}