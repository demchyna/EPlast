﻿using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.DTO.UserProfiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IKVService
    {
        Task<IEnumerable<KadrasDTO>> GetAllKVsAsync();

        Task<IEnumerable<KadrasDTO>> GetKVsWithKVType(KVTypeDTO kvTypeDTO);

        Task<IEnumerable<KadrasDTO>> GetKVsOfGivenUser(UserDTO userDTO);

        Task<KadrasDTO> CreateKadra(KadrasDTO kadrasDTO);

        Task<KadrasDTO> GetKadraById(int KadraID);

        Task<KadrasDTO> GetKadraByRegisterNumber(int KadrasRegisterNumber);

        Task<KadrasDTO> UpdateKadra(int KadraID, KadrasDTO kadrasDTO);


    }
}
