using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helper;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PageList<LikesDto>> GetUserLikes(LikesParams likesParams);
    }
}