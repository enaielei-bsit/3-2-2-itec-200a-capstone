/*
 * Date Created: Sunday, October 24, 2021 3:08 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using YamlDotNet.Serialization;

using Utilities;

namespace Ph.CoDe_A.Lakbay.Core {
    [Serializable]
    public class Entry {
        public enum Type {
            Text, Document, Image, Audio, Video
        }

        public Type type;
        public string value = "";
        [YamlIgnore]
        public virtual string raw => $"{type}:{value}";

        public Entry() {}

        public Entry(string str) {
            Parse(str);
        }

        public Entry(Type type, string value) {
            this.type = type;
            this.value = value;
        }

        public virtual void Parse(string str) {
            var parts = str.Split(':').ToList();
            if(parts.Count > 1) {
                if(Enum.TryParse(parts[0], out type)) {
                    parts.Pop();
                }
            }
            value = parts.Join(":");
        }

        public static implicit operator string(Entry entry) {
            return entry.raw;
        }

        public static implicit operator Entry(string str) {
            return new Entry(str);
        }
    }

    [Serializable]
    public class Content : IList<Entry> {
        [SerializeField]
        protected List<Entry> _entries = new List<Entry>();

        public Entry this[int index] {
            get => _entries[index];
            set => _entries[index] = value;
        }

        public int Count => _entries.Count;

        public bool IsReadOnly => false;

        public Content(params Entry[] entries) {
            _entries.AddRange(entries);
        }

        public void Add(Entry item) => _entries.Add(item);

        public void Clear() => _entries.Clear();

        public bool Contains(Entry item) => _entries.Contains(item);

        public void CopyTo(Entry[] array, int arrayIndex) =>
            _entries.CopyTo(array, arrayIndex);

        public IEnumerator<Entry> GetEnumerator() {
            return _entries.GetEnumerator();
        }

        public int IndexOf(Entry item) {
            return _entries.IndexOf(item);
        }

        public void Insert(int index, Entry item) =>
            _entries.Insert(index, item);

        public bool Remove(Entry item) => _entries.Remove(item);

        public void RemoveAt(int index) => _entries.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() {
            return (_entries as IEnumerable).GetEnumerator();
        }

        public static implicit operator bool(Content content) {
            return content._entries != null;
        }
    }
}