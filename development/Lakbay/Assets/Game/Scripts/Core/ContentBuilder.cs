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
        protected string _recentContent;

        public GameObject root;
        public Content content;
        public List<ContentEntryHandler> entryHandlers =
            new List<ContentEntryHandler>();

        public override void Update() {
            base.Update();
            Build();
        }

        [ContextMenu("Build")]
        public virtual void Build() => Build(content);

        public virtual void Build(Content content) {
            string scontent = content?.ToString();
            if(_recentContent == scontent) return;

            if(!root || !content) return;
            if(content && content.entries.Count == 0) return;
            if(Application.isPlaying) root.DestroyChildren();
            else root.DestroyChildrenImmediately();
            foreach(var entry in content.entries) {
                foreach(var handler in entryHandlers) {
                    handler.OnBuild(this, entry);
                }
            }

            _recentContent = scontent;
        }

        public override void Awake() {
            base.Awake();
            if(!root) root = gameObject;
        }

        public override void OnValidate() {
            base.OnValidate();
            if(!root) root = gameObject;
        }
    }
}