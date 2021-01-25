using BarberAPI.Entities;
using BarberAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarberAPI.Services
{
    public interface IAuthService
    {
        Dictionary<string, Guid> Authenticate(string username, string password);
        Task<Client> RegisterClient(Client user, string password);
        Task<Barber> RegisterBarber(Barber user, string password);
        Task<BarbershopOwner> RegisterBarbershopOwner(BarbershopOwner user, string password);
        Task<Client> GetByGd(Guid gd);
        Task UpdateClient(Client user, string password = null);
        Task DeleteClientAccount(Guid client_gd);
    }

    public class AuthService : IAuthService
    {
        protected DataContext _context;

        public AuthService(DataContext context)
        {
            _context = context;
        }

        /*
         * Checks if user is a barber or client
         * Returns null on failed login
         * Returns Dictionary with type of user & gd on succesfull login
        */
        public Dictionary<string, Guid> Authenticate(string username, string password)
        {
            // Dictionary will hold type of user & gd of user
            Dictionary<string, Guid> typeUser = new Dictionary<string, Guid>();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new AppException("Please enter your username & password.");

            var user = _context.Clients.SingleOrDefault(usr => usr.Username == username);
            var barber = _context.Barbers.SingleOrDefault(barber => barber.Username == username);
            var barbershopOwner = _context.BarbershopOwners.SingleOrDefault(owner => owner.Username == username);

            // Check if username exists
            if (user == null && barber == null && barbershopOwner == null)
                return null;
            else if (user != null && barber == null && barbershopOwner == null)
            {
                // Check if password is correct
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;
                typeUser.Add("Client", user.Gd);
                return typeUser;
            }
            else if (user == null && barber != null && barbershopOwner == null)
            {
                if (!VerifyPasswordHash(password, barber.PasswordHash, barber.PasswordSalt))
                    return null;
                typeUser.Add("Barber", barber.Gd);
                return typeUser;
            }
            else if (user == null && barber == null && barbershopOwner != null)
            {
                if (!VerifyPasswordHash(password, barbershopOwner.PasswordHash, barbershopOwner.PasswordSalt))
                    return null;
                typeUser.Add("Owner", barbershopOwner.Gd);
                return typeUser;
            }
            return null;
        }
        
        public async Task<Client> RegisterClient(Client user, string password)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            // Check if username is taken
            if (_context.Clients.Any(usr => usr.Username == user.Username))
                throw new AppException("Username: \"" + user.Username + "\" is already taken");

            // Check if email is taken
            if (_context.Clients.Any(usr => usr.Email == user.Email))
                throw new AppException("Email: \"" + user.Email + "\" is already taken");

            // Check if phone number is taken
            if (_context.Clients.Any(usr => usr.Phone == user.Phone))
                throw new AppException("Phone number: \"" + user.Phone + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Clients.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<Barber> RegisterBarber(Barber barber, string password)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Barbers.Any(usr => usr.Username == barber.Username))
                throw new AppException("Username \"" + barber.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            barber.PasswordHash = passwordHash;
            barber.PasswordSalt = passwordSalt;

            await _context.Barbers.AddAsync(barber);
            await _context.SaveChangesAsync();

            return barber;
        }

        public async Task<BarbershopOwner> RegisterBarbershopOwner(BarbershopOwner barbershopOwner, string password)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Barbers.Any(usr => usr.Username == barbershopOwner.Username))
                throw new AppException("Username \"" + barbershopOwner.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            barbershopOwner.PasswordHash = passwordHash;
            barbershopOwner.PasswordSalt = passwordSalt;

            await _context.BarbershopOwners.AddAsync(barbershopOwner);
            await _context.SaveChangesAsync();

            return barbershopOwner;
        }

        public async Task DeleteClientAccount(Guid client_gd)
        {
            var user = await _context.Clients.FindAsync(client_gd);
            if (user != null)
            {
                _context.Clients.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Client> GetByGd(Guid gd)
        {
            return await _context.Clients.FindAsync(gd);
        }

        public async Task UpdateClient(Client userParam, string password = null)
        {
            var user = await _context.Clients.FindAsync(userParam.Gd);

            if (user == null)
                throw new AppException("User not found");

            // Update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // Throw error if the new username is already taken
                if (_context.Clients.Any(usr => usr.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");

                user.Username = userParam.Username;
            }

            // Update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.Firstname))
                user.Firstname = userParam.Firstname;

            if (!string.IsNullOrWhiteSpace(userParam.Lastname))
                user.Lastname = userParam.Lastname;

            if (!string.IsNullOrWhiteSpace(userParam.Username))
                user.Username = userParam.Username;

            if (!string.IsNullOrWhiteSpace(userParam.Email))
                user.Email = userParam.Email;

            if (!string.IsNullOrWhiteSpace(userParam.Phone))
                user.Phone = userParam.Phone;

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Clients.Update(user);
            await _context.SaveChangesAsync();
        }

        // Private helper methods
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Validation
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            // Validation:
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");

            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
