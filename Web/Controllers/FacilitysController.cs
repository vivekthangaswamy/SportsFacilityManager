using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Data.Models;
using Web.Models.ViewModels;
using Web.Models.Facilitys;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Services.Data;
using Web.Common;
using Data.Enums;
using Services.Common;
using Microsoft.AspNetCore.Http;
using System.IO;
using Services.External;

namespace Web.Controllers
{
    public class FacilitysController : Controller
    {
        private readonly IFacilityservice Facilityservice;
        private readonly IMemoryCache memoryCache;
        private readonly ISettingService settingService;
        private readonly IImageManager imageManager;

        public FacilitysController(IFacilityservice _Facilityservice,
                               IMemoryCache memoryCache,
                               ISettingService settingService,
                               IImageManager imageManager)
        {
            Facilityservice = _Facilityservice;
            this.memoryCache = memoryCache;
            this.settingService = settingService;
            this.imageManager = imageManager;
        }
        public async Task<IActionResult> Index(int id = 1, 
                                               int pageSize = 10, 
                                               bool availableOnly = false, 
                                               FacilityType[] type = null,
                                               int minCapacity = 0)
        {
            var searchResults = await Facilityservice.GetSearchResults<FacilityViewModel>(availableOnly, type, minCapacity);
            var resultsCount = searchResults.Count();
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            var pages = (int)Math.Ceiling((double)resultsCount / pageSize);
            if (id <= 0 || id > pages)
            {
                id = 1;
            }

            var model = new FacilityIndexViewModel
            {
                PagesCount = pages,
                CurrentPage = id,
                Facilitys = searchResults.GetPageItems(id, pageSize),
                Controller = "Facilitys",
                Action = nameof(Index),
                BreakfastPrice = await memoryCache.GetBreakfastPrice(settingService),
                AllInclusivePrice = await memoryCache.GetAllInclusivePrice(settingService),
                MaxCapacity = await Facilityservice.GetMaxCapacity(),
                AvailableOnly = availableOnly,
                MinCapacity = minCapacity,
                Types = type,
            };

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        public async Task<IActionResult> Details(string id)
        {
            var Facility = await Facilityservice.GetFacility<FacilityViewModel>(id);
            if (Facility != null)
            {
                return this.View(Facility);
            }
            return this.NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(FacilityInputModel createModel)
        {

            if (!await Facilityservice.IsFacilityNumberFree(createModel.Number))
            {
                ModelState.AddModelError(nameof(createModel.Number), "Facility with this number alreay exists");
            }

            if (createModel.UseSamePhoto)
            {
                ModelState.AddModelError("Error", "Error parsing your request");
            }
            else if (createModel.PhotoUpload != null)
            {
                var timestamp = $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}";
                var fileName = $"_{timestamp}_SportsFacility_FacilityPhoto";

                IFormFile file = createModel.PhotoUpload;

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var photoUrl = await imageManager.UploadImageAsync(stream, fileName);

                if (string.IsNullOrWhiteSpace(photoUrl) ||  photoUrl.StartsWith("Error"))
                {
                    ModelState.AddModelError(nameof(createModel.PhotoUpload), $"An error occured: {photoUrl}.");
                    return this.View(createModel);
                }

                var Facility = new Facility
                {
                    Capacity = createModel.Capacity,
                    AdultPrice = createModel.AdultPrice,
                    ChildrenPrice = createModel.ChildrenPrice,
                    Type = createModel.Type,
                    Number = createModel.Number,
                    ImageUrl = photoUrl,
                };

                await Facilityservice.AddFacility(Facility);

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(nameof(createModel.PhotoUpload), "Image is required. Upload one.");
            return this.View(createModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var Facility = await Facilityservice.GetFacility<FacilityViewModel>(id);
            if (Facility != null)
            {
                await Facilityservice.DeleteFacility(id);
                return this.RedirectToAction("Index", "Facilitys");
            }
            return this.NotFound();
        }

        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Facility = await Facilityservice.GetFacility<FacilityInputModel>(id);
            if (Facility == null)
            {
                return NotFound();
            }

            return this.View(Facility);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(string id, FacilityInputModel input)
        {

            var uFacility = await Facilityservice.GetFacility<FacilityInputModel>(id);
            if (uFacility == null)
            {
                return this.NotFound();
            }

            if (!await Facilityservice.IsFacilityNumberFree(input.Number, id))
            {
                ModelState.AddModelError(nameof(input.Number), "Number with same Id already exists");
            }

            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            string photoUrl = string.Empty;
           
            if (input.UseSamePhoto)
            {
                photoUrl = (await Facilityservice.GetFacility<FacilityViewModel>(id)).ImageUrl;
            }
            else if (input.PhotoUpload != null)
            {
                var timestamp = $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}";
                var fileName = $"_{timestamp}_SportsFacility_FacilityPhoto";

                IFormFile file = input.PhotoUpload;

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                photoUrl = await imageManager.UploadImageAsync(stream, fileName);

                if (string.IsNullOrWhiteSpace(photoUrl)|| photoUrl.StartsWith("Error"))
                {
                    ModelState.AddModelError(nameof(input.PhotoUpload), $"An error occured: {photoUrl}.");
                    return this.View(input);
                }
            }
            else
            {
                return this.View(input);
            }

            var Facility = new Facility
            {
                Capacity = input.Capacity,
                AdultPrice = input.AdultPrice,
                ChildrenPrice = input.ChildrenPrice,
                Type = input.Type,
                Number = input.Number,
                ImageUrl = photoUrl,
            };

            await Facilityservice.UpdateFacility(id,Facility);

            return RedirectToAction(nameof(Index));
        }
    }
}
