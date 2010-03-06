using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BNSharp;
using BNSharp.BattleNet;
using BNSharp.BattleNet.Clans;
using System.Diagnostics;
using System.Text.RegularExpressions;
using BNSharp.BattleNet.Stats;
using JinxBot.Controls;
using System.Drawing;
using System.Timers;

namespace JinxBot.Plugins.ClanGnome
{
    public class ClanGnome
    {
        private string m_dbPath;
        private IJinxBotClient m_client;
        private static Regex ClanSearch = new Regex(@"clan\s+(?<tag>\w{2,4})\W", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private int m_gatewayID, m_nextClanID;
        private Timer m_timer;

        public ClanGnome(string dbPath, IJinxBotClient client)
        {
            m_timer = new Timer(45000) { AutoReset = true };
            m_timer.Elapsed += new ElapsedEventHandler(m_timer_Elapsed);

            Debug.Assert(client != null);
            m_client = client;
            m_dbPath = dbPath;
            client.Client.RegisterUserSpokeNotification(Priority.Low, HandleChat);
            client.Client.RegisterWhisperReceivedNotification(Priority.Low, HandleChat);
            client.Client.RegisterUserShownNotification(Priority.Low, HandleUser);
            client.Client.RegisterUserFlagsChangedNotification(Priority.Low, HandleUser);
            client.Client.RegisterUserJoinedNotification(Priority.Low, HandleUser);
            client.Client.RegisterConnectedNotification(Priority.Low, HandleConnected);
            client.Client.RegisterChannelDidNotExistNotification(Priority.Low, HandleChannelError);
            client.Client.RegisterChannelWasFullNotification(Priority.Low, HandleChannelError);
            client.Client.RegisterChannelWasRestrictedNotification(Priority.Low, HandleChannelError);

            client.Client.RegisterJoinedChannelNotification(Priority.Low, HandleChannelJoined);

            using (ClansDataContext dc = ClansDataContext.Create(m_dbPath))
            {
                var gw = (from g in dc.Gateways
                          where g.Name.ToUpper() == client.Client.Settings.Gateway.Name.ToUpper()
                          select g).FirstOrDefault();
                if (gw != null)
                    m_gatewayID = gw.ID;
                else
                {
                    Gateway newGateway = new Gateway { Name = client.Client.Settings.Gateway.Name };
                    dc.Gateways.InsertOnSubmit(newGateway);
                    dc.SubmitChanges();
                    m_gatewayID = newGateway.ID;
                }
            }

            m_client.MainWindow.AddChat(new ChatNode[] { 
                new ChatNode("JinxBot Clan Gnome", Color.DarkRed),
                new ChatNode(" initialized...", Color.White)
            });
        }

        void m_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (m_client.Client.IsConnected)
            {
                try
                {
                    using (var dc = ClansDataContext.Create(m_dbPath))
                    {
                        // select the clan with the oldest channel visit
                        var clans = from cl in
                                        (from c in dc.Clans
                                         where c.GatewayID == m_gatewayID
                                         select new
                                         {
                                             Clan = c,
                                             LatestChannelView = c.ChannelViews.OrderByDescending(cv => cv.When).Select(cv => cv.When).FirstOrDefault()
                                         })
                                    orderby cl.LatestChannelView ascending
                                    select cl.Clan;
                        var clan = clans.FirstOrDefault();

                        if (clan != null)
                        {
                            m_client.Client.JoinChannel("Clan " + clan.Tag, JoinMethod.Default);
                            m_nextClanID = clan.ID;
                            Debug.WriteLine("Found clan " + clan.Tag);
                        }
                        else
                        {
                            m_client.Client.JoinChannel("Clan Recruitment", JoinMethod.Default);
                            m_nextClanID = 0;
                            Debug.WriteLine("No channel found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    m_client.MainWindow.AddChat(new ChatNode[] {
                        new ChatNode("JinxBot Clan Gnome error ", Color.DarkRed),
                        new ChatNode(ex.ToString(), Color.Yellow)
                    });
                }
            }
        }

        public void Unhook()
        {
            m_client.Client.UnregisterUserSpokeNotification(Priority.Low, HandleChat);
            m_client.Client.UnregisterWhisperReceivedNotification(Priority.Low, HandleChat);
            m_client.Client.UnregisterUserShownNotification(Priority.Low, HandleUser);
            m_client.Client.UnregisterUserFlagsChangedNotification(Priority.Low, HandleUser);
            m_client.Client.UnregisterUserJoinedNotification(Priority.Low, HandleUser);
            m_client.Client.UnregisterConnectedNotification(Priority.Low, HandleConnected);
            m_client.Client.UnregisterChannelDidNotExistNotification(Priority.Low, HandleChannelError);
            m_client.Client.UnregisterChannelWasFullNotification(Priority.Low, HandleChannelError);
            m_client.Client.UnregisterChannelWasRestrictedNotification(Priority.Low, HandleChannelError);

            m_client.Client.UnregisterJoinedChannelNotification(Priority.Low, HandleChannelJoined);
        }

        private void HandleChannelJoined(object sender, ServerChatEventArgs e)
        {
            
        }

        private void HandleChannelError(object sender, ServerChatEventArgs e)
        {
            if (m_nextClanID != 0)
            {
                using (var dc = ClansDataContext.Create(m_dbPath))
                {
                    var view = new ChannelView
                    {
                        ClanID = m_nextClanID,
                        AllowedView = false,
                        When = DateTime.Now,
                        UserCount = 0,
                        UserList = ""
                    };
                    dc.ChannelViews.InsertOnSubmit(view);

                    try
                    {
                        dc.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        m_client.MainWindow.AddChat(new ChatNode[] { 
                                new ChatNode("JinxBot Clan Gnome error ", Color.DarkRed),
                                new ChatNode("Error saving a new clan:", Color.Red),
                                ChatNode.NewLine,
                                new ChatNode(ex.ToString(), Color.OrangeRed)
                            });
                    }
                }
            }
        }

        private void HandleChat(object sender, ChatMessageEventArgs e)
        {
            Match m = ClanSearch.Match(e.Text);
            if (m.Success)
            {
                string tag = m.Groups["tag"].Value;
                NoteTag(tag, e.Username, m_client.Client.ChannelName, e.Text);
            }
        }

        private void HandleUser(object sender, UserEventArgs e)
        {
            Product p = e.User.Stats.Product;
            if (p == Product.Warcraft3Retail || p == Product.Warcraft3Expansion)
            {
                // check to see if the user has a clan
                Warcraft3Stats w3s = e.User.Stats as Warcraft3Stats;
                string tag = w3s.ClanTag;
                NoteTag(tag, e.User, m_client.Client.ChannelName, e.EventType);
            }

            if (e.User.Username == m_client.Client.UniqueUsername)
            {
                if (m_nextClanID != 0)
                {
                    using (var dc = ClansDataContext.Create(m_dbPath))
                    {
                        var view = new ChannelView
                        {
                            ClanID = m_nextClanID,
                            AllowedView = true,
                            When = DateTime.Now,
                            UserCount = m_client.Client.Channel.Count,
                            UserList = FormatChannel(m_client.Client.Channel)
                        };
                        dc.ChannelViews.InsertOnSubmit(view);

                        try
                        {
                            dc.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            m_client.MainWindow.AddChat(new ChatNode[] { 
                                new ChatNode("JinxBot Clan Gnome error ", Color.DarkRed),
                                new ChatNode("Error saving a clan visit:", Color.Red),
                                ChatNode.NewLine,
                                new ChatNode(ex.ToString(), Color.OrangeRed)
                            });
                        }
                    }
                }
            }
        }

        private string FormatChannel(IEnumerable<ChatUser> readOnlyCollection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var user in readOnlyCollection)
            {
                if (sb.Length > 0)
                    sb.Append(",");

                sb.Append(user.Username);
            }

            return sb.ToString();
        }

