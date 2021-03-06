﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class UserPlastDegree
    {
        public int Id { get; set; }
        [Required]
        public int PlastDegreeId { get; set; }
        public PlastDegree PlastDegree { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public bool IsCurrent { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
