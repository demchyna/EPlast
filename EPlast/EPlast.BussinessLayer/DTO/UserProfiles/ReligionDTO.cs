﻿using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO.UserProfiles
{
    public class ReligionDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserProfileDTO> UserProfiles { get; set; }
    }
}