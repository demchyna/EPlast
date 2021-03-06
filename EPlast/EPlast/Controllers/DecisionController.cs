﻿using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.Models;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Logging;
using Organization = EPlast.Models.Organization;

namespace EPlast.Controllers
{
    public class DecisionController : Controller
    {
        private readonly IDecisionService _decisionService;
        private readonly IMapper _mapper;
        private readonly IPdfService _PDFService;
        private readonly ILoggerService<DecisionController> _loggerService;

        public DecisionController(IPdfService PDFService, IDecisionService decisionService,
            IMapper mapper, ILoggerService<DecisionController> loggerService)
        {
            _PDFService = PDFService;
            _decisionService = decisionService;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<DecisionViewModel> CreateDecision()
        {
            DecisionViewModel decisionViewModel = null;
            try
            {
                var organizations = _mapper.Map<IEnumerable<Organization>>(await _decisionService.GetOrganizationListAsync());
                decisionViewModel = new DecisionViewModel
                {
                    DecisionWrapper = _mapper.Map<DecisionWrapper>(_decisionService.CreateDecision()),
                    OrganizationListItems = from item in organizations
                                            select new SelectListItem
                                            {
                                                Text = item.OrganizationName,
                                                Value = item.ID.ToString()
                                            },
                    DecisionTargets = _mapper.Map<IEnumerable<DecisionTarget>>(await _decisionService.GetDecisionTargetListAsync()),
                    DecisionStatusTypeListItems = _decisionService.GetDecisionStatusTypes()
                };
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");
            }

            return decisionViewModel;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<JsonResult> GetDecision(int id)
        {
            bool success = true;
            Decision decision = null;
            try
            {
                decision = _mapper.Map<Decision>(await _decisionService.GetDecisionAsync(id));
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");
                success = false;
            }

            return Json(new { success, decision });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> ChangeDecision(Decision decision)
        {
            var success = false;
            try
            {
                 await _decisionService.ChangeDecisionAsync(
                    _mapper.Map<DecisionDTO>(decision));
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");
            }

            return Json(new
            {
                success,
                text = "Зміни пройшли успішно!",
                decision
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> SaveDecision(DecisionWrapper decisionWrapper)
        {
            try
            {
                ModelState.Remove("DecisionWrapper.Decision.DecisionStatusType");
                if (!ModelState.IsValid && decisionWrapper.Decision.DecisionTarget.ID != 0 ||
                    decisionWrapper == null)
                {
                    ModelState.AddModelError("", "Дані введені неправильно");

                    return Json(new
                    {
                        success = false,
                        text = ModelState.Values.SelectMany(v => v.Errors),
                        model = decisionWrapper,
                        modelstate = ModelState
                    });
                }

                if (decisionWrapper.File != null &&
                    decisionWrapper.File.Length > 10485760)
                {
                    ModelState.AddModelError("", "файл за великий (більше 10 Мб)");

                    return Json(new { success = false, text = "file length > 10485760" });
                }

                decisionWrapper.Decision.HaveFile = decisionWrapper.File != null;
                decisionWrapper.Decision.ID = await _decisionService.SaveDecisionAsync(
                    _mapper.Map<DecisionWrapperDTO>(decisionWrapper));

                return Json(new
                {
                    success = true,
                    Text = "Рішення додано!",
                    decision = decisionWrapper.Decision,
                    decisionOrganization = (await _decisionService
                        .GetDecisionOrganizationAsync(_mapper.Map<OrganizationDTO>(decisionWrapper.Decision.Organization)))
                        .OrganizationName
                });
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");

                return Json(new
                {
                    success = false,
                    text = e.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReadDecision()
        {
            List<DecisionViewModel> decisions = null;
            try
            {
                decisions = new List<DecisionViewModel>
                (
                    _mapper.Map<IEnumerable<DecisionWrapper>>(await _decisionService.GetDecisionListAsync())
                        .Select(decesion => new DecisionViewModel { DecisionWrapper = decesion })
                        .ToList()
                );
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");
            }

            return View(Tuple.Create(await CreateDecision(), decisions));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> DeleteDecision(int id)
        {
            await _decisionService.DeleteDecisionAsync(id);
            return Json(new
            {
                success = true,
                text = "Зміни пройшли успішно!"
            });
        }
        /* 
              [Authorize(Roles = "Admin")]
              public async Task<IActionResult> Download(int id, string filename)
              {
                  /*  byte[] fileBytes;
                    try
                    {
                        DecisionIdVerify(id);
                        fileBytes = await _decisionService.DownloadDecisionFileAsync(id);
                    }
                    catch (Exception e)
                    {
                        _loggerService.LogError($"{e.Message}");

                        return RedirectToAction("HandleError", "Error");
                    }

                    return File(fileBytes, _decisionService.GetContentType(id, filename), filename);

                }
                   */
        private static void DecisionIdVerify(int id)
        {
            if (id <= 0) throw new ArgumentException("Decision id cannot be null lest than zero");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetPdf(int objId)
        {
            try
            {
                DecisionIdVerify(objId);
                var arr = await _PDFService.DecisionCreatePDFAsync(objId);

                return File(arr, "application/pdf");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");

                return RedirectToAction("HandleError", "Error");
            }
        }
    }
}