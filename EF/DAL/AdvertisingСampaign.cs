﻿using System.ComponentModel.DataAnnotations;

namespace AlgoBot.EF.DAL
{
	public class AdvertisingСampaign
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public int FirmId { get; set; }
		public Firm Firm { get; set; }
		public ReferalSystem ReferalSystem { get; set; }
		public string Distribution { get; set; }
		public int? ProcentScore { get; set; }
		public int? Score { get; set; }
		public List<Course> Courses { get; set; }
	}
}
