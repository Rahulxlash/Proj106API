using Cricketta.API.Helpers;
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
        public ILeagueMatchRepository leagueMatchRepository { get; set; }
        [Dependency]
        public IMatchRepository matchRepository { get; set; }
        [Dependency]
        public ITeamRepository teamRepository { get; set; }
        [Dependency]
        public IUnitofWork unitofWork { get; set; }

        public LeagueController()
        {
        }

        public async Task<IHttpActionResult> Get()
        {
            return Ok(leagueRepository.GetAll());
        }

        [HttpGet]
        public async Task<IHttpActionResult> Summary(int id, int value)
        {
            var league = leagueRepository.GetById(id);
            var leagueMatches = leagueMatchRepository.GetMany(lm => lm.LeagueId == id && lm.TossDone == true).OrderBy(lm => lm.MatchDate).ToList();
            var isMyLeague = value == league.Creator;
            var CompUser = userRepository.GetById((isMyLeague ? league.Competitor : league.Creator));

            swapValues(leagueMatches, isMyLeague);


            LeagueModel result = new LeagueModel()
            {
                LeagueId = league.LeagueId,
                Name = league.Name.Trim(),
                Points = (isMyLeague ? league.Points : league.Points * -1),
                SummaryMatches = leagueMatches,
                Accepted = league.Accepted,
                Competitor = league.Competitor.ToString().Trim(),
                CompetitorName = CompUser.UserName.Trim(),
                CompetitorPoint = (isMyLeague ? league.CompetitorPoint : league.CreatorPoint),
                Creator = league.Creator,
                CreatorPoint = (isMyLeague ? league.CreatorPoint : league.CompetitorPoint),
                IsMyLeague = isMyLeague,
                CompetitorFBId = CompUser.FacebookId.Trim(),
            };

            return Ok(result);
        }

        private static void swapValues(List<LeagueMatch> leagueMatches, bool isMyLeague)
        {
            if (!isMyLeague)
            {
                foreach (var match in leagueMatches)
                {
                    var run = match.CreatorRun;
                    match.CreatorRun = match.CompetitorRun;
                    match.CompetitorRun = match.CreatorRun;

                    var wicket = match.CreatorWicket;
                    match.CreatorWicket = match.CompetitorWicket;
                    match.CompetitorWicket = match.CreatorWicket;

                    var point = match.CreatorPoint;
                    match.CreatorPoint = match.CompetitorPoint;
                    match.CompetitorPoint = match.CreatorPoint;
                }
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Matches(int id, int value)
        {
            var matches = leagueMatchRepository.GetMany(lm => lm.LeagueId == id).OrderBy(lm => lm.MatchDate).ToList();
            var isMyLeague = id == value;
            swapValues(matches, isMyLeague);

            return Ok(matches);
        }

        public async Task<IHttpActionResult> Post(LeagueModel model)
        {
            //Add New League
            String compid = model.Competitor.ToString();
            var appUser = userRepository.GetMany(u => u.FacebookId == compid).FirstOrDefault();
            var creator = userRepository.GetById(model.Creator);
            var obj = new League
            {
                Name = model.Name,
                Competitor = appUser.UserId,
                Creator = model.Creator,
                CreateDate = DateTime.Now.Date,
                Accepted = 0,
                TournamentId = 1
            };

            var league = leagueRepository.Add(obj);
            unitofWork.SaveChanges();
            //Insert Matches for League
            var matches = matchRepository.GetMany(m => m.TournamentId == 1).ToList();

            foreach (var match in matches)
            {
                LeagueMatch lm = new LeagueMatch()
                {
                    LeagueId = league.LeagueId,
                    MatchDate = match.MatchDate,
                    TeamId1 = match.TeamId1,
                    TeamId2 = match.TeamId2,
                    Toss = 0,
                    TossDone = false,
                    Venue = match.Venue,
                    MatchId = match.MatchId,
                    TeamName1 = teamRepository.GetById(match.TeamId1).ShortName,
                    TeamName2 = teamRepository.GetById(match.TeamId2).ShortName
                };

                leagueMatchRepository.Add(lm);
            }

            unitofWork.SaveChanges();

            Notification notif = new Notification()
            {
                Title = creator.UserName,
                Message = "New Challange from " + creator.UserName + "- " + league.Name,
                Payload = new ChallangePayload()
                {
                    leagueId = league.LeagueId,
                    Tag = "LEAGUE_CHALLANGE",
                    userId = model.Creator
                }
            };

            NotificationHelper.sendNotification(appUser.DeviceToken, notif);
            return Ok(league);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Accept(int id)
        {
            var league = leagueRepository.GetById(id);
            league.Accepted = 1;
            leagueRepository.Update(league);
            unitofWork.SaveChanges();
            var creator = userRepository.GetById(league.Creator);
            var comp = userRepository.GetById(league.Competitor);

            Notification obj = new Notification()
            {
                Title = "Challange Accepted",
                Message = league.Name + " accepted by " + comp.UserName,
                Payload = new ChallangePayload()
                {
                    userId = league.Competitor,
                    Tag = "CHALLANGE_ACCEPTED",
                    leagueId = league.LeagueId
                }
            };


            NotificationHelper.sendNotification(creator.DeviceToken, obj);

            return Ok(league);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Reject(int id)
        {
            var league = leagueRepository.GetById(id);
            league.Accepted = 2;
            leagueRepository.Update(league);
            unitofWork.SaveChanges();
            var creator = userRepository.GetById(league.Creator);
            var comp = userRepository.GetById(league.Competitor);

            Notification obj = new Notification()
            {
                Title = "Challange Rejected",
                Message = league.Name + " rejected by " + comp.UserName,
                Payload = new ChallangePayload()
                {
                    userId = league.Competitor,
                    Tag = "CHALLANGE_ACCEPTED",
                    leagueId = league.LeagueId
                }
            };


            NotificationHelper.sendNotification(creator.DeviceToken, obj);
            return Ok(league);
        }
    }
}
