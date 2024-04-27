using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using netcore.fcimiddleware.fondos.web.Models;
using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.Fondos;
using netcore.fcimiddleware.fondos.web.Models.V1.Monedas;
using netcore.fcimiddleware.fondos.web.Services.Fondos;
using netcore.fcimiddleware.fondos.web.Services.Monedas;
using System.Diagnostics;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Controllers
{
    public class FondoController : Controller
    {
        private readonly ILogger<FondoController> _logger;
        private readonly IFondoProxy _proxy;
        private readonly IMonedaProxy _monedaProxy;

        public FondoController(
            ILogger<FondoController> logger,
            IFondoProxy proxy,
            IMonedaProxy monedaProxy)
        {
            _logger = logger;
            _proxy = proxy;
            _monedaProxy = monedaProxy;
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
                var data = JsonSerializer.Deserialize<PaginationQueryResponse<Fondo>>(
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
        public async Task<IActionResult> Add(FondoCreateRequest request)
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
        public async Task<IActionResult> Add()
        {
            FondoCreateRequest model = new FondoCreateRequest();
            return View(model);
        }
        #endregion

        #region "Delete"
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new Fondo { Id = id };
            var result = await _proxy.GetById(request);

            if (result.IsSuccessStatusCode)
            {
                var data = await getById(result);

                return View(data);
            }
            var badRequest = await getBadRequest(result);

            ModelState.AddModelError(string.Empty, badRequest.Message);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Fondo request)
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
            var request = new Fondo { Id = id };
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
        public async Task<IActionResult> Edit(Fondo request)
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

        #region "View"
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var request = new Fondo { Id = id };
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
        private async Task<Fondo> getById(HttpResponseMessage result)
        {
            var data = JsonSerializer.Deserialize<Fondo>(
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
