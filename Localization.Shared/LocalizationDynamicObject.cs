using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Text;

namespace Localization.Shared
{
    public class LocalizationDynamicObject : DynamicObject, IDictionary<string, object>
    {
        private readonly IDictionary<string, object> dictionary;

        public LocalizationDynamicObject(IDictionary<string, object> obj)
        {
            dictionary = obj;
        }

        public object this[string key]
        {
            get
            {
                object result;
                dictionary.TryGetValue(key, out result);
                return Wrap(result);
            }
            set { dictionary[key] = Unwrap(value); }
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The compiler generates calls to invoke this")]
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "The compiler generates calls to invoke this")]
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }

        public static object Wrap(object value)
        {
            var obj = value as IDictionary<string, object>;
            if (obj != null)
            {
                return new LocalizationDynamicObject(obj);
            }

            return value;
        }

        public static object Unwrap(object value)
        {
            var dictWrapper = value as LocalizationDynamicObject;
            if (dictWrapper != null)
            {
                return dictWrapper.dictionary;
            }

            return value;
        }

        public void Add(string key, object value)
        {
            dictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return dictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return dictionary.Values; }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            dictionary.Add(item);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return dictionary.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return dictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}