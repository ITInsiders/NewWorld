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

            if (!identity.isAuth)
            {
                Clients.Caller.Auth();
                return;
            }
            else if (user == null)
            {
                if (quest == null)
                {
                    quest = new QuestGame(QuestId);
                    QuestGames.Add(quest);
                }

                user = new UserGame(Id, identity.id).setQuest(quest);

                if (!user.isCreator)
                {
                    JsonAnswer task = quest.Task(user.IndexTask);
                    user.Answers.Add(new JsonAnswer(task).SetUserId(user.Id));
                }
                
                quest.users.Add(user);
            } else
            {
                user.ConnectionId = Id;
            }

            Clients.Caller.isYou(user.JsonUser);

            if (user.isCreator)
            {
                Clients.Caller.AddGame(quest.JsonQuestUsers);
                Clients.Caller.AddAnswers(quest.Answers.Where(x => x.UserAnswer != null));

                if (quest.isGameOver)
                {
                    Clients.Caller.Win(quest.JsonWins);
                }
            }
            else
            {

                UserGame creator = quest?.Creator;
                if (creator != null)
                {
                    Clients.Client(creator.ConnectionId).ChangeUsers(new List<JsonUser>() { user.JsonUser }, true);
                }

                Clients.Caller.AddGame(quest.JsonQuest);
                Clients.Caller.AddAnswers(user.Answers);

                if (user.Win != null)
                {
                    Clients.Caller.Win(quest.JsonWins);
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
                creator = user;
                user = quest.UserId(answer.UserId);

                JsonAnswer Answer = user.Answers.LastOrDefault(x => x.Id == answer.Id);

                if (Answer.isTrue == null)
                {
                    bool isTrue = answer.isTrue ?? false;
                    Answer.isTrue = isTrue;
                    
                    if (isTrue)
                    {
                        Clients.Client(user.ConnectionId).UpdateAnswer(Answer);
                        user.IndexTask++;
                    }
                    else
                    {
                        user.Lives--;
                        Clients.Client(user.ConnectionId).UpdateAnswer(new JsonTask(Answer));
                    }

                    JsonAnswer task = quest.Task(user.IndexTask);
                    Clients.Client(user.ConnectionId).isYou(user.JsonUser);

                    if (task != null && user.Lives >= 0)
                    {
                        task = new JsonAnswer(task);
                        task = task.SetUserId(user.Id);

                        user.Answers.Add(task);
                        Clients.Client(user.ConnectionId).AddAnswers(new List<JsonTask>() { new JsonTask(task) });
                    }
                    else if (task == null)
                    {
                        user.Win = DateTime.Now;
                        Clients.Client(user.ConnectionId).Win(quest.JsonWins);

                        if (quest.isGameOver)
                        {
                            Clients.Client(creator.ConnectionId).Win(quest.JsonWins);
                        }
                    }
                }
            }
            else
            {
                JsonAnswer Answer = user.Answers.LastOrDefault(x => x.Id == answer.Id);
                Answer.UserAnswer = answer.UserAnswer;

                if (creator != null)
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

            if (creator != null && !user.isCreator)
            {
                Clients.Client(creator.ConnectionId).ChangeUsers(new List<JsonUser>() { user.JsonUser }, false);
            }
            
            if (quest != null && quest.isGameOver)
                QuestGames.Remove(quest);

            return base.OnDisconnected(stopCalled);
        }
    }
}