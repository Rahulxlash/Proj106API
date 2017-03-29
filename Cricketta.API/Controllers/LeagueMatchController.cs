using Cricketta.API.Helpers;
using Cricketta.API.Models;
using Cricketta.Data.Data;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cricketta.API.Controllers
{
    public class LeagueMatchController : ApiController
    {
        [Dependency]
        public ILeagueMatchRepository leagueMatchRepository { get; set; }
        [Dependency]
        public ILeagueRepository leagueRepository { get; set; }
        [Dependency]
        public IUserRepository userRepository { get; set; }

        public IHttpActionResult Get(int Id)
        {
            var leagueMatch = leagueMatchRepository.GetById(Id);
            return Ok(leagueMatch);
        }

        [HttpPost]
        public IHttpActionResult RequestToss(int Id, int value)
        {
            var match = leagueMatchRepository.GetById(Id);
            var league = leagueRepository.GetById(match.LeagueId);
            var isMyLeague = league.Creator == value;

            if (match.TossDone == true)
                return BadRequest("Toss is already done for this match.");

            Notification obj = new Notification();
            obj.Title = league.Name.Trim();
            obj.Message = "Toss Request " + match.TeamName1.Trim() + " vs " + match.TeamName2.Trim() + " " + match.MatchDate.ToShortDateString();
            obj.Payload = new TossPayload
            {
                Tag = "TOSS_REQUEST",
                userId = value,
                matchId = match.LeagueMatchId
            };

            if (isMyLeague)
                NotificationHelper.sendNotification(getUserDeviceId(league.Competitor), obj);
            else
                NotificationHelper.sendNotification(getUserDeviceId(league.Creator), obj);

            return Ok();
        }

        private string getUserDeviceId(int userId)
        {
            var user = userRepository.GetById(userId);
            return user.DeviceToken;
        }

    }
}
