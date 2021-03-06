﻿using Camino.Shared.Requests.Filters;
using Camino.Core.Constants;
using Camino.Framework.Attributes;
using Camino.Framework.Controllers;
using Camino.Core.Contracts.Helpers;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Web.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Results.Products;
using Camino.Shared.Enums;
using Camino.Shared.Requests.Products;

namespace Module.Web.ProductManagement.Controllers
{
    public class ProductCategoryController : BaseAuthController
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IHttpHelper _httpHelper;

        public ProductCategoryController(IProductCategoryService productCategoryService,
            IHttpContextAccessor httpContextAccessor, IHttpHelper httpHelper)
            : base(httpContextAccessor)
        {
            _httpHelper = httpHelper;
            _productCategoryService = productCategoryService;
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductCategory)]
        [LoadResultAuthorizations("ProductCategory", PolicyMethod.CanCreate, PolicyMethod.CanUpdate, PolicyMethod.CanDelete)]
        public async Task<IActionResult> Index(ProductCategoryFilterModel filter)
        {
            var filterRequest = new ProductCategoryFilter()
            {
                CreatedById = filter.CreatedById,
                CreatedDateFrom = filter.CreatedDateFrom,
                CreatedDateTo = filter.CreatedDateTo,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Search = filter.Search,
                UpdatedById = filter.UpdatedById
            };

            var categoryPageList = await _productCategoryService.GetAsync(filterRequest);
            var categories = categoryPageList.Collections.Select(x => new ProductCategoryModel()
            {
                Id = x.Id,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                CreatedById = x.CreatedById,
                CreatedDate = x.CreatedDate,
                Name = x.Name
            });

            var categoryPage = new PageListModel<ProductCategoryModel>(categories)
            {
                Filter = filter,
                TotalPage = categoryPageList.TotalPage,
                TotalResult = categoryPageList.TotalResult
            };

            if (_httpHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ProductCategoryTable", categoryPage);
            }

            return View(categoryPage);
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductCategory)]
        [LoadResultAuthorizations("ProductCategory", PolicyMethod.CanUpdate)]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToNotFoundPage();
            }

            try
            {
                var category = await _productCategoryService.FindAsync(id);
                if (category == null)
                {
                    return RedirectToNotFoundPage();
                }

                var model = new ProductCategoryModel()
                {
                    Id = category.Id,
                    Description = category.Description,
                    CreatedBy = category.CreatedBy,
                    CreatedById = category.CreatedById,
                    CreatedDate = category.CreatedDate,
                    Name = category.Name,
                    UpdateById = category.UpdatedById,
                    ParentId = category.ParentId,
                    ParentCategoryName = category.ParentCategoryName,
                    UpdatedBy = category.UpdatedBy,
                    UpdatedDate = category.UpdatedDate
                };
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToErrorPage();
            }
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateProductCategory)]
        public IActionResult Create()
        {
            var model = new ProductCategoryModel();
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanCreateProductCategory)]
        public async Task<IActionResult> Create(ProductCategoryModel model)
        {
            var category = new ProductCategoryRequest()
            {
                Description = model.Description,
                Name = model.Name,
                ParentId = model.ParentId
            };

            var exist = _productCategoryService.FindByName(model.Name);
            if (exist != null)
            {
                return RedirectToErrorPage();
            }

            category.UpdatedById = LoggedUserId;
            category.CreatedById = LoggedUserId;
            var id = await _productCategoryService.CreateAsync(category);

            return RedirectToAction(nameof(Detail), new { id });
        }

        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProductCategory)]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _productCategoryService.FindAsync(id);
            var model = new ProductCategoryModel()
            {
                Id = category.Id,
                Description = category.Description,
                CreatedBy = category.CreatedBy,
                CreatedById = category.CreatedById,
                CreatedDate = category.CreatedDate,
                Name = category.Name,
                UpdateById = category.UpdatedById,
                ParentId = category.ParentId,
                ParentCategoryName = category.ParentCategoryName,
                UpdatedBy = category.UpdatedBy,
                UpdatedDate = category.UpdatedDate
            };
            return View(model);
        }

        [HttpPost]
        [ApplicationAuthorize(AuthorizePolicyConst.CanUpdateProductCategory)]
        public async Task<IActionResult> Update(ProductCategoryModel model)
        {
            var category = new ProductCategoryRequest()
            {
                Description = model.Description,
                Name = model.Name,
                ParentId = model.ParentId,
                Id = model.Id
            };

            if (category.Id <= 0)
            {
                return RedirectToErrorPage();
            }

            var exist = await _productCategoryService.FindAsync(model.Id);
            if (exist == null)
            {
                return RedirectToErrorPage();
            }

            category.UpdatedById = LoggedUserId;
            await _productCategoryService.UpdateAsync(category);
            return RedirectToAction(nameof(Detail), new { id = category.Id });
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadProductCategory)]
        public async Task<IActionResult> Search(string q, string currentId = null, bool isParentOnly = false)
        {
            int[] currentIds = null;
            if (!string.IsNullOrEmpty(currentId))
            {
                currentIds = currentId.Split(',').Select(x => int.Parse(x)).ToArray();
            }

            IList<ProductCategoryResult> categories;
            if (isParentOnly)
            {
                categories = await _productCategoryService.SearchParentsAsync(currentIds, q);
            }
            else
            {
                categories = await _productCategoryService.SearchAsync(currentIds, q);
            }

            if (categories == null || !categories.Any())
            {
                return Json(new List<Select2ItemModel>());
            }

            var categorySeletions = categories
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.ParentId.HasValue ? $"-- {x.Name}" : x.Name
                });

            return Json(categorySeletions);
        }
    }
}