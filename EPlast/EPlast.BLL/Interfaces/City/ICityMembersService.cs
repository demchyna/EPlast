﻿using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.City
{
    public interface ICityMembersService
    {
        Task<IEnumerable<CityMembersDTO>> GetMembersByCityIdAsync(int cityId);
        Task<CityMembersDTO> AddFollowerAsync(int cityId, string userId);
        Task<CityMembersDTO> ToggleApproveStatusAsync(string userId);
        Task RemoveMemberAsync(string userId);
    }
}