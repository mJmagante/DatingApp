using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PageList<LikesDto>> GetUserLikes(LikesParams likesParams)
        {
            var user = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likesParams.Predicate == "liked")
            {
                likes = likes.Where(x => x.SourceUserId == likesParams.UserId);
                user = likes.Select(x => x.TargetUser);
            }

            if(likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(x => x.TargetUserId == likesParams.UserId);
                user = likes.Select(x => x.SourceUser);
            }

            var likedUser = user.Select(user => new LikesDto 
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PageList<LikesDto>.CreateAsync(likedUser, likesParams.PageNumber,likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(x => x.LikeUsers).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}