﻿using System.ComponentModel.DataAnnotations;

namespace AlgoBot.EF.DAL
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public int CampaignId { get; set; }
    }
}