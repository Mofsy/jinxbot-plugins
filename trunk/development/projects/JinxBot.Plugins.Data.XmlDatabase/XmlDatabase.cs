using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;
using BNSharp.BattleNet;

namespace JinxBot.Plugins.Data.XmlDatabase
{
    [JinxBotPlugin(
        Author = "MyndFyre",
        Name = "JinxBot XML Database",
        Description = "Provides a database to JinxBot in a fairly straightforward XML file.",
        Version = "0.1.0.0",
        Url = "http://www.jinxbot.net/wiki/Databases"
        )]
    internal class XmlDatabase : IJinxBotDatabase
    {
        private const string DIABLO_2_BASED = @"\A(?:(?<charName>[^*#@\s]*)\*)?(?<accountName>[^#\s]+?)(?:#(?<instance>\d{1,9}))?(?:@(?<gateway>USEast|USWest|Asia|Europe|Azeroth|Lordaeron|Kalimdor|Northrend|Blizzard))?(?:#(?<instance>\d{1,9}))?\Z";
        private const string OTHER_CLIENT_BASED = @"\A(?<accountName>[^#\s]+?)(?:#(?<instance>\d{1,9}))?(?:@(?<gateway>USEast|USWest|Asia|Europe|Azeroth|Lordaeron|Kalimdor|Northrend|Blizzard))?(?:#(?<instance>\d{1,9}))?\Z";

        private List<User> m_users;
        private List<Role> m_roles;
        private List<Meta> m_metas;

        private string m_defaultGateway;
        private bool m_isDiablo2;
        private DatabaseSettings m_settings;

        private Regex m_separator;

        public void Load(TextReader reader)
        {
            XElement root = XElement.Load(reader);
            XElement rootNode = root;
            m_users = (from u in rootNode.Element("Users").Elements("User")
                       select new User(u, this)).ToList();
            m_roles = (from r in rootNode.Element("Roles").Elements("Role")
                       select new Role(r)).ToList();
            m_metas = (from m in rootNode.Element("Metas").Elements("Meta")
                       select new Meta(m)).ToList();
        }

