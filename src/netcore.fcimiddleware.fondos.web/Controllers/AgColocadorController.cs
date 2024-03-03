using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.web.Models;
using netcore.fcimiddleware.fondos.web.Models.Shared;
using netcore.fcimiddleware.fondos.web.Models.V1.AgColocadores;
using netcore.fcimiddleware.fondos.web.Models.V1.SocDepositarias;
using netcore.fcimiddleware.fondos.web.Services.AgColocadores;
using System.Diagnostics;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Controllers
{
    public class AgColocadorController : Controller
    {
        private readonly ILogger<AgColocadorController> _logger;
        private readonly IAgColocadorProxy _proxy;

        public AgColocadorController(
            ILogger<AgColocadorController> logger,
            IAgColocadorProxy proxy)
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
                var data = JsonSerializer.Deserialize<PaginationQueryResponse<AgColocador>>(
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
        public async Task<IActionResult> Add(AgColocador request)
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
            AgColocador model = new AgColocador();
            return View(model);
        }
        #endregion

        #region "Delete"
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new AgColocador { Id = id };
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
        public async Task<IActionResult> Delete(AgColocador request)
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
            var request = new AgColocador { Id = id };
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
        public async Task<IActionResult> Edit(AgColocador request)
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
        public async Task<IActionResult> Detail(AgColocador request)
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
        private async Task<AgColocador> getById(HttpResponseMessage result)
        {
            var data = JsonSerializer.Deserialize<AgColocador>(
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
