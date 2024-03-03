using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.web.Models;
using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.Monedas;
using netcore.fcimiddleware.fondos.web.Services.Monedas;
using System.Diagnostics;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Controllers
{
    public class MonedaController : Controller
    {
        private readonly ILogger<MonedaController> _logger;
        private readonly IMonedaProxy _proxy;

        public MonedaController(
            ILogger<MonedaController> logger,
            IMonedaProxy proxy
            )
        {
            _logger = logger;
            _proxy = proxy;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search = "", string? sort = "", int pageIndex = 1, int pageSize = 10)
        {           

            ViewData["search"] = search;
            ViewData["sort"] = sort;
            ViewData["pageIndex"] = pageIndex;
            var result = await _proxy.Pagination(new PaginationQueryRequest { PageIndex = pageIndex, PageSize = pageSize, Search = search, Sort = sort });

            if (result.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<PaginationQueryResponse<Moneda>>(
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Moneda request)
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
            Moneda model = new Moneda();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new Moneda { Id = id };
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
        public async Task<IActionResult> Delete(Moneda request)
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var request = new Moneda { Id = id };
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
        public async Task<IActionResult> Edit(Moneda request)
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

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var request = new Moneda { Id = id };
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<Moneda> getById(HttpResponseMessage result)
        {
            var data = JsonSerializer.Deserialize<Moneda>(
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
    }
}
