using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.web.Models;
using netcore.fcimiddleware.fondos.web.Models.V1.Moneda;
using netcore.fcimiddleware.fondos.web.Models.V1.Shared;
using netcore.fcimiddleware.fondos.web.Services.Moneda;
using System.Diagnostics;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Controllers
{
    public class MonedaController : Controller
    {
        private readonly ILogger<MonedaController> _logger;
        private readonly IMonedaProxy _monedaProxy;

        public MonedaController(
            ILogger<MonedaController> logger,
            IMonedaProxy MonedaProxy
            )
        {
            _logger = logger;
            _monedaProxy = MonedaProxy;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search = "", string? sort = "", int pageIndex = 1, int pageSize = 10)
        {           

            ViewData["search"] = search;
            ViewData["sort"] = sort;
            ViewData["pageIndex"] = pageIndex;
            var result = await _monedaProxy.PaginationMoneda(new PaginationQueryRequest { PageIndex = pageIndex, PageSize = pageSize, Search = search, Sort = sort });

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return RedirectToAction("Index", "Home");
            }

            var data = JsonSerializer.Deserialize<PaginationQueryResponse<GetByIdMonedaResponse>>(
                    await result.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return View(data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateMonedaRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _monedaProxy.CreateMoneda(request);

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var badRequest = await getBadRequest(result);

                    ModelState.AddModelError(string.Empty, badRequest.Message);
                    return View(request);
                }

                var dateResponse = JsonSerializer.Deserialize<int>(
                        await result.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );

                return RedirectToAction(nameof(Index));
            }
            
            return View(request);
        }

        [HttpGet]
        public IActionResult Add()
        {
            CreateMonedaRequest model = new CreateMonedaRequest();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _monedaProxy.GetByIdMoneda(new GetByIdMonedaRequest { Id = id });

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return RedirectToAction("Index", "Home");
            }

            var data = await getById(result);

            var model = new DeleteMonedaRequest
            {
                Id = data.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteMonedaRequest request)
        {            
            var result = await _monedaProxy.DeleteMoneda(request);

            if (result.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return View(request);
            }


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _monedaProxy.GetByIdMoneda(new GetByIdMonedaRequest { Id = id});

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return View();
            }

            var data = await getById(result);

            var updateRequest = new UpdateMonedaRequest
            {
                Id = data.Id,
                Descripcion = data.Descripcion,
                IdCAFCI = data.IdCAFCI
            };

            return View(updateRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMonedaRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _monedaProxy.UpdateMoneda(request);
                if ((result.StatusCode != System.Net.HttpStatusCode.OK) && (result.StatusCode != System.Net.HttpStatusCode.NoContent))
                {
                    var badRequest = await getBadRequest(result);

                    ModelState.AddModelError(string.Empty, badRequest.Message);
                    return View(request);
                }

                return RedirectToAction(nameof(Index));

            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _monedaProxy.GetByIdMoneda(new GetByIdMonedaRequest { Id = id });

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return View();
            }

            var data = await getById(result);

            return View(data);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<GetByIdMonedaResponse> getById(HttpResponseMessage result)
        {
            var data = JsonSerializer.Deserialize<GetByIdMonedaResponse>(
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
