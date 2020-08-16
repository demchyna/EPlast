﻿using EPlast.BLL.DTO.UserProfiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.EducatorsStaff
{
    public class KadrasDTO
    {
        public int ID { get; set; }

        public UserDTO User { get; set; }

        public KVTypeDTO KVType { get; set; }

        public DateTime DateOfGranting { get; set; }

        public int NumberInRegister { get; set; }

        public string BasisOfGranting { get; set; }

        public string Link { get; set; }
    }
}
