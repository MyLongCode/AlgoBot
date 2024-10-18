using Algo96.EF;
using AlgoBot.EF.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoBot.EF
{
    public class DBMethods
    {
        private readonly BotDbContext _db;

        public DBMethods(BotDbContext db) => _db = db;

        public async Task EditStageReg(string username, int stage)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            user.StageReg = stage;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<BotUser> GetUser(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            return user;
        }

        public async Task<int> GetUserStageReg(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            return user.StageReg;
        }

        public async Task<IEnumerable<BotUser>> GetReferals(string username)
        {
            var users = await _db.Users.Where(user => user.ReferalUsername == username).ToListAsync();
            return users;
        }

        public async Task CreateUser(string username, string referal)
        {
            var user = new BotUser
            {
                Firstname = "",
                Username = username,
                PhoneNumber = "",
                ChildAge = "",
                ChildName = "",
                ReferalUsername = referal,
                StageReg = 0
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserFirstName(string username, string firstName)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            user.Firstname = firstName;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserPhoneNumber(string username, string phonenumber)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user  => user.Username == username);
            user.PhoneNumber = phonenumber;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserChildAge(string username, string childAge)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            user.ChildAge = childAge;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserChildName(string username, string childName)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            user.ChildName = childName;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
