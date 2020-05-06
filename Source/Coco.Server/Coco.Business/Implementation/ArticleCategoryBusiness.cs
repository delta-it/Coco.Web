﻿using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.DAL;
using Coco.Entities.Domain.Content;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Content;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Coco.Business.Implementation
{
    public class ArticleCategoryBusiness : IArticleCategoryBusiness
    {
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ContentDbContext _contentDbContext;

        public ArticleCategoryBusiness(IMapper mapper, IRepository<ArticleCategory> articleCategoryRepository,
            IRepository<User> userRepository, ContentDbContext contentDbContext)
        {
            _mapper = mapper;
            _articleCategoryRepository = articleCategoryRepository;
            _contentDbContext = contentDbContext;
            _userRepository = userRepository;
        }

        public ArticleCategoryDto Find(int id)
        {
            var exist = _articleCategoryRepository.Get(x => x.Id == id)
                .Include(x => x.ParentCategory)
                .FirstOrDefault();

            var createdByUser = _userRepository.Find(exist.CreatedById);
            var updatedByUser = _userRepository.Find(exist.UpdatedById);

            var category = _mapper.Map<ArticleCategoryDto>(exist);
            category.CreatedBy = createdByUser.DisplayName;
            category.UpdatedBy = updatedByUser.DisplayName;
            if (exist.ParentCategory != null)
            {
                category.ParentCategoryName = exist.ParentCategory.Name;
            }

            return category;
        }

        public List<ArticleCategoryDto> GetFull()
        {
            var categories = _articleCategoryRepository.Get().Select(a => new ArticleCategoryDto
            {
                CreatedById = a.CreatedById,
                CreatedDate = a.CreatedDate,
                Description = a.Description,
                Id = a.Id,
                Name = a.Name,
                ParentCategoryId = a.ParentCategoryId,
                UpdatedById = a.UpdatedById,
                UpdatedDate = a.UpdatedDate
            }).ToList();

            var createdByIds = categories.Select(x => x.CreatedById).ToArray();
            var updatedByIds = categories.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).ToList();

            foreach (var category in categories)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.CreatedBy = createdBy.DisplayName;

                var updatedBy = createdByUsers.FirstOrDefault(x => x.Id == category.CreatedById);
                category.UpdatedBy = updatedBy.DisplayName;
            }

            return categories;
        }

        public List<ArticleCategoryDto> Get(Expression<Func<ArticleCategory, bool>> filter)
        {
            var categories = _articleCategoryRepository.Get(filter).Select(a => new ArticleCategoryDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToList();

            return categories;
        }

        public long Add(ArticleCategoryDto category)
        {
            var newCategory = _mapper.Map<ArticleCategory>(category);
            newCategory.UpdatedDate = DateTime.UtcNow;
            newCategory.CreatedDate = DateTime.UtcNow;

            _articleCategoryRepository.Add(newCategory);
            return _contentDbContext.SaveChanges();
        }

        public ArticleCategoryDto Update(ArticleCategoryDto category)
        {
            var exist = _articleCategoryRepository.Find(category.Id);
            exist.Description = category.Description;
            exist.Name = category.Name;
            exist.ParentCategoryId = category.ParentCategoryId;
            exist.UpdatedById = category.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;

            _articleCategoryRepository.Update(exist);
            _contentDbContext.SaveChanges();

            category.UpdatedDate = exist.UpdatedDate;
            return category;
        }
    }
}
