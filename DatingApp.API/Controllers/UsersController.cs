using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{   
    [ServiceFilter(typeof(LogUserActivity))]  // users activity will be save
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        [HttpGet]

        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

             var userFromRepo = await _repo.GetUser(currentUserId);
            userParams.UserId = currentUserId;

          if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }


            var users = await _repo.GetUsers(userParams);
            //  lesson 78 28-05-2020 paging also
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            
         //   Response.AddPagination(users.CurrentPage, users.PageSize, 
        //    users.TotalPages,users.TotalPages);
         Response.AddPagination(users.CurrentPage, users.PageSize, 
            users.TotalCount,users.TotalPages);
            return Ok(usersToReturn);
            //  end lesson 78 pagenation
           

        }
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            //  lesson 78 28-05-2020
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
            //  end lesson 78
            

        }
        [HttpPut("{id}")]  // id from routes

        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             return Unauthorized();

             var userFromRepo = await _repo.GetUser(id);

             _mapper.Map(userForUpdateDto, userFromRepo);

             if (await _repo.SaveAll())
               return NoContent();

               throw new System.Exception($"Updating user {id} failed on save");
        }
         // for likes
         [HttpPost("{id}/like/{recipientId}")]
         public async Task<IActionResult> LikeUser(int id, int recipientId)
         {
             if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                 return Unauthorized();

              var like = await _repo.GetLike(id, recipientId);

              if (like != null)
                 return BadRequest("You already like this user");

              if (await _repo.GetUser(recipientId) == null)
                 return NotFound();

              like = new Like
             {
                 LikerId = id,
                 LikeeId = recipientId
             };

              _repo.Add<Like>(like);

              if (await _repo.SaveAll()) // will save the database await se
                 return Ok();

              return BadRequest("Failed to like user");
         }

    }
}