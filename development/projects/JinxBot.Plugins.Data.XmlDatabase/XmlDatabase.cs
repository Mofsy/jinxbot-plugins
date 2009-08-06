using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace JinxBot.Plugins.Data.XmlDatabase
{
    internal class XmlDatabase : IJinxBotDatabase
    {
        private string m_path;
        private List<User> m_users;
        private List<Role> m_roles;
        private List<Meta> m_metas;

        private string m_defaultGateway;
        private bool m_isDiablo2;

        public void Load(TextReader reader)
        {
            XElement root = XElement.Load(reader);
            XElement rootNode = root;
            m_users = (from u in rootNode.Element("Users").Elements("User")
                       select new User(u)).ToList();
            m_roles = (from r in rootNode.Element("Roles").Elements("Role")
                       select new Role(r)).ToList();
            m_metas = (from m in rootNode.Element("Metas").Elements("Meta")
                       select new Meta(m)).ToList();
        }

        public void Save(TextWriter writer)
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("JinxBotDatabase", new XAttribute("Version", "1.0"), new XAttribute("Provider", "JinxBot.Plugins.Data.Xml.XmlDatabase, JinxBot.Plugins.Data.Xml, Version=1.0.0.0"),
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
        }

        public IEnumerable<IJinxBotPrincipal> FindUsers(string matchPattern)
        {
            return null;
        }

        public IJinxBotPrincipal FindExact(BNSharp.BattleNet.ChatUser user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IJinxBotPrincipal> FindUsersInRole(string role)
        {
            throw new NotImplementedException();
        }

        public void AddUserToRole(IJinxBotPrincipal user, string role)
        {
            throw new NotImplementedException();
        }

        public void RemoveRoleFromUser(IJinxBotPrincipal user, string role)
        {
            throw new NotImplementedException();
        }

        public void AddRoleToMeta(string matchPattern, string role)
        {
            throw new NotImplementedException();
        }

        public void RemoveRoleFromMeta(string matchPattern, string role)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IJinxBotRole> DefinedRoles
        {
            get { throw new NotImplementedException(); }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IJinxBotPlugin Members

        public void Startup(IDictionary<string, string> settings)
        {
            throw new NotImplementedException();
        }

        public void Shutdown(IDictionary<string, string> settings)
        {
            throw new NotImplementedException();
        }

        public object GetSettingsObject()
        {
            throw new NotImplementedException();
        }

        public IPluginUpdateManifest CheckForUpdates()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
