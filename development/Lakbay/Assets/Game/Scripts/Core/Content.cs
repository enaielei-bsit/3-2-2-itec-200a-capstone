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

using Utilities;

namespace Ph.CoDe_A.Lakbay.Core {
    [Serializable]
    public class Entry {
        public enum Type {
            Text, Document, Image, Audio, Video
        }

        public Type type;
        public string value = "";
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
    }

    [CreateAssetMenu(
        fileName="ContentData",
        menuName="Game/Core/ContentData"
    )]
    public class Content : ScriptableObject {
        [SerializeField]
        protected TextAsset _file;

        public List<Entry> entries = new List<Entry>();

        public virtual void Load(string str) {
            if(str == null || str.Length == 0) return;
            this.entries.Clear();
            var entries = str.DeserializeAsYaml<List<string>>();
            foreach(var entry in entries) {
                this.entries.Add(new Entry(entry));
            }
        }

        public virtual void Load(TextAsset file) => Load(file?.ToString());

        [ContextMenu("Load")]
        public virtual void Load() => Load(_file);

        public override string ToString() =>
            entries.Select((e) => e.raw).Join("\n");
    }
}