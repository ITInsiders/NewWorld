using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;
using Newtonsoft.Json;
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
            UserGame creator = quest?.Creator;

            if (!identity.isAuth)
            {
                Clients.Caller.Auth();
                return;
            }
            else if (user == null)
            {
                quest = quest ?? new QuestGame(QuestId);
                user = new UserGame(Id, identity.id).setQuest(quest);

                JsonAnswer task = quest.Task(user.IndexTask);
                user.Answers.Add(new JsonAnswer(task).SetUserId(user.Id));

                quest.users.Add(user);
                QuestGames.Add(quest);
            } else
            {
                user.ConnectionId = Id;
            }

            Clients.Caller.isYou(user.JsonUser);

            if (user.isCreator)
            {
                Clients.Caller.AddGame(quest.JsonQuestUsers);
                Clients.Caller.AddAnswers(quest.Answers.Where(x => x.UserAnswer != null));
            }
            else
            {
                if (creator != null)
                {
                    Clients.Client(creator.ConnectionId).ChangeUsers(new List<JsonUser>() { user.JsonUser }, true);
                }

                Clients.Caller.AddGame(quest.JsonQuest);
                Clients.Caller.AddAnswers(user.Answers.Where(x => x.isTrue != null).Select(x => x));

                if (user.Win == null)
                {
                    JsonAnswer task = new JsonAnswer(user.Answers.FirstOrDefault(x => x.isTrue == null));
                    Clients.Client(user.ConnectionId).AddAnswers(new List<JsonTask>() { new JsonTask(task) });
                } else
                {
                    user.Win = DateTime.Now;
                    Clients.Client(user.ConnectionId).Win(quest.JsonWins);

                    if (quest.isGameOver && quest.Creator != null)
                        Clients.Client(quest.Creator.ConnectionId).Win(quest.JsonWins);
                }
            }

            if (PositionPlayers == null)
            {
                PositionPlayers = new Thread(CheckPositionPlayers);
                PositionPlayers.Start();
            }
        }

        public void SendAnswer(string json)
        {
            JsonAnswer answer = JsonConvert.DeserializeObject<JsonAnswer>(json);

            string Id = Context.ConnectionId;

            QuestGame quest = QuestGames.FirstOrDefault(x => x.User(Id) != null);
            UserGame user = quest?.User(Id);
            UserGame creator = quest?.Creator;

            if (user == null)
            {
                Clients.Caller.Reload();
                return;
            }
            else if (user.isCreator)
            {
                user = quest.UserId(answer.UserId);
                JsonAnswer Answer = user.Answers.LastOrDefault(x => x.Id == answer.Id);
                if (Answer.isTrue == null)
                    Answer.isTrue = answer.isTrue;
                else
                    Answer.isTrue = false;

                if (Answer.isTrue ?? false && user.Lives > 0)
                {
                    user.IndexTask++;
                    JsonAnswer task = quest.Task(user.IndexTask);

                    Clients.Client(user.ConnectionId).UpdateAnswer(Answer);

                    if (task == null)
                    {
                        user.Win = DateTime.Now;
                        Clients.Client(user.ConnectionId).Win(quest.JsonWins);

                        if (quest.isGameOver && quest.Creator != null)
                        {
                            Clients.Client(quest.Creator.ConnectionId).Win(quest.JsonWins);
                        }
                    }
                    else
                    {
                        task = new JsonAnswer(task).SetUserId(user.Id);
                        user.Answers.Add(task);
                        Clients.Client(user.ConnectionId).AddAnswers(new List<JsonTask>() { new JsonTask(task) });

                        if (creator != null)
                        {
                            Clients.Client(creator.ConnectionId).AddAnswers(new List<JsonTask>() { new JsonTask(task) });
                        }
                    }
                }
                else
                {
                    JsonAnswer task = quest.Task(user.IndexTask);
                    task = new JsonAnswer(task).SetUserId(user.Id);
                    user.Answers.Add(task);

                    Answer = new JsonAnswer(Answer);

                    user.Lives--;
                    Clients.Caller.isYou(user.JsonUser);
                    Clients.Client(user.ConnectionId).UpdateAnswer(new JsonTask(Answer));
                    Clients.Client(user.ConnectionId).AddAnswers(new List<JsonTask>() { new JsonTask(Answer) });
                }
            }
            else
            {
                JsonAnswer Answer = user.Answers.LastOrDefault(x => x.Id == answer.Id);
                Answer.UserAnswer = answer.UserAnswer;

                Clients.Client(creator.ConnectionId).AddAnswers(new List<JsonAnswer>() { Answer });
            }
        }

        public void AddPosition(double[] Position)
        {
            string Id = Context.ConnectionId;

            QuestGame quest = QuestGames.FirstOrDefault(x => x.User(Id) != null);
            UserGame user = quest?.User(Id);
            UserGame creator = quest?.Creator;

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
                List<string> ids = QuestGames.SelectMany(x => x.users).Where(x => !x.isCreator).Select(x => x.ConnectionId).ToList();
                Clients.Clients(ids).CheckPosition();
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
            UserGame creator = quest?.Creator;

            if (creator != null)
            {
                Clients.Client(creator.ConnectionId).ChangeUsers(new List<JsonUser>() { user.JsonUser }, false);
            }

            if (quest != null && quest.isGameOver)
                QuestGames.Remove(quest);

            return base.OnDisconnected(stopCalled);
        }
    }
}