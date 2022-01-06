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
    public abstract class EntryValue {

    }

    [Serializable]
    public class EntryText : EntryValue {
        public string value = "";

        public EntryText() {}

        public EntryText(string value="") {
            this.value = value;
        }

        public static implicit operator EntryText(string str) {
            return new EntryText(str);
        }

        public override string ToString() {
            return value;
        }
    }

    [Serializable]
    public abstract class EntryAsset<T> : EntryValue
        where T : UnityEngine.Object {
        public string path = "";

        public EntryAsset() {}

        public EntryAsset(string path="") {
            this.path = path;
        }

        public virtual T Get(Database database) {
            return database?.Get<T>(path);
        }

        public virtual T Get() => Get(Session.database);

        public override string ToString() {
            return path;
        }
    }

    [Serializable]
    public class EntryImage : EntryAsset<Sprite> {
        public string description = "";
        public string source = "";

        public EntryImage() {}

        public EntryImage(string path="", string description="", string source="")
            : base(path) {
            this.description = description;
            this.source = source;
        }

        public static implicit operator EntryImage(string path) {
            return new EntryImage(path);
        }

        public override string ToString() {
            return new string[] {
                base.ToString(), description, source
            }.Join(";");
        }
    }

    // [Serializable]
    // public class EntryVideo : EntryAsset<> {
    //     public string path = "";
    // }

    [Serializable]
    public class Entry {
        public enum Type {
            Text, Document, Image, Audio, Video
        }

        [YamlIgnore]
        public virtual Type type {
            get {
                if(text != null) return Type.Text;
                else if(image != null) return Type.Image;
                return Type.Text;
            }
            set {
                if(value == Type.Text) {
                    _text = "";
                    _image = null;
                } else if(value == Type.Image) {
                    _text = null;
                    _image = "";
                }
            }
        }

        protected EntryText _text;
        public EntryText text {
            get => _text;
            set {
                type = Type.Text;
                _text = value;
            }
        }
        protected EntryImage _image;
        public EntryImage image {
            get => _image;
            set {
                type = Type.Image;
                _image = value;
            }
        }

        public Entry() : this(Type.Text) {}

        public Entry(Type type=Type.Text) {
            this.type = type;
        }

        public Entry(string text) : this((EntryText) text) {}

        public Entry(EntryText text) : this(Type.Text) {
            this.text = text;
        }

        public Entry(EntryImage image) : this(Type.Image) {
            this.image = image;
        }
        
        public virtual T GetAsset<T>(Database database)
            where T : UnityEngine.Object {
            if(type == Type.Image) return image?.Get(database) as T;
            return default;
        }

        public virtual T GetAsset<T>() 
            where T : UnityEngine.Object => GetAsset<T>(Session.database);

        public static implicit operator Entry(string str) {
            return new Entry(str);
        }

        public override string ToString() {
            if(type == Type.Text) return text?.ToString();
            else if(type == Type.Image) return image?.ToString();
            return text?.ToString();
        }
    }
}