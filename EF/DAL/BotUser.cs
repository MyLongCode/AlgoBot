using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoBot.EF.DAL
{
    public class BotUser
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Username{ get; set; }
        public string PhoneNumber{ get; set; } = null!;
        public string ChildAge{ get; set; } = null!;
        public string ChildName { get; set; } = null!;
    }
}
