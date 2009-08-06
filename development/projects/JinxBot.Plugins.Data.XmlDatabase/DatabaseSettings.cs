using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinxBot.Plugins.Data.XmlDatabase
{
    internal class DatabaseSettings : IDictionary<string, string>
    {
        private Dictionary<string, string> m_actual = new Dictionary<string, string>();

        public DatabaseSettings()
        {
            m_actual["FilePath"] = "";
        }

        public string FilePath
        {
            get { return m_actual["FilePath"]; }
            set { m_actual["FilePath"] = value; }
        }

        #region IDictionary<string,string> Members

        public void Add(string key, string value)
        {
            m_actual.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return m_actual.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return m_actual.Keys; }
        }

        public bool Remove(string key)
        {
            return m_actual.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return m_actual.TryGetValue(key, out value);
        }

        public ICollection<string> Values
        {
            get { return m_actual.Values; }
        }

        public string this[string key]
        {
            get
            {
                return m_actual[key];
            }
            set
            {
                m_actual[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<string,string>> Members

        public void Add(KeyValuePair<string, string> item)
        {
            m_actual.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            m_actual.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)m_actual).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, string>>)m_actual).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_actual.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<string, string>>)m_actual).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)m_actual).Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,string>> Members

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return m_actual.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_actual.GetEnumerator();
        }

        #endregion
    }
}
