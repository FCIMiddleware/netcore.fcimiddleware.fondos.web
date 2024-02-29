using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.web.Models;
using netcore.fcimiddleware.fondos.web.Models.V1.Moneda;
using netcore.fcimiddleware.fondos.web.Models.V1.Pais;
using netcore.fcimiddleware.fondos.web.Models.V1.Shared;
using netcore.fcimiddleware.fondos.web.Services.Pais;
using System.Diagnostics;
using System.Text.Json;

namespace netcore.fcimiddleware.fondos.web.Controllers
{
    public class PaisController : Controller
    {
        private readonly ILogger<PaisController> _logger;
        private readonly IPaisProxy _proxy;

        public PaisController(
            ILogger<PaisController> logger, 
            IPaisProxy proxy )
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

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return RedirectToAction("Index", "Home");
            }

            var data = JsonSerializer.Deserialize<PaginationQueryResponse<GetByIdPaisResponse>>(
                    await result.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return View(data);
        }
        #endregion

        #region "Create"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreatePaisRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _proxy.Create(request);

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
            CreatePaisRequest model = new CreatePaisRequest();
            return View(model);
        }
        #endregion

        #region "Delete"
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _proxy.GetById(new GetByIdPaisRequest { Id = id });

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return RedirectToAction("Index", "Home");
            }

            var data = await getById(result);

            return View(new DeletePaisRequest { Id = data.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeletePaisRequest request)
        {
            var result = await _proxy.Delete(request);

            if (result.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region "Update"
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _proxy.GetById(new GetByIdPaisRequest { Id = id });

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return View();
            }

            var data = await getById(result);

            var updateRequest = new UpdatePaisRequest
            {
                Id = data.Id,
                Descripcion = data.Descripcion,
                IdCAFCI = data.IdCAFCI
            };

            return View(updateRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePaisRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _proxy.Update(request);
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
        #endregion

        #region "View"
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _proxy.GetById(new GetByIdPaisRequest { Id = id });

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var badRequest = await getBadRequest(result);

                ModelState.AddModelError(string.Empty, badRequest.Message);
                return View();
            }

            var data = await getById(result);

            return View(data);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region "Private Method"
        private async Task<GetByIdPaisResponse> getById(HttpResponseMessage result)
        {
            var data = JsonSerializer.Deserialize<GetByIdPaisResponse>(
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
