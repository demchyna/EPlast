﻿using EPlast.BussinessLayer.DTO;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Services.Interfaces;

namespace EPlast.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;
        private readonly IClubAdministrationService _clubAdministrationService;
        private readonly IClubMembersService _clubMembersService;
        private readonly IMapper _mapper;
        private readonly ILoggerService<ClubController> _logger;
        private readonly IUserManagerService _userManagerService;

        public ClubController(IClubService clubService, IClubAdministrationService clubAdministrationService, IClubMembersService clubMembersService, IMapper mapper, ILoggerService<ClubController> logger, IUserManagerService userManagerService)
        {
            _clubService = clubService;
            _clubAdministrationService = clubAdministrationService;
            _clubMembersService = clubMembersService;
            _mapper = mapper;
            _logger = logger;
            _userManagerService = userManagerService;
        }

        public IActionResult Index()
        {
            var clubs = _clubService.GetAllClubs();
            var viewModels = _mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(clubs);

            return View(viewModels);
        }

        public IActionResult Club(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(_clubService.GetClubProfile(index));
                ViewBag.usermanager = _userManagerService;
                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubAdmins(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(_clubAdministrationService.GetCurrentClubAdministrationByID(index));
                ViewBag.usermanager = _userManagerService;
                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubMembers(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(_clubService.GetClubMembersOrFollowers(index, true));
                ViewBag.usermanager = _userManagerService;
                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubFollowers(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(_clubService.GetClubMembersOrFollowers(index, false));
                ViewBag.usermanager = _userManagerService;
                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult ClubDescription(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubDTO, ClubViewModel>(_clubService.GetClubInfoById(index));

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpGet]
        public IActionResult EditClub(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubDTO, ClubViewModel>(_clubService.GetClubInfoById(index));

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult EditClub(ClubViewModel model, IFormFile file)
        {
            try
            {
                _clubService.Update(_mapper.Map<ClubViewModel, ClubDTO>(model), file);

                return RedirectToAction("Club", new { index = model.ID });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatus(int index, int clubIndex)
        {
            try
            {
                _clubMembersService.ToggleIsApprovedInClubMembers(index, clubIndex);

                return RedirectToAction("ClubMembers", new { index = clubIndex });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatusFollowers(int index, int clubIndex)
        {
            try
            {
                _clubMembersService.ToggleIsApprovedInClubMembers(index, clubIndex);

                return RedirectToAction("ClubFollowers", new { index = clubIndex });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatusClub(int index, int clubIndex)
        {
            try
            {
                _clubMembersService.ToggleIsApprovedInClubMembers(index, clubIndex);

                return RedirectToAction("Club", new { index = clubIndex });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult DeleteFromAdmins(int adminId, int clubIndex)
        {
            bool isSuccessfull = _clubAdministrationService.DeleteClubAdmin(adminId);

            if (isSuccessfull)
            {
                return RedirectToAction("ClubAdmins", new { index = clubIndex });
            }
            else
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public int AddEndDate([FromBody] AdminEndDateDTO adminEndDate)
        {
            try
            {
                _clubAdministrationService.SetAdminEndDate(adminEndDate);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        [HttpPost]
        public IActionResult AddToClubAdministration([FromBody] ClubAdministrationDTO createdAdmin)
        {
            try
            {
                _clubAdministrationService.AddClubAdmin(createdAdmin);

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        public IActionResult ChooseAClub()
        {
            var clubs = _mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(_clubService.GetAllClubs());

            ViewBag.usermanager = _userManagerService;

            return View(clubs);
        }

        public IActionResult AddAsClubFollower(int clubIndex)
        {
            var userId = _userManagerService.GetUserId(User);

            _clubMembersService.AddFollower(clubIndex, userId);

            return RedirectToAction("UserProfile", "Account", new { userId });
        }

        [HttpGet]
        public IActionResult CreateClub()
        {
            try
            {
                return View(new ClubViewModel());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult CreateClub(ClubViewModel model, IFormFile file)
        {
            try
            {
                var club = _clubService.Create(_mapper.Map<ClubViewModel, ClubDTO>(model), file);

                return RedirectToAction("Club", new { index = club.ID });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}