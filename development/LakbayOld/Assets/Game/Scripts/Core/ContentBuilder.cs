/*
 * Date Created: Sunday, October 24, 2021 3:46 PM
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
    public class ContentBuilder : Widget {
        [SerializeField]
        protected TextAsset _file;
        public GameObject root;
        public Content content;
        public List<ContentEntryHandler> entryHandlers =
            new List<ContentEntryHandler>();

        public override void Update() {
            base.Update();
        }

        [ContextMenu("Build")]
        public virtual void Build() => Build(content);

        public virtual IEnumerator BuildEnumerator(Content content) {
            if(!content || !root) yield break;
            this.content = content;
            
            if(Application.isPlaying) root.DestroyChildren();
            else root.DestroyChildrenImmediately();

            yield return new WaitForEndOfFrame();
            
            foreach(var entry in content) {
                foreach(var handler in entryHandlers) {
                    handler.OnBuild(this, entry);
                }
            }
        }

        public virtual void Build(Content content) =>
            StartCoroutine(BuildEnumerator(content));

        public virtual void Build(TextAsset content) {
            if(_file && _file.text.Length > 0)
                Build(_file.text.DeserializeAsYaml<Content>());
        }

        [ContextMenu("Build from File")]
        protected virtual void BuildFromFile() => Build(_file);

        public override void Awake() {
            base.Awake();
            if(!root) root = gameObject;
            // Build();
        }

        public override void OnValidate() {
            base.OnValidate();
            if(!root) root = gameObject;
        }
    }
}