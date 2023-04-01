using BigSchool.DTOs;
using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        public FollowingsController() 
        {
            _dbContext = new ApplicationDbContext();
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult Follow(FollowingDto followingDto)
        {
            var userId = User.Identity.GetUserId();
            if (_dbContext.Followings.Any(f => f.FollowerId == userId && f.FolloweeId == followingDto.FolloweeId))
            {
                return BadRequest("Following Already Exists!!!");
            }

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = followingDto.FolloweeId
            };

            _dbContext.Followings.Add(following);
            _dbContext.SaveChanges();

            return Ok();
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult UnFollow(string followerId, string followeeId)
        {
            var follow = _dbContext.Followings
                .Where(x => x.FollowerId == followerId && x.FolloweeId == followeeId)
                .Include(x => x.Follower)
                .Include(x => x.Followee).SingleOrDefault();

            _dbContext.Followings.Remove(follow);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
