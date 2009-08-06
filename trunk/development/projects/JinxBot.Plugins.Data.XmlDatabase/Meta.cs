using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace JinxBot.Plugins.Data.XmlDatabase
{
    public class Meta : IJinxBotMeta
    {
        private List<string> m_roles;
        private string m_matchRegex;

        internal Meta(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException("input");

            InputString = input;
            
            Matcher = MatchUtility.CreateMetaMatch(input, out m_matchRegex);
            
            m_roles = new List<string>();
        }

        internal Meta(XElement element)
        {
            Debug.Assert(element != null);

            InputString = element.Attribute("InputString").Value;
            m_matchRegex = element.Attribute("Match").Value;
            Matcher = new Regex(m_matchRegex, RegexOptions.IgnoreCase);

            m_roles = (from r in element.Elements("Roles").Elements("Add").Attributes("Role")
                       select r.Value).ToList();
        }

        internal XElement Serialize()
        {
            return new XElement("Meta",
                new XAttribute("InputString", InputString), new XAttribute("Match", m_matchRegex),
                new XElement("Roles",
                    from r in m_roles
                    select new XElement("Add", new XAttribute("Role", r))
                    )
                );
        }

        public string InputString
        {
            get;
            private set;
        }

        public Regex Matcher
        {
            get;
            private set;
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

        public bool IsInRole(string roleName)
        {
            return m_roles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
        }

        #region IJinxBotMeta Members

        public IEnumerable<string> Roles
        {
            get { return new ReadOnlyCollection<string>(m_roles); }
        }

        #endregion
    }
}
