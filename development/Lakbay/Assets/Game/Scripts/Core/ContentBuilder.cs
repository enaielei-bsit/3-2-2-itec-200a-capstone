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
    [ExecuteInEditMode]
    public class ContentBuilder : Widget {
        protected string _recentContent;

        public GameObject root;
        public Content content;
        public List<ContentEntryHandler> entryHandlers =
            new List<ContentEntryHandler>();

        public override void Update() {
            base.Update();
            string scontent = content.ToString();
            if(_recentContent != scontent) {
                Build();
                _recentContent = scontent;
            }
        }

        [ContextMenu("Build")]
        public virtual void Build() {
            if(Application.isPlaying) gameObject.DestroyChildren();
            else gameObject.DestroyChildrenImmediately();
            foreach(var entry in content.entries) {
                foreach(var handler in entryHandlers) {
                    handler.OnBuild(this, entry);
                }
            }
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