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
    public class LeagueController : ApiController
    {
        [Dependency]
        public ILeagueRepository leagueRepository { get; set; }
        [Dependency]
        public IUserRepository userRepository { get; set; }
        [Dependency]
        public IUnitofWork unitofWork { get; set; }

        public LeagueController()
        {
        }

        public async Task<IHttpActionResult> Get()
        {
            return Ok(leagueRepository.GetAll());
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            return Ok(leagueRepository.GetById(id));
        }

        public async Task<IHttpActionResult> GetByName(String id)
        {
            return Ok(leagueRepository.GetMany(l => l.Name.ToLower() == id.ToLower()));
        }

        public async Task<IHttpActionResult> Post(LeagueModel model)
        {
            String compid = model.Competitor.ToString();
            var appUser = userRepository.GetMany(u => u.FacebookId == compid).FirstOrDefault();

            var obj = new League
            {
                Name = model.Name,
                Competitor = appUser.UserId,
                Creator = model.Creator,
                CreateDate = DateTime.Now.Date,
                Accepted = 0
            };

            var league = leagueRepository.Add(obj);
            unitofWork.SaveChanges();
            return Ok(league);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Accept(int id)
        {
            var league = leagueRepository.GetById(id);
            league.Accepted = 1;
            leagueRepository.Update(league);
            unitofWork.SaveChanges();
            return Ok(league);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Reject(int id)
        {
            var league = leagueRepository.GetById(id);
            league.Accepted = 2;
            leagueRepository.Update(league);
            unitofWork.SaveChanges();
            return Ok(league);
        }
    }
}
