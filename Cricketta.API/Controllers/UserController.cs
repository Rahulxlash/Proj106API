using Cricketta.API.Models;
using Cricketta.Data.Base;
using Cricketta.Data.Data;
using Cricketta.Data.Model;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cricketta.API.Controllers
{
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        [Dependency]
        public IUserRepository userRepository {get;set;}
        [Dependency]
        public IUnitofWork unitofWork { get; set; }
        [Dependency]
        public ILeagueRepository leagueRepository { get; set; }

        public UserController(IUnitofWork unitofWork, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.leagueRepository = leagueRepository;
            this.unitofWork = unitofWork;
        }

        public async Task<IHttpActionResult> Get()
        {
            return Ok(userRepository.GetAll());
        }

        public async Task<IHttpActionResult> Get(int Id)
        {
            var user = userRepository.GetById(Id);
            return Ok(user);
        }

        public async Task<IHttpActionResult> GetByFBId(string id)
        {
            var user = userRepository.GetMany(u => u.FacebookId == id).FirstOrDefault();
            return Ok(user);
        }

        public async Task<IHttpActionResult> GetByUserName(string id)
        {
            var user = userRepository.GetMany(u => u.UserName.Trim().ToLower() == id.ToLower()).FirstOrDefault();
            return Ok(user);
        }

        public async Task<IHttpActionResult> Post(UserModel user)
        {
            var obj = userRepository.Add(new User
            {
                UserName = user.UserName,
                FacebookId = user.FacebookId,
                ProfileImage = user.ProfileImage
            });

            unitofWork.SaveChanges();
            return Ok(obj);
        }
        [HttpGet]
        public async Task<IHttpActionResult> League(int id)
        {
            var leagues = leagueRepository.GetMany(l => l.Creator == id || l.Competitor == id);
            return Ok(leagues);
        }
    }
}
