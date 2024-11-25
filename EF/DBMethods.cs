using AlgoBot.EF;
using AlgoBot.EF.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlgoBot.EF
{
    public class DBMethods
    {
        private readonly BotDbContext _db;

        public DBMethods(BotDbContext db) => _db = db;

        public async Task EditStageReg(string username, int stage)
        {
            var user = await GetUser(username);
            user.StageReg = stage;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<User> GetUser(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Login == username);
            return user;
        }

        public async Task<User> GetUserAsNoTracking(string username)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Login == username);
            return user;
        }

        public async Task<int> GetUserStageReg(string username)
        {
            var user = await GetUser(username);
            return user.StageReg;
        }

        public async Task<IEnumerable<User>> GetReferals(string username)
        {
            var users = await _db.Users.Where(user => user.ReferalUsername == username).ToListAsync();
            return users;
        }

        public async Task CreateUser(string username, string referal)
        {
            var botUser = new User
            {
                FullName = "",
                Role = "botuser",
                Login = username,
                Password = new Random().Next(100000, 999999).ToString(),
                PhoneNumber = "",
                ChildAge = "",
                ChildName = "",
                ReferalUsername = referal,
                StageReg = 0,
                Score = 0,
                Cashback = 0,
            };
            _db.Users.Add(botUser);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserFirstName(string username, string firstName)
        {
            var user = await GetUser(username);
            user.FullName = firstName;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserPhoneNumber(string username, string phonenumber)
        {
            var user = await GetUser(username);
            var phoneNumber = new StringBuilder(phonenumber);
            phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace(" ", "").Replace("+", "");
            if (phoneNumber[0] == '8') phoneNumber[0] = '7';
            user.PhoneNumber = phoneNumber.ToString();
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserChildAge(string username, string childAge)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Login == username);
            user.ChildAge = childAge;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserChildName(string username, string childName)
        {
            var user = await GetUser(username);
            user.ChildName = childName;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<int> GetCashback(string username)
        {
            var user = await GetUserAsNoTracking(username);
            return user.Cashback;
        }
    }
}
