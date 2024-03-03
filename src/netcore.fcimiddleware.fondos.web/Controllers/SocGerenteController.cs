using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.SocDepositarias;
using netcore.fcimiddleware.fondos.web.Models;
using netcore.fcimiddleware.fondos.web.Services.SocGerentes;
using System.Diagnostics;
using System.Text.Json;
using netcore.fcimiddleware.fondos.web.Models.V1.SocGerentes;

namespace netcore.fcimiddleware.fondos.web.Controllers
{
    public class SocGerenteController : Controller
    {
        private readonly ILogger<SocGerenteController> _logger;
        private readonly ISocGerenteProxy _proxy;

        public SocGerenteController(
            ILogger<SocGerenteController> logger,
            ISocGerenteProxy proxy)
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
                var data = JsonSerializer.Deserialize<PaginationQueryResponse<SocGerente>>(
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
        #endregion

        #region "Add"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SocGerente request)
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
            SocGerente model = new SocGerente();
            return View(model);
        }
        #endregion

        #region "Delete"
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new SocGerente { Id = id };
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
        public async Task<IActionResult> Delete(SocGerente request)
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
            var request = new SocGerente { Id = id };
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
        public async Task<IActionResult> Edit(SocGerente request)
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
        public async Task<IActionResult> Detail(SocGerente request)
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
        private async Task<SocGerente> getById(HttpResponseMessage result)
        {
            var data = JsonSerializer.Deserialize<SocGerente>(
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
