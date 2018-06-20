using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;
using NW.PL.Models.Quest;
using NW.BL.DTO;

namespace NW.PL.Hubs
{
    public class ChatQuestHub : Hub
    {
        private static Thread PositionPlayers;

        static List<QuestGame> QuestGames = new List<QuestGame>();

        // Подключение нового пользователя
        public void Connect(int QuestId)
        {
            string Id = Context.ConnectionId;
            Identity identity = new Identity();

            QuestGame quest = QuestGames.FirstOrDefault(x => x.User(Id) != null) 
                ?? QuestGames.FirstOrDefault(x => x.UserId(identity.id) != null) 
                ?? QuestGames.FirstOrDefault(x => x.Id == QuestId);
            UserGame user = quest?.User(Id) ?? quest?.UserId(identity.id);
            UserGame creator = quest?.isCreator;

            if (identity.isAuth)
            {
                Clients.Caller.Auth();
                return;
            }
            else if (user == null)
            {
                quest = quest ?? new QuestGame(QuestId);
                user = new UserGame(Id, identity.id).setQuest(quest);
                quest.users.Add(user);
                QuestGames.Add(quest);
            }

            Clients.Caller.isYou(user.JsonUser);

            if (user.isCreator)
            {
                Clients.Caller.AddGame(quest.JsonQuestUsers);
                Clients.Caller.AddAnswers(quest.Answers);
            }
            else
            {
                if (creator != null)
                {
                    Clients.Client(creator.ConnectionId).AddUsers(new List<JsonUser>() { user.JsonUser });
                }

                Clients.Caller.AddGame(quest.JsonQuest);
                Clients.Caller.AddAnswers(user.Answers.Where(x => x.isTrue ?? false).Select(x => (JsonAnswerUser) x));
                Clients.Caller.AddTask(user.Answers.FirstOrDefault(x => x.isTrue == null));
            }

            if (PositionPlayers == null)
            {
                PositionPlayers = new Thread(CheckPositionPlayers);
                PositionPlayers.Start();
            }
        }

        public void SendAnswer(JsonAnswer answer)
        {
            string Id = Context.ConnectionId;

            QuestGame quest = QuestGames.FirstOrDefault(x => x.User(Id) != null);
            UserGame user = quest?.User(Id);
            UserGame creator = quest?.isCreator;

            if (user == null)
            {
                Clients.Caller.Reload();
                return;
            }
            else if (user.isCreator)
            {
                user = quest.UserId(answer.UserId);
                JsonAnswer Answer = user.Answers.FirstOrDefault(x => x.Id == answer.Id);
                Answer.isTrue = answer.isTrue;

                if (answer.isTrue ?? false)
                {
                    user.IndexTask++;
                    JsonAnswer task = quest.Task(user.IndexTask);

                    if (task == null)
                    {
                        user.Win = DateTime.Now;
                        Clients.Client(user.ConnectionId).Win(quest.JsonWins);
                    }
                    else
                    {
                        user.Answers.Add(new JsonAnswer(task).SetUserId(user.Id));
                        Clients.Client(user.ConnectionId).UpdateAnswer(Answer);
                        Clients.Client(user.ConnectionId).AddTask((JsonTask)Answer);
                    }
                }
            }
            else
            {
                JsonAnswer Answer = user.Answers.FirstOrDefault(x => x.Id == answer.Id);
                Answer.UserAnswer = answer.UserAnswer;

                Clients.Client(quest.isCreator.ConnectionId).AddAnswers(new List<JsonAnswer>() { Answer });
            }
        }

        public void AddPosition(double[] Position)
        {
            string Id = Context.ConnectionId;

            QuestGame quest = QuestGames.FirstOrDefault(x => x.User(Id) != null);
            UserGame user = quest?.User(Id);
            UserGame creator = quest?.isCreator;

            if (user != null)
            {
                user.Position = Position;

                if (creator != null)
                    Clients.Client(creator.ConnectionId).UserPosition(user.JsonUser);
            }
        }

        private void CheckPositionPlayers()
        {
            do
            {
                Clients.All.CheckPosition();
                Thread.Sleep(5000);
            } while (QuestGames.Count > 0);
            PositionPlayers = null;
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            string Id = Context.ConnectionId;

            QuestGame quest = QuestGames.FirstOrDefault(x => x.User(Id) != null);
            UserGame user = quest?.User(Id);
            UserGame creator = quest?.isCreator;

            if (creator != null)
            {
                Clients.Client(creator.ConnectionId).OutUser(user.JsonUser);
                quest.users.Remove(user);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}