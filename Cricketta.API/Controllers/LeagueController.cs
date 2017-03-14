using Cricketta.Data.Base;
using Cricketta.Data.Data;
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

    }
}
