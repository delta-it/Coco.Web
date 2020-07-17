﻿using Coco.Business.Contracts;
using Coco.Business.ValidationStrategies;
using Coco.Core.Exceptions;
using Coco.Core.Utils;
using Coco.Contract;
using Coco.Core.Dtos.General;
using System;
using System.Linq;
using System.Threading.Tasks;
using Coco.Core.Entities.Content;
using System.Collections.Generic;
using Coco.Core.Entities.Identity;
using Coco.Core.Dtos.Content;
using UserPhotoType = Coco.Core.Entities.Enums.UserPhotoType;

namespace Coco.Business.Implementation.UserBusiness
{
    public class UserPhotoBusiness : IUserPhotoBusiness
    {
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        public UserPhotoBusiness(ValidationStrategyContext validationStrategyContext, IRepository<UserPhoto> userPhotoRepository,
            IRepository<UserInfo> userInfoRepository)
        {
            _userPhotoRepository = userPhotoRepository;
            _userInfoRepository = userInfoRepository;
            _validationStrategyContext = validationStrategyContext;
        }

        public async Task<UserPhotoUpdateDto> UpdateUserPhotoAsync(UserPhotoUpdateDto model, long userId)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userInfo = _userInfoRepository.FirstOrDefault(x => x.Id == userId);
            if (userInfo == null)
            {
                throw new ArgumentException(nameof(userInfo));
            }

            _validationStrategyContext.SetStrategy(new Base64ImageValidationStrategy());
            bool canUpdate = _validationStrategyContext.Validate(model.PhotoUrl);

            if (!canUpdate)
            {
                throw new ArgumentException(model.PhotoUrl);
            }

            if (model.UserPhotoType == UserPhotoType.Avatar)
            {
                _validationStrategyContext.SetStrategy(new AvatarValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(model);
            }
            else if (model.UserPhotoType == UserPhotoType.Cover)
            {
                _validationStrategyContext.SetStrategy(new UserCoverValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(model);
            }

            if (!canUpdate && model.UserPhotoType == UserPhotoType.Avatar)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPhotoType.Avatar)}Should larger than 100px X 100px");
            }
            else if (!canUpdate)
            {
                throw new PhotoSizeInvalidException($"{nameof(UserPhotoType.Cover)}Should larger than 1000px X 300px");
            }

            int maxSize = model.UserPhotoType == UserPhotoType.Avatar ? 600 : 1000;
            var newImage = ImageUtil
                .Crop(model.PhotoUrl, model.XAxis, model.YAxis, model.Width, model.Height, model.Scale, maxSize);

            var userPhotoType = (byte)model.UserPhotoType;
            var userPhoto = _userPhotoRepository
                .Get(x => x.UserId == userId && x.TypeId == userPhotoType)
                .FirstOrDefault();

            model.UserPhotoCode = Guid.NewGuid().ToString();
            if (userPhoto == null)
            {
                userPhoto = new UserPhoto()
                {
                    CreatedById = userId,
                    CreatedDate = DateTime.UtcNow,
                    ImageData = newImage,
                    TypeId = (byte)model.UserPhotoType,
                    UserId = userId,
                    Name = model.FileName,
                    Code = model.UserPhotoCode,
                };

                await _userPhotoRepository.AddAsync(userPhoto);
            }
            else
            {
                userPhoto.ImageData = newImage;
                userPhoto.Name = model.FileName;
                userPhoto.Code = model.UserPhotoCode;
                await _userPhotoRepository.UpdateAsync(userPhoto);
            }
            model.PhotoUrl = userPhoto.Code;
            return model;
        }

        public async Task DeleteUserPhotoAsync(long userId, UserPhotoType userPhotoType)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var type = (byte)userPhotoType;
            var userPhoto = _userPhotoRepository
                .Get(x => x.UserId.Equals(userId) && x.TypeId.Equals(type))
                .FirstOrDefault();

            if (userPhoto == null)
            {
                return;
            }

            await _userPhotoRepository.DeleteAsync(userPhoto);
        }

        public async Task<UserPhotoDto> GetUserPhotoByCodeAsync(string code, UserPhotoType type)
        {
            var photoType = (byte)type;
            var userPhotos = await _userPhotoRepository.GetAsync(x => x.Code.Equals(code) && x.TypeId.Equals(photoType));

            if (userPhotos == null || !userPhotos.Any())
            {
                return null;
            }

            return userPhotos.Select(x => new UserPhotoDto()
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                ImageData = x.ImageData,
                Name = x.Name,
                TypeId = x.TypeId,
                UserId = x.UserId,
                Url = x.Url
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<UserPhotoDto>> GetUserPhotosAsync(long userId)
        {
            var userPhotos = await _userPhotoRepository.GetAsync(x => x.UserId == userId);
            
            return userPhotos.Select(x => new UserPhotoDto()
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Url = x.Url,
                TypeId = x.TypeId
            });
        }

        public UserPhotoDto GetUserPhotoByUserId(long userId, UserPhotoType type)
        {
            var photoType = (byte)type;
            var userPhotos = _userPhotoRepository.Get(x => x.UserId == userId && x.TypeId.Equals(photoType));
            if (userPhotos == null || !userPhotos.Any())
            {
                return null;
            }

            return userPhotos.Select(x => new UserPhotoDto()
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Url = x.Url,
                TypeId = x.TypeId
            }).FirstOrDefault();
        }
    }
}