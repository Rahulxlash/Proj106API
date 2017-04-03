using Cricketta.API.Helpers;
using Cricketta.API.Models;
using Cricketta.Data.Base;
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
        [Dependency]
        public IPlayerRepository playerRepository { get; set; }
        [Dependency]
        public ITeamPlayerRepository teamPlayerRepository { get; set; }
        [Dependency]
        public ILeagueScoreCardRepository scoreCardRepository { get; set; }
        [Dependency]
        public IUnitofWork unitOfWork { get; set; }

        public IHttpActionResult Get(int id)
        {
            var leagueMatch = leagueMatchRepository.GetById(id);
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
                click_action = "MAIN_NOTIFICATION",
                userId = value,
                matchId = match.LeagueMatchId
            };

            if (isMyLeague)
                NotificationHelper.sendNotification(getUserDeviceId(league.Competitor), obj);
            else
                NotificationHelper.sendNotification(getUserDeviceId(league.Creator), obj);
            var leagueMatch = leagueMatchRepository.GetById(Id);

            leagueMatch.Toss = -1;
            leagueMatch.TossRequestedBy = value;
            leagueMatchRepository.Update(leagueMatch);
            unitOfWork.SaveChanges();


            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DoToss(int Id, int value)//1 for head and 2 for tail
        {
            var match = leagueMatchRepository.GetById(Id);
            var league = leagueRepository.GetById(match.LeagueId);
            var isMyLeague = league.Creator != match.TossRequestedBy;

            var Result = 0;
            var rand = new Random().Next(int.MinValue, int.MaxValue);
            if (rand % 2 == 0)
                Result = 1;//Head
            else
                Result = 2;//Tail

            if (value == Result)
            {
                if (isMyLeague)
                    match.Toss = 1;
                else
                    match.Toss = 2;
            }
            else
            {
                if (isMyLeague)
                    match.Toss = 2;
                else
                    match.Toss = 1;
            }

            match.TossDone = true;
            leagueMatchRepository.Update(match);
            unitOfWork.SaveChanges();

            Notification obj = new Notification();
            obj.Title = league.Name.Trim();
            obj.Message = "Toss " + (value == Result ? "Lost " : "Won ") + match.TeamName1.Trim() + " vs " + match.TeamName2.Trim() + " " + match.MatchDate.ToShortDateString();
            obj.Payload = new TossPayload
            {
                Tag = "TOSS_DONE",
                click_action = "MAIN_NOTIFICATION",
                userId = value,
                matchId = match.LeagueMatchId
            };
            if (isMyLeague)
                NotificationHelper.sendNotification(getUserDeviceId(league.Competitor), obj);
            else
                NotificationHelper.sendNotification(getUserDeviceId(league.Creator), obj);

            return Ok(new
            {
                Result = Result,
                Value = (Result == 1 ? "Head" : "Tail")
            });
        }

        public IHttpActionResult getAllPlayers(int Id)
        {
            var leagueMatch = leagueMatchRepository.GetById(Id);
            var teamPlayer = teamPlayerRepository.GetMany(tp => tp.TeamId == leagueMatch.TeamId1 || tp.TeamId == leagueMatch.TeamId2);
            var players = playerRepository.GetMany(p => teamPlayer.Any(tp => p.PlayerId == tp.PlayerId));

            var selectedPlayers = scoreCardRepository.GetMany(sc => sc.MatchId == leagueMatch.MatchId && sc.LeagueId == leagueMatch.LeagueId).ToList();
            var remainPlayer = players.Where(p => !selectedPlayers.Any(sp => p.PlayerId == sp.PlayerId));

            return Ok(remainPlayer);
        }

        public IHttpActionResult getScoreCard(int Id, int value)
        {
            var leagueMatch = leagueMatchRepository.GetById(Id);
            var selectedPlayers = scoreCardRepository.GetMany(sc => sc.MatchId == leagueMatch.MatchId && sc.LeagueId == leagueMatch.LeagueId);
            //var players = playerRepository.GetMany(p => selectedPlayers.Any(sp => p.PlayerId == sp.PlayerId));

            List<PlayerModel> data = new List<PlayerModel>();

            foreach(var card in selectedPlayers)
            {
                var player = playerRepository.GetById(card.PlayerId);
                var obj = new PlayerModel
                {
                    PlayerId = card.PlayerId,
                    Name = player.Name.Trim(),
                    Run = card.Run,
                    Wicket = card.Wicket,
                    Bat = player.Bat,
                    Bowl = player.Bowl,
                    Captain = player.Captain,
                    isExtra = card.Extra,
                    isPlaying = card.isPlaying,
                    Keeper = player.Keeper,
                    MatchId = card.MatchId,
                    UserId = card.UserId
                };
                data.Add(obj);
            }
            return Ok(data);
        }

        //[HttpPost]
        //public IHttpActionResult selectTeamPlayer()
        //{

        //}

        private string getUserDeviceId(int userId)
        {
            var user = userRepository.GetById(userId);
            return user.DeviceToken;
        }

    }
}