        public void Save(TextWriter writer)
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("JinxBotDatabase", new XAttribute("Version", "1.0"), new XAttribute("Provider", GetType().AssemblyQualifiedName),
                    new XElement("Users",
                        from u in m_users
                        select u.Serialize()
                        ),
                    new XElement("Roles",
                        from r in m_roles
                        select r.Serialize()
                        ),
                    new XComment("WARNING!  Do not modify the meta match patterns generated here unless you "),
                    new XComment("know what you are doing!  These are specific types of regular expressions "),
                    new XComment("that do not match typical JinxBot syntax.  The XML database provider does "),
                    new XComment("validate loaded meta matching or regenerate the match patterns when loading "),
                    new XComment("them from disk.  So, modify them at your own risk!                        "),
                    new XElement("Metas",
                        from m in m_metas
                        select m.Serialize()
                        )
                    )
                );
            doc.Save(writer);
        }

        internal bool IsRoleOverridden(string role, IEnumerable<string> userRoleCollection)
        {
            var rolesThatOverrideThisRole = from r in m_roles
                                            where r.Overrides.Contains(role, StringComparer.OrdinalIgnoreCase)
                                            select r;
            var userRoleThatOverrides = (from r in rolesThatOverrideThisRole
                                         where userRoleCollection.Contains(r.Name, StringComparer.OrdinalIgnoreCase)
                                         select r).FirstOrDefault();
            return userRoleThatOverrides != null;
        }

        #region IJinxBotDatabase Members

        public void InitializeConnection(string defaultNamespace, bool isDiablo2)
        {
            m_defaultGateway = defaultNamespace;
            m_isDiablo2 = isDiablo2;
            m_separator = new Regex(isDiablo2 ? DIABLO_2_BASED : OTHER_CLIENT_BASED, RegexOptions.IgnoreCase);
        }

        public IEnumerable<IJinxBotPrincipal> FindUsers(string matchPattern)
        {
            string pattern;
            Regex test = MatchUtility.CreateMetaMatch(matchPattern, out pattern);

            return from u in m_users
                   where test.IsMatch(u.AccountName)
                   select u as IJinxBotPrincipal;
        }

        public IJinxBotPrincipal FindExact(ChatUser user)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();

            string accountName, gateway;
            Match m = m_separator.Match(user.Username);
            if (!m.Success)
                throw new InvalidDataException("Could not match the user's username into appropriate parts.");

            accountName = m.Groups["accountName"].Value;
            if (m.Groups["gateway"].Success)
                gateway = m.Groups["gateway"].Value;
            else
                gateway = m_defaultGateway;

            User dbUser = (from u in m_users
                           where u.AccountName.Equals(accountName, StringComparison.OrdinalIgnoreCase) &&
                                   u.Gateway.Equals(gateway, StringComparison.OrdinalIgnoreCase)
                           select u).FirstOrDefault();
            if (dbUser == null)
            {
                dbUser = new User(accountName, gateway, this);
                foreach (Meta meta in m_metas)
                {
                    if (meta.Matcher.IsMatch(accountName))
                    {
                        foreach (string role in meta.Roles)
                            dbUser.AddRole(role);
                    }
                }
                m_users.Add(dbUser);
            }

            return dbUser;
        }

        public IEnumerable<IJinxBotPrincipal> FindUsersInRole(string role)
        {
            return from u in m_users
                   where u.IsInRole(role)
                   select u as IJinxBotPrincipal;
        }

        public void AddUserToRole(IJinxBotPrincipal user, string role)
        {
            User u = user as User;
            if (u == null)
                throw new InvalidCastException("Attempted to use a user from a different provider; expected JinxBot.Plugins.Data.XmlDatabase.User");

            u.AddRole(role);
        }

        public void RemoveRoleFromUser(IJinxBotPrincipal user, string role)
        {
            User u = user as User;
            if (u == null)
                throw new InvalidCastException("Attempted to use a user from a different provider; expected JinxBot.Plugins.Data.XmlDatabase.User");

            u.RemoveRole(role);
        }

        public void AddRoleToMeta(string matchPattern, string role)
        {
            Meta m = (from meta in m_metas
                      where meta.InputString == matchPattern
                      select meta).FirstOrDefault();
            if (m == null)
            {
                m = new Meta(matchPattern);
                m_metas.Add(m);
            }
            m.AddRole(role);
        }

        public void RemoveRoleFromMeta(string matchPattern, string role)
        {
            Meta m = (from meta in m_metas
                      where meta.InputString == matchPattern
                      select meta).FirstOrDefault();
            if (m != null)
            {
                m.RemoveRole(role);
            }
        }

        public IEnumerable<IJinxBotRole> DefinedRoles
        {
            get { return m_roles.Select(r => r as IJinxBotRole); }
        }

        public void Clear()
        {
            m_users.Clear();
            m_roles.Clear();
            m_metas.Clear();
        }

        #endregion

        #region IJinxBotPlugin Members

        public void Startup(IDictionary<string, string> settings)
        {
            m_settings = new DatabaseSettings();
            m_settings.FilePath = settings["FilePath"];

            if (File.Exists(m_settings.FilePath))
            {
                using (FileStream fs = new FileStream(m_settings.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader sr = new StreamReader(fs))
                {
                    Load(sr);
                }
            }
            else
            {
                m_users = new List<User>();
                m_roles = new List<Role>();
                m_metas = new List<Meta>();
            }
        }

        public void Shutdown(IDictionary<string, string> settings)
        {
            settings["FilePath"] = m_settings.FilePath;

            using (FileStream fs = new FileStream(m_settings.FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                Save(sw);
            }
        }

        public object GetSettingsObject()
        {
            return new DatabaseSettingsView(m_settings);
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
    }
}
