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
        public ILeagueRepository leagueRepository {get;set;}
        [Dependency]
        public IUnitofWork unitofWork { get; set; }

        public LeagueController(IUnitofWork unitofWork, ILeagueRepository leagueRepository)
        {
            this.leagueRepository = leagueRepository;
            this.unitofWork = unitofWork;
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
            var obj = new League
            {
                Name = model.Name,
                Competitor = model.Competitor,
                Creator = model.Creator,
                CreateDate = DateTime.Now.Date,
                Accepted = false
            };

            var league = leagueRepository.Add(obj);
            unitofWork.SaveChanges();
            return Ok(league);
        }
    }
}
