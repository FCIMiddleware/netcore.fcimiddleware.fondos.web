using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.web.Models;
using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.SocDepositarias;
using netcore.fcimiddleware.fondos.web.Services.SocDepositarias;
using System.Diagnostics;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Controllers
{
    public class SocDepositariaController : Controller
    {
        private readonly ILogger<SocDepositariaController> _logger;
        private readonly ISocDepositariaProxy _proxy;

        public SocDepositariaController(
            ILogger<SocDepositariaController> logger,
            ISocDepositariaProxy proxy)
        {
            _logger = logger;
            _proxy = proxy;
        }


        #region "Pagination"
        [HttpGet]
        public async Task<IActionResult> Index(string? search = "", string? sort = "", int pageIndex = 1, int pageSize = 10)
        {

            ViewData["search"] = search;
            ViewData["sort"] = sort;
            ViewData["pageIndex"] = pageIndex;
            var result = await _proxy.Pagination(new PaginationQueryRequest { PageIndex = pageIndex, PageSize = pageSize, Search = search, Sort = sort });

            if (result.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<PaginationQueryResponse<SocDepositaria>>(
                    await result.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
                return View(data);                
            }

            var badRequest = await getBadRequest(result);
            ModelState.AddModelError(string.Empty, badRequest.Message);
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<JsonResult> List(string? searchSocDepositaria = "")
        {
            //Fondo model = new Fondo();
            ViewData["searchSocDepositaria"] = searchSocDepositaria;
            var result = await _proxy.List(new PaginationQueryRequest { PageIndex = 1, PageSize = 10, Search = searchSocDepositaria, Sort = "descripcionAsc" });
            var data = JsonSerializer.Deserialize<PaginationQueryResponse<SocDepositariaList>>(
                    await result.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return Json(data);
        }
        #endregion

        #region "Add"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SocDepositaria request)
        {
            if (ModelState.IsValid)
            {
                var result = await _proxy.Create(request);
                if (result.IsSuccessStatusCode)
                {
                    var dateResponse = JsonSerializer.Deserialize<int>(
                        await result.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );
                    return RedirectToAction(nameof(Index));                    
                }

                var badRequest = await getBadRequest(result);
                ModelState.AddModelError(string.Empty, badRequest.Message);
                return View(request);
            }

            return View(request);
        }

        [HttpGet]
        public IActionResult Add()
        {
            SocDepositaria model = new SocDepositaria();
            return View(model);
        }
        #endregion

        #region "Delete"
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new SocDepositaria { Id = id };
            var result = await _proxy.GetById(request);

            if (result.IsSuccessStatusCode)
            {
                var data = await getById(result);

                return View(request);                
            }

            var badRequest = await getBadRequest(result);

            ModelState.AddModelError(string.Empty, badRequest.Message);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SocDepositaria request)
        {
            var result = await _proxy.Delete(request);

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));                
            }

            var badRequest = await getBadRequest(result);

            ModelState.AddModelError(string.Empty, badRequest.Message);
            return View(request);
        }
        #endregion

        #region "Edit"
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var request = new SocDepositaria { Id = id };
            var result = await _proxy.GetById(request);

            if (result.IsSuccessStatusCode)
            {
                var data = await getById(result);
                return View(data);                
            }

            var badRequest = await getBadRequest(result);
            ModelState.AddModelError(string.Empty, badRequest.Message);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SocDepositaria request)
        {
            if (ModelState.IsValid)
            {
                var result = await _proxy.Update(request);
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));                    
                }

                var badRequest = await getBadRequest(result);
                ModelState.AddModelError(string.Empty, badRequest.Message);
                return View(request);
            }
            return View(request);
        }
        #endregion

        #region "Detail"
        [HttpGet]
        public async Task<IActionResult> Detail(SocDepositaria request)
        {
            var result = await _proxy.GetById(request);
            if (result.IsSuccessStatusCode)
            {
                var data = await getById(result);
                return View(data);                
            }

            var badRequest = await getBadRequest(result);
            ModelState.AddModelError(string.Empty, badRequest.Message);
            return View();
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region "Private Method"
        private async Task<SocDepositaria> getById(HttpResponseMessage result)
        {
            var data = JsonSerializer.Deserialize<SocDepositaria>(
                    await result.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return data;
        }

        private async Task<BadRequest> getBadRequest(HttpResponseMessage result)
        {
            var badRequest = JsonSerializer.Deserialize<BadRequest>(
                        await result.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );

            return badRequest;
        }
        #endregion

    }
}
