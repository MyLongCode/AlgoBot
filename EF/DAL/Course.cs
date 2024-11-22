using System.ComponentModel.DataAnnotations;

namespace AlgoBot.EF.DAL
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AdvertisingСampaign> Сampaigns { get; set; }
    }
}