        private void NoteTag(string tag, string userName, string currentChannel, string action)
        {
            using (ClansDataContext dc = ClansDataContext.Create(m_dbPath))
            {
                if (dc.Clans.Where(c => c.Tag.ToUpper() == tag.ToUpper()).Count() == 0)
                {
                    Clan c = new Clan
                    {
                        GatewayID = m_gatewayID,
                        Tag = tag,
                        DiscoverySource = string.Format("{0}, a user in {1}, saying \"{2}\".", userName, currentChannel, action)
                    };
                    dc.Clans.InsertOnSubmit(c);

                    try
                    {
                        dc.SubmitChanges();
                        m_client.MainWindow.AddChat(new ChatNode[] { 
                            new ChatNode("JinxBot Clan Gnome added ", Color.DarkRed),
                            new ChatNode(c.DiscoverySource, Color.Yellow)
                        });
                    }
                    catch (Exception ex)
                    {
                        m_client.MainWindow.AddChat(new ChatNode[] { 
                            new ChatNode("JinxBot Clan Gnome error ", Color.DarkRed),
                            new ChatNode("Error saving a new clan:", Color.Red),
                            ChatNode.NewLine,
                            new ChatNode(ex.ToString(), Color.OrangeRed)
                        });
                    }
                }
                else
                {
                    m_client.MainWindow.AddChat(new ChatNode[] { 
                        new ChatNode("JinxBot Clan Gnome note: ", Color.DarkRed),
                        new ChatNode("Already noted clan ", Color.Yellow),
                        new ChatNode(tag, Color.Lime)
                    });
                }
            }
        }

