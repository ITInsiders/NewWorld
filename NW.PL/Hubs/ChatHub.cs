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

            if (identity.isAuth)
            {
                Clients.Caller.Auth();
                return;
            }
            else if (user == null)
            {
                quest = quest ?? new QuestGame(QuestId);
                user = new UserGame(Id, identity.id);
                quest.users.Add(user);
                QuestGames.Add(quest);
            }



            if (user == null)
            {
                user = new 


                Identity identity = new Identity();

                if (identity.user != null)
                {
                    user = quest?.User()

                    if (quest == null)
                    {
                        quest = new QuestGame() { Id = QuestId };
                        QuestGames.Add(quest);
                    }

                    user = new UserGame(Id, identity.id);

                    if (user == null)
                    {
                        user = new QuestGame.UserConnection()
                        {
                            ConnectionId = Id,
                            isCreator = quest.Quest.Creater == identity.user.Id,
                            Quest = quest,
                            UserId = identity.user.Id
                        };

                        quest.userConnection.Add(user);
                    }
                    else
                    {
                        user.ConnectionId = Id;
                    }
                }
                else
                {
                    Clients.Caller.Auth("Пожалуйста авторизуйтесь!");
                    return;
                }
            }

            
            if (!user.isCreator)
            {
                Clients.Caller.SetGame(quest.jsonQuestMini);

                if (quest.isCreator != null)
                {
                    Clients.Client(quest.isCreator.ConnectionId).NewUser(user.jsonUser);
                    Clients.Caller.SetMessage(quest.isCreator.Messages);
                }
            }
            else
            {
                Clients.Caller.SetGame(quest.jsonQuest);
            }


            if (PositionPlayers == null)
            {
                PositionPlayers = new Thread(CheckPositionPlayers);
                PositionPlayers.Start();
            }
        }

        public void SendMessage(string Message, int User = 0)
        {
            string Id = Context.ConnectionId;

            QuestGame quest = QuestGames.FirstOrDefault(x => x.UserConnect(Id) != null);
            QuestGame.UserConnection user = quest?.UserConnect(Id);
            QuestGame.UserConnection isCreator = quest?.isCreator;

            if (user == null)
            {
                Clients.Caller.Error("К сожалению вас нет в игре, попробуйте перезагрузить страницу!");
            }
            else
            {
                if (user.isCreator)
                {
                    if (User == 0)
                    {
                        Clients
                            .Clients(quest.Players.Select(x => x.ConnectionId).ToList())
                            .SetMessage(Message);
                    }
                    else
                    {
                        Clients.Client(quest.User(User).ConnectionId).Answer(Message);
                    }
                    
                }
                else if (isCreator != null)
                {
                    Clients
                        .Client(isCreator.ConnectionId)
                        .SetMessage(Message, user.jsonUser);
                }

                user.Messages.Add(Message);
            }
        }

        public void AddPosition(double[] Position)
        {
            string Id = Context.ConnectionId;

            QuestGame quest = QuestGames.FirstOrDefault(x => x.UserConnect(Id) != null);
            QuestGame.UserConnection user = quest?.UserConnect(Id);
            QuestGame.UserConnection isCreator = quest?.isCreator;

            if (user != null)
            {
                user.Position = Position;

                if (isCreator != null)
                    Clients.Client(isCreator.ConnectionId).UserPosition(user.UserId, Position);
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

            QuestGame quest = QuestGames.FirstOrDefault(x => x.UserConnect(Id) != null);
            QuestGame.UserConnection user = quest?.UserConnect(Id);

            if (quest != null && quest.isCreator != null)
            {
                Clients.Client(quest.isCreator.ConnectionId).OutUser(user.jsonUser);
                quest.userConnection.Remove(user);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}