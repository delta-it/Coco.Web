﻿using System.Threading.Tasks;
using System.Linq;
using LinqToDB;
using Camino.Shared.Requests.Filters;
using System;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Articles;
using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Articles;
using Camino.Core.Domain.Articles;
using Camino.Core.Domain.Media;
using System.Collections.Generic;
using Camino.Shared.Requests.Articles;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using LinqToDB.Tools;

namespace Camino.Service.Repository.Articles
{
    public class ArticlePictureRepository : IArticlePictureRepository
    {
        private readonly IRepository<ArticlePicture> _articlePictureRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<Article> _articleRepository;

        public ArticlePictureRepository(IRepository<ArticlePicture> articlePictureRepository, IRepository<Picture> pictureRepository,
            IRepository<Article> articleRepository)
        {
            _articlePictureRepository = articlePictureRepository;
            _pictureRepository = pictureRepository;
            _articleRepository = articleRepository;
        }

        public async Task<BasePageList<ArticlePictureResult>> GetAsync(ArticlePictureFilter filter)
        {
            var pictureQuery = _pictureRepository.Get(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(filter.Search))
            {
                var search = filter.Search.ToLower();
                pictureQuery = pictureQuery.Where(pic => pic.Title.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (!string.IsNullOrEmpty(filter.MimeType))
            {
                var mimeType = filter.MimeType.ToLower();
                pictureQuery = pictureQuery.Where(x => x.MimeType.Contains(mimeType));
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                pictureQuery = pictureQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTimeOffset.UtcNow);
            }

            var query = from ap in _articlePictureRepository.Table
                        join p in pictureQuery
                        on ap.PictureId equals p.Id
                        join a in _articleRepository.Table
                        on ap.ArticleId equals a.Id
                        select new ArticlePictureResult()
                        {
                            ArticleId = a.Id,
                            ArticleName = a.Name,
                            PictureId = p.Id,
                            PictureName = p.FileName,
                            ArticlePictureTypeId = ap.PictureTypeId,
                            PictureCreatedById = p.CreatedById,
                            PictureCreatedDate = p.CreatedDate,
                            ContentType = p.MimeType
                        };

            var filteredNumber = query.Select(x => x.PictureId).Count();

            var articlePictures = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<ArticlePictureResult>(articlePictures)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };

            return result;
        }

        public async Task<ArticlePictureResult> GetArticlePictureByArticleIdAsync(long articleId)
        {
            var articlePicture = await (from articlePic in _articlePictureRepository.Get(x => x.ArticleId == articleId)
                                        join picture in _pictureRepository.Get(x => !x.IsDeleted)
                                        on articlePic.PictureId equals picture.Id
                                        select new ArticlePictureResult
                                        {
                                            ArticleId = articlePic.ArticleId,
                                            ArticlePictureTypeId = articlePic.PictureTypeId,
                                            PictureId = articlePic.PictureId
                                        }).FirstOrDefaultAsync();
            return articlePicture;
        }

        public async Task<IList<ArticlePictureResult>> GetArticlePicturesByArticleIdsAsync(IEnumerable<long> articleIds)
        {
            var articlePictures = await (from articlePic in _articlePictureRepository.Get(x => x.ArticleId.In(articleIds))
                                        join picture in _pictureRepository.Get(x => !x.IsDeleted)
                                        on articlePic.PictureId equals picture.Id
                                        select new ArticlePictureResult
                                        {
                                            ArticleId = articlePic.ArticleId,
                                            ArticlePictureTypeId = articlePic.PictureTypeId,
                                            PictureId = articlePic.PictureId
                                        }).ToListAsync();
            return articlePictures;
        }

        public async Task<long> CreateAsync(ArticlePictureModifyRequest request)
        {
            var pictureData = Convert.FromBase64String(request.Picture.Base64Data);
            var pictureId = await _pictureRepository.AddWithInt64EntityAsync(new Picture()
            {
                CreatedById = request.UpdatedById,
                CreatedDate = request.CreatedDate,
                FileName = request.Picture.FileName,
                MimeType = request.Picture.ContentType,
                UpdatedById = request.UpdatedById,
                UpdatedDate = request.UpdatedDate,
                BinaryData = pictureData,
                IsPublished = true
            });

            var id = await _articlePictureRepository.AddWithInt64EntityAsync(new ArticlePicture()
            {
                ArticleId = request.ArticleId,
                PictureId = pictureId,
                PictureTypeId = (int)ArticlePictureType.Thumbnail
            });

            return id;
        }

        public async Task<bool> UpdateAsync(ArticlePictureModifyRequest request)
        {
            var pictureTypeId = (int)ArticlePictureType.Thumbnail;
            var shouldRemovePicture = request.Picture.Id == 0 && string.IsNullOrEmpty(request.Picture.Base64Data);
            var shouldUpdatePicture = request.Picture.Id == 0 && !string.IsNullOrEmpty(request.Picture.Base64Data);

            // Remove Old thumbnail
            if (shouldRemovePicture || shouldUpdatePicture)
            {
                var articlePictures = _articlePictureRepository
                    .Get(x => x.ArticleId == request.ArticleId && x.PictureTypeId == pictureTypeId);
                if (articlePictures.Any())
                {
                    var pictureIds = articlePictures.Select(x => x.PictureId).ToList();
                    await articlePictures.DeleteAsync();

                    await _pictureRepository.Get(x => x.Id.In(pictureIds))
                        .DeleteAsync();
                }
            }

            if (shouldUpdatePicture)
            {
                var base64Data = ImageUtil.EncodeJavascriptBase64(request.Picture.Base64Data);
                var pictureData = Convert.FromBase64String(base64Data);
                var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                {
                    CreatedById = request.UpdatedById,
                    CreatedDate = request.CreatedDate,
                    FileName = request.Picture.FileName,
                    MimeType = request.Picture.ContentType,
                    UpdatedById = request.UpdatedById,
                    UpdatedDate = request.UpdatedDate,
                    BinaryData = pictureData
                });

                _articlePictureRepository.Add(new ArticlePicture()
                {
                    ArticleId = request.ArticleId,
                    PictureId = pictureId,
                    PictureTypeId = pictureTypeId
                });
            }

            return true;
        }

        public async Task<bool> DeleteByArticleIdAsync(long articleId)
        {
            var articlePictures = _articlePictureRepository.Get(x => x.ArticleId == articleId);
            var pictureIds = articlePictures.Select(x => x.PictureId).ToList();
            await articlePictures.DeleteAsync();

            await _pictureRepository.Get(x => x.Id.In(pictureIds))
                .DeleteAsync();

            return true;
        }

        public async Task<bool> SoftDeleteByArticleIdAsync(long articleId)
        {
            await (from articlePicture in _articlePictureRepository.Get(x => x.ArticleId == articleId)
                   join picture in _pictureRepository.Table
                   on articlePicture.PictureId equals picture.Id
                   select picture)
                .Set(x => x.IsDeleted, true)
                .UpdateAsync();

            return true;
        }
    }
}
