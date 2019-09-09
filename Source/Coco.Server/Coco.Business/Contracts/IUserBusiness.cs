﻿using Coco.Entities.Model.User;
using Coco.Entities.Model.General;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IUserBusiness
    {
        long Create(UserModel user);
        UserModel Find(long id);
        UserLoggedInModel GetLoggedIn(long id);
        Task<UserModel> FindUserByEmail(string email, bool includeInActived = false);
        Task<UserModel> FindUserByUsername(string username, bool includeInActived = false);
        void Delete(long id);
        Task<UserModel> UpdateAuthenticationAsync(UserModel user);
        Task<UserProfileUpdateModel> UpdateUserProfileAsync(UserProfileUpdateModel model);
        Task<UserModel> FindByIdAsync(long id);
        Task<UserFullModel> GetFullByIdAsync(long id);
        Task<UpdatePerItem> UpdateInfoItemAsync(UpdatePerItem model);
        Task<UserLoggedInModel> UpdatePasswordAsync(UserPasswordUpdateModel model);
    }
}
