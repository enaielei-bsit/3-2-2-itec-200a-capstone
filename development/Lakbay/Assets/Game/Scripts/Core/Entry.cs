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

using CommandLine;
using YamlDotNet.Serialization;

using Utilities;

namespace Ph.CoDe_A.Lakbay.Core {
    [Serializable]
    public class Entry {
        public enum Type {
            Text, Document, Image, Audio, Video
        }

        [SerializeField]
        protected Type _type;
        [Option('t', "type")]
        public virtual Type type {
            get => _type;
            set => _type = value;
        }
        [SerializeField]
        protected string _value = "";
        [Value(0)]
        public virtual string value {
            get => _value;
            set => _value = value;
        }
        [YamlIgnore]
        public virtual string raw =>
            Parser.Default.FormatCommandLine(this);

        public Entry() {}
        
        public Entry(Type type, string value) {
            this.type = type;
            this.value = value;
        }

        public virtual T GetAsset<T>(Database database)
            where T : UnityEngine.Object {
            return database?.Get<T>(value);
        }

        public virtual T GetAsset<T>() 
            where T : UnityEngine.Object => GetAsset<T>(Session.database);

        public static Entry Parse(string str) {
            Entry entry = default;
            Parser.Default.ParseArguments<Entry>(str.ToCommandLine())
                .WithParsed((e) => entry = e);
            return entry;
        }

        public static implicit operator string(Entry entry) {
            return entry.raw;
        }

        public static implicit operator Entry(string str) {
            return Parse(str);
        }

        public override string ToString() {
            return raw;
        }
    }
}