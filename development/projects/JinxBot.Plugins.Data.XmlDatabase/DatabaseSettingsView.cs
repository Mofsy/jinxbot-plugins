using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinxBot.Controls.Design;
using System.ComponentModel;
using JinxBot.Plugins.Data.XmlDatabase.Design;
using System.Drawing.Design;

namespace JinxBot.Plugins.Data.XmlDatabase
{
    internal class DatabaseSettingsView
    {
        private DatabaseSettings m_settings;

        public DatabaseSettingsView(DatabaseSettings settings)
        {
            m_settings = settings;
        }

        [Name("XML File Path")]
        [Category("XML Database")]
        [Description("Specifies the location of the database file.  This typically doesn't need to change.")]
        [Browsable(true)]
        [Editor(typeof(XmlFileBrowserTypeEditor), typeof(UITypeEditor))]
        public string FilePath
        {
            get { return m_settings.FilePath; }
            set { m_settings.FilePath = value; }
        }
    }
}
