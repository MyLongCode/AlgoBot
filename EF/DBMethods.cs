﻿using Algo96.EF;
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
            if (user == null)
            {
                await CreateUser(username);
                return 0;
            }
            return user.StageReg;
        }

        public async Task CreateUser(string username)
        {
            var user = new BotUser
            {
                Firstname = "",
                Username = username,
                PhoneNumber = "",
                ChildAge = "",
                ChildName = "",
                StageReg = 0
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserFirstName(string username, string firstName)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            user.Firstname = firstName;
            user.StageReg = 2;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserPhoneNumber(string username, string phonenumber)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user  => user.Username == username);
            user.PhoneNumber = phonenumber;
            user.StageReg = 2;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserChildAge(string username, string childAge)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            user.ChildAge = childAge;
            user.StageReg = 3;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddUserChildName(string username, string childName)
        {
            var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == username);
            user.ChildName = childName;
            user.StageReg = 4;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
