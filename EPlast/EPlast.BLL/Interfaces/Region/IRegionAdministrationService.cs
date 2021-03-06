﻿using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Region;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Region
{
    public interface IRegionAdministrationService
    {
        Task AddRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO);

        Task EditRegionAdministrator(RegionAdministrationDTO regionAdministrationDTO);

        Task DeleteAdminByIdAsync(int Id);

        Task<IEnumerable<RegionAdministrationDTO>> GetUsersAdministrations(string userId);

        Task<IEnumerable<RegionAdministrationDTO>> GetUsersPreviousAdministrations(string userId);

        Task<RegionAdministrationDTO> GetHead(int regionId);

        Task<int> GetAdminType(string name);

        Task<IEnumerable<RegionAdministrationDTO>> GetAdministrationAsync(int regionId);

        Task<IEnumerable<AdminTypeDTO>> GetAllAdminTypes();
    }
}
