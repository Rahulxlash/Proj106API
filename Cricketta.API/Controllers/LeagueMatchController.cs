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


            return Ok(leagueMatch);
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
                    match.Toss = league.Creator;
                else
                    match.Toss = league.Competitor;
            }
            else
            {
                if (isMyLeague)
                    match.Toss = league.Creator;
                else
                    match.Toss = league.Competitor;
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
            var selPlayers = players.Where(p => selectedPlayers.Any(sp => p.PlayerId == sp.PlayerId));
            List<PlayerModel> teamPlayers = new List<PlayerModel>();

            foreach (var card in selectedPlayers)
            {
                Player player = playerRepository.GetById(card.PlayerId);

                PlayerModel obj = new PlayerModel
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
                        UserId = card.UserId,
                        Photo = player.Photo
                    };
                teamPlayers.Add(obj);
            }

            return Ok(new
            {
                matchId = Id,
                selected = teamPlayers,
                remain = remainPlayer
            });
        }

        public IHttpActionResult getScoreCard(int Id, int value)
        {
            var leagueMatch = leagueMatchRepository.GetById(Id);
            var selectedPlayers = scoreCardRepository.GetMany(sc => sc.MatchId == leagueMatch.MatchId && sc.LeagueId == leagueMatch.LeagueId);
            //var players = playerRepository.GetMany(p => selectedPlayers.Any(sp => p.PlayerId == sp.PlayerId));

            List<PlayerModel> data = new List<PlayerModel>();

            foreach (var card in selectedPlayers)
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

        [HttpPost]
        public IHttpActionResult addTeamPlayer(int Id, AddPlayerModel model)
        {
            var match = leagueMatchRepository.GetById(Id);
            var league = leagueRepository.GetById(match.LeagueId);
            var cards = scoreCardRepository.GetMany(s => s.MatchId == match.MatchId && s.UserId == model.UserId);
            bool isMyLeague = league.Creator == model.UserId;
            bool extra = false;
            if (cards.Count() >= 5)
                extra = true;

            LeagueScoreCard card = new LeagueScoreCard()
            {
                LeagueId = league.LeagueId,
                TournamentId = league.TournamentId,
                UserId = model.UserId,
                MatchId = match.MatchId,
                PlayerId = model.PlayerId,
                isPlaying = false,
                Run = 0,
                Wicket = 0,
                Extra = extra
            };

            scoreCardRepository.Add(card);
            unitOfWork.SaveChanges();
            cards = scoreCardRepository.GetMany(s => s.MatchId == match.MatchId);
            if (cards.Count() == 14)
                match.TeamDone = true;
            leagueMatchRepository.Update(match);

            unitOfWork.SaveChanges();
            string deviceId;
            if (isMyLeague)
                deviceId = userRepository.GetById(league.Competitor).DeviceToken;
            else
                deviceId = userRepository.GetById(league.Creator).DeviceToken;

            PlayerSelectedMessage msg = new PlayerSelectedMessage()
            {
                Tag = "PLAYER_SELECTED",
                leagueMatchId = match.LeagueMatchId,
                playerId = model.PlayerId,
                userId = model.UserId
            };

            NotificationHelper.sendMessage(deviceId, msg);

            return Ok(card);

        }

        private string getUserDeviceId(int userId)
        {
            var user = userRepository.GetById(userId);
            return user.DeviceToken;
        }

    }
}
