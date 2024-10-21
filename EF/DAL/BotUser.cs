using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoBot.EF.DAL
{
    public class BotUser
    {
        [Key]
        public string Username { get; set; }
        public string? ReferalUsername { get; set; }
        public string Firstname { get; set; } = null!;
        public string PhoneNumber{ get; set; } = null!;
        public string ChildAge{ get; set; } = null!;
        public string ChildName { get; set; } = null!;
        public int StageReg { get; set; }
        public int Score { get; set; }
    }
}
