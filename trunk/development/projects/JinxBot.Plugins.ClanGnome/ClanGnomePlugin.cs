using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace JinxBot.Plugins.ClanGnome
{
    [JinxBotPlugin(Author = "MyndFyre", Description = "Gnomes clan channels.", Name = "Clan Gnome", Url = "http://www.jinxbot.net", Version = "1.0.0.0")]
    public class ClanGnomePlugin : IJinxBotPlugin, ISingleClientPlugin
    {
        private const string DB_PATH_KEY = "ClanGnomeDatabasePath";

        public ClanGnomePlugin()
        {

        }

        private string m_dbPath;
        private ClanGnome m_cg;

        #region IJinxBotPlugin Members

        public void Startup(IDictionary<string, string> settings)
        {
            if (!settings.ContainsKey(DB_PATH_KEY))
            {
                string appDataPath = JinxBot.Configuration.JinxBotConfiguration.ApplicationDataPath;
                string db = Guid.NewGuid().ToString() + ".sdf";
                settings.Add(DB_PATH_KEY, Path.Combine(appDataPath, db));
            }

            string dbPath = settings[DB_PATH_KEY];
            if (!File.Exists(dbPath))
            {
                File.WriteAllBytes(dbPath, Resources.ClansMaster);
            }
            else
            {
                try
                {
                    using (ClansDataContext c = ClansDataContext.CreateReadOnly(dbPath))
                    {
                        Trace.WriteLine(c.Gateways.Count());
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException("The database at the database path has been corrupted.", ex);
                }
            }

            m_dbPath = dbPath;
        }

        public void Shutdown(IDictionary<string, string> settings)
        {
            settings[DB_PATH_KEY] = m_dbPath;
        }

        public object GetSettingsObject()
        {
            return new object();
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

        #region ISingleClientPlugin Members

        public void CreatePluginWindows(JinxBot.Plugins.UI.IProfileDocument profileDocument)
        {
            
        }

        public void DestroyPluginWindows(JinxBot.Plugins.UI.IProfileDocument profileDocument)
        {
            
        }

        public void RegisterEvents(IJinxBotClient profileClient)
        {
            m_cg = new ClanGnome(m_dbPath, profileClient);
        }

        public void UnregisterEvents(IJinxBotClient profileClient)
        {
            if (m_cg != null)
            {
                m_cg.Unhook();
                m_cg = null;
            }
        }

        #endregion
    }
}
