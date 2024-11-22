using System.ComponentModel.DataAnnotations;

namespace AlgoBot.EF.DAL
{
	public class Firm
	{
		[Key]
		public int Id { get; set; }
		public int OwnerId { get; set; }
		public User Owner { get; set; }
		public string Name { get; set; }
	}
}
