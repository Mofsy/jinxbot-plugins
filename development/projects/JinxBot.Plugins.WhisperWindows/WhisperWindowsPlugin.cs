using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinxBot.Plugins.UI;
using System.Windows.Forms;
using BNSharp;
using System.ComponentModel;
using System.Diagnostics;

namespace JinxBot.Plugins.WhisperWindows
{
    [JinxBotPlugin(
        Author = "MyndFyre",
        Name = "JinxBot Whisper Windows Plugin",
        Url = "http://www.jinxbot.net/wiki/Whisper_Window_Plugin",
        Version = "1.0.0.0",
        Description = "Pops up whisper windows for whisper conversations.")]
    internal class WhisperWindowsPlugin : ISingleClientPlugin
    {
        private delegate void Invokee();

        private IJinxBotClient m_client;
        private IProfileDocument m_profileDoc;
        private Dictionary<string, WhisperTab> m_forms = new Dictionary<string, WhisperTab>();
        
        #region ISingleClientPlugin Members

        public void CreatePluginWindows(JinxBot.Plugins.UI.IProfileDocument profileDocument)
        {
            m_profileDoc = profileDocument;
        }

        public void DestroyPluginWindows(JinxBot.Plugins.UI.IProfileDocument profileDocument)
        {
            lock (m_forms)
            {
                foreach (string key in m_forms.Keys)
                {
                    WhisperTab tab = m_forms[key];
                    tab.Close();
                    tab.Dispose();
                }
                m_forms.Clear();
            }
        }

        public void RegisterEvents(IJinxBotClient profileClient)
        {
            m_client = profileClient;

            m_client.Client.RegisterWhisperReceivedNotification(Priority.Low, client_WhisperReceived);
            m_client.Client.RegisterWhisperSentNotification(Priority.Low, client_WhisperSent);
        }

        public void UnregisterEvents(IJinxBotClient profileClient)
        {
            m_client.Client.UnregisterWhisperReceivedNotification(Priority.Low, client_WhisperReceived);
            m_client.Client.UnregisterWhisperSentNotification(Priority.Low, client_WhisperSent);
        }

        #endregion

        private void client_WhisperReceived(object sender, ChatMessageEventArgs e)
        {
            Invokee del = delegate
            {
                lock (m_forms)
                {
                    WhisperTab tab = null;
                    if (!m_forms.ContainsKey(e.Username))
                    {
                        tab = new WhisperTab(m_client);
                        tab.OtherPerson = e.Username;
                        tab.MyUsername = m_client.Client.UniqueUsername;
                        m_profileDoc.AddDocument(tab);
                        tab.TabText = tab.Text = e.Username;
                        tab.Disposed += tab_Disposed;


                        m_forms.Add(e.Username, tab);
                    }
                    else
                    {
                        tab = m_forms[e.Username];
                    }

                    tab.AddReceived(e);
                }
            };
            ISynchronizeInvoke sync = m_profileDoc as ISynchronizeInvoke;
            if (sync.InvokeRequired)
                sync.BeginInvoke(del, new object[0]);
            else
                del();
        }

        private void tab_Disposed(object sender, EventArgs e)
        {
            lock (m_forms)
            {
                WhisperTab tab = (WhisperTab)sender;
                if (m_forms.ContainsKey(tab.OtherPerson))
                {
                    m_forms.Remove(tab.OtherPerson);
                }
                tab.Disposed -= tab_Disposed;
            }
        }

        private void client_WhisperSent(object sender, ChatMessageEventArgs e)
        {
            Invokee del = delegate
            {
                lock (m_forms)
                {
                    WhisperTab tab = null;
                    if (!m_forms.ContainsKey(e.Username))
                    {
                        tab = new WhisperTab(m_client);
                        tab.OtherPerson = e.Username;
                        tab.MyUsername = m_client.Client.UniqueUsername;
                        m_profileDoc.AddDocument(tab);
                        tab.TabText = tab.Text = e.Username;
                        tab.Disposed += tab_Disposed;

                        m_forms.Add(e.Username, tab);
                    }
                    else
                    {
                        tab = m_forms[e.Username];
                    }

                    tab.AddSent(e);
                }
            };
            ISynchronizeInvoke sync = m_profileDoc as ISynchronizeInvoke;
            if (sync.InvokeRequired)
                sync.BeginInvoke(del, new object[0]);
            else
                del();
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
    }
}