        private void NoteTag(string tag, ChatUser chatUser, string currentChannel, ChatEventType action)
        {
            if (string.IsNullOrEmpty(tag))
                return;

            string act = "joining the channel";
            switch (action)
            {
                case ChatEventType.UserInChannel:
                    act = "being seen in the channel";
                    break;
                case ChatEventType.UserFlagsChanged:
                    act = "having flags updated in the channel";
                    break;
                case ChatEventType.UserLeftChannel:
                    act = "leaving the channel";
                    break;
                case ChatEventType.UserJoinedChannel:
                    act = "joining the channel";
                    break;
            }
            using (ClansDataContext dc = ClansDataContext.Create(m_dbPath))
            {
                if (dc.Clans.Where(c => c.Tag.ToUpper() == tag.ToUpper()).Count() == 0)
                {
                    Clan c = new Clan
                    {
                        GatewayID = m_gatewayID,
                        Tag = tag,
                        DiscoverySource = string.Format("{0}, a Warcraft 3 user in {1}, {2}.", chatUser.Username, currentChannel, act)
                    };
                    dc.Clans.InsertOnSubmit(c);

                    try
                    {
                        dc.SubmitChanges();
                        m_client.MainWindow.AddChat(new ChatNode[] { 
                            new ChatNode("JinxBot Clan Gnome added ", Color.DarkRed),
                            new ChatNode(c.DiscoverySource, Color.Yellow)
                        });
                    }
                    catch (Exception ex)
                    {
                        m_client.MainWindow.AddChat(new ChatNode[] { 
                            new ChatNode("JinxBot Clan Gnome error ", Color.DarkRed),
                            new ChatNode("Error saving a new clan:", Color.Red),
                            ChatNode.NewLine,
                            new ChatNode(ex.ToString(), Color.OrangeRed)
                    });
                    }
                }
                else
                {
                    m_client.MainWindow.AddChat(new ChatNode[] { 
                        new ChatNode("JinxBot Clan Gnome note: ", Color.DarkRed),
                        new ChatNode("Already noted clan ", Color.Yellow),
                        new ChatNode(tag, Color.Lime)
                    });
                }
            }
        }

        private void HandleConnected(object sender, EventArgs e)
        {
            m_timer.Start();
        }

        private void HandleDisconnected(object sender, EventArgs e)
        {
            m_timer.Stop();
        }
    }
}
