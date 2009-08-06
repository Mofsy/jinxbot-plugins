using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNSharp.BattleNet;
using JinxBot.Plugins.Data;
using BNSharp;

namespace JinxBot.Plugins.Moderation.JinxMod
{
    [JinxBotPlugin(Author = "MyndFyre and Newby", 
        Name = "JinxMod - a JinxBot Take on Moderation", 
        Url = "http://www.jinxbot.net/wiki/JinxMod",
        Version = "0.0.0.0",
        Description = "Supports basic channel moderation.")]
    public class JinxMod : ISingleClientPlugin, ICommandHandler
    {
        private IJinxBotClient m_client;
        private BattleNetClient m_bnet;
        private IJinxBotDatabase m_db;

        #region ISingleClientPlugin Members

        public void CreatePluginWindows(JinxBot.Plugins.UI.IProfileDocument profileDocument)
        {
            // no windows necessary
        }

        public void DestroyPluginWindows(JinxBot.Plugins.UI.IProfileDocument profileDocument)
        {
            
        }

        public void RegisterEvents(IJinxBotClient profileClient)
        {
            m_client = profileClient;
            m_bnet = profileClient.Client;
            m_db = profileClient.Database;

            m_bnet.RegisterUserJoinedNotification(Priority.High, user_Joined);
            m_bnet.RegisterUserShownNotification(Priority.High, user_Joined);
            m_bnet.RegisterUserFlagsChangedNotification(Priority.High, user_Joined);
            m_bnet.RegisterUserLeftNotification(Priority.High, user_Left);
        }

        public void UnregisterEvents(IJinxBotClient profileClient)
        {
            m_bnet.UnregisterUserJoinedNotification(Priority.High, user_Joined);
            m_bnet.UnregisterUserShownNotification(Priority.High, user_Joined);
            m_bnet.UnregisterUserFlagsChangedNotification(Priority.High, user_Joined);
            m_bnet.UnregisterUserLeftNotification(Priority.High, user_Left);

            m_bnet = null;
            m_client = null;
            m_db = null;
        }

        #endregion

        private void user_Joined(object sender, UserEventArgs e)
        {
            IJinxBotPrincipal user = m_db.FindExact(e.User);
        }

        private void user_Left(object sender, UserEventArgs e)
        {
            IJinxBotPrincipal user = m_db.FindExact(e.User);
        }

        #region IJinxBotPlugin Members

        public void Startup(IDictionary<string, string> settings)
        {
            
        }

        public void Shutdown(IDictionary<string, string> settings)
        {
            
        }

        public object GetSettingsObject()
        {
            return null;
        }

        public IPluginUpdateManifest CheckForUpdates()
        {
            return null;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion

        #region ICommandHandler Members

        public bool HandleCommand(IJinxBotPrincipal commander, string command, string[] parameters)
        {
            // TODO: Fix the ability to determine if I am the owner (that is, if I'm commanding myself)
            if (commander.Username.Equals(m_bnet.UniqueUsername, StringComparison.OrdinalIgnoreCase) || commander.IsInRole("O"))
            {
                switch (command.ToUpper())
                {
                    case "FIND":
                        if (parameters.Length == 1)
                        {
                            Find(parameters[0]);
                            return true;
                        }
                        return false;
                    case "ADDROLE":
                        if (parameters.Length == 2)
                        {
                            AddRole(parameters);
                            return true;
                        }
                        return false;
                    case "REMROLE":
                        if (parameters.Length == 2)
                        {
                            RemoveRole(parameters);
                            return true;
                        }
                        return false;
                    default:
                        return false;
                }
            }

            return false;
        }

        private void Find(string user)
        {
            m_client.SendMessage("Finding " + user + "...");
        }

        private void AddRole(string[] parameters)
        {
            m_client.SendMessage("Pretending to add " + parameters[0] + " to role " + parameters[1] + "...");
        }

        private void RemoveRole(string[] parameters)
        {
            m_client.SendMessage("Pretending to remove " + parameters[0] + " from role " + parameters[1] + "...");
        }

        public IEnumerable<string> GetCommandHelp(IJinxBotPrincipal commander)
        {
            if (commander.Username.Equals(m_bnet.UniqueUsername, StringComparison.OrdinalIgnoreCase) || commander.IsInRole("O"))
            {
                yield return "addrole <user|meta> <role> - Adds the specified role to a user or meta";
                yield return "delrole <user|meta> <role> - Removes the specified role from a user or meta";
                yield return "find <user> - Finds the most recent occurrance of <user>.";
            }
        }

        #endregion
    }
}
