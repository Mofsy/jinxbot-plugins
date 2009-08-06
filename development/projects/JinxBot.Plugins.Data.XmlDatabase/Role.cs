using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace JinxBot.Plugins.Data.XmlDatabase
{
    public class Role : IJinxBotRole
    {
        private List<string> m_overrides;

        internal Role(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            Name = name;
            m_overrides = new List<string>();
        }

        internal Role(XElement roleRoot)
        {
            Name = roleRoot.Attribute("Name").Value;
            Description = roleRoot.Attribute("Description").Value;
            m_overrides = (from o in roleRoot.Elements("Overrides").Elements("Override").Attributes("Name")
                           select o.Value).ToList();
        }

        internal XElement Serialize()
        {
            return new XElement("Role",
                new XAttribute("Name", Name),
                new XAttribute("Description", Description ?? ""),
                new XElement("Overrides",
                    from o in m_overrides
                    select new XElement("Override", new XAttribute("Name", o))
                    )
                );
        }

        public string Name
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            set;
        }

        public IEnumerable<string> Overrides
        {
            get
            {
                return new ReadOnlyCollection<string>(m_overrides);
            }
        }

        public void AddOverride(string roleNameThatIsOverridden)
        {
            if (!m_overrides.Contains(roleNameThatIsOverridden, StringComparer.OrdinalIgnoreCase))
                m_overrides.Add(roleNameThatIsOverridden);
        }

        public void RemoveOverride(string roleNameThatIsNoLongerOverridden)
        {
            m_overrides.Remove(roleNameThatIsNoLongerOverridden);
        }
    }
}
