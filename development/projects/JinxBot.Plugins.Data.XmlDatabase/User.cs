using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Globalization;

namespace JinxBot.Plugins.Data.XmlDatabase
{
    internal class User : IJinxBotPrincipal
    {
        private XmlDatabase m_owner;
        private List<string> m_roles;

        internal User(string username, string gateway, XmlDatabase databaseProvider)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            if (string.IsNullOrEmpty(gateway))
                throw new ArgumentNullException("gateway");

            AccountName = username;
            Gateway = gateway;
            m_roles = new List<string>();

            m_owner = databaseProvider;
        }

        internal User(XElement element, XmlDatabase databaseProvider)
        {
            Debug.Assert(element != null);

            AccountName = element.Attribute("Name").Value;
            Gateway = element.Attribute("Gateway").Value;
            XAttribute lastSeen = element.Attribute("LastSeen");
            DateTime last;
            if (lastSeen != null && DateTime.TryParse(lastSeen.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out last))
            {
                LastSeen = last;
                LastSeenAs = element.Attribute("LastSeenAs").Value;
                LastSeenProduct = element.Attribute("LastSeenProduct").Value;
            }

            m_roles = (from r in element.Elements("Roles").Elements("Add").Attributes("Role")
                       select r.Value).ToList();

            m_owner = databaseProvider;
        }

        internal XElement Serialize()
        {
            return new XElement("User",
                new XAttribute("Name", AccountName), new XAttribute("Gateway", Gateway), new XAttribute("LastSeen", LastSeen.HasValue ? LastSeen.Value.ToString("r") : "Never"), new XAttribute("LastSeenAs", LastSeenAs ?? ""), new XAttribute("LastSeenProduct", LastSeenProduct ?? ""),
                new XElement("Roles",
                    from r in m_roles
                    select new XElement("Add", new XAttribute("Role", r))
                    )
                );
        }

        public string AccountName
        {
            get;
            private set;
        }

        public string Gateway
        {
            get;
            private set;
        }

        public DateTime? LastSeen
        {
            get;
            set;
        }

        public string LastSeenAs
        {
            get;
            set;
        }

        public string LastSeenProduct
        {
            get;
            set;
        }

        public bool IsInRole(string roleName)
        {
            return m_roles.Contains(roleName, StringComparer.OrdinalIgnoreCase) 
                && !m_owner.IsRoleOverridden(roleName, m_roles);
        }

        public void AddRole(string roleName)
        {
            if (!m_roles.Contains(roleName, StringComparer.OrdinalIgnoreCase))
                m_roles.Add(roleName);
        }

        public void RemoveRole(string roleName)
        {
            m_roles.RemoveAll(s => s.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<string> Roles
        {
            get { return new ReadOnlyCollection<string>(m_roles); }
        }

        #region IJinxBotPrincipal Members

        public string Username
        {
            get { return AccountName; }
        }

        #endregion
    }
}
