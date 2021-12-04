/*
 * Date Created: Tuesday, November 23, 2021 10:50 AM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    using Core;

    public class RepeaterHandler : Controller, LoadingScreen.IMonitored {
        protected Coroutine _coroutine;
        protected bool _built = false;
        public virtual bool built => _built;

        public int maxCount = 10;
        protected int _count;
        public virtual int count => _count;
        [HideInInspector]
        public int repeated = 0;
        public int maxRepeat = -1;
        public Vector3 offset = Vector3.forward;
        public Transform root;
        public List<Repeater> repeaters = new List<Repeater>();

        public override void Awake() {
            base.Awake();
            if(!root) root = transform;
        }

        public override void OnValidate() {
            base.OnValidate();
            while(repeaters.Contains(null)) repeaters.Remove(null);
        }

        [ContextMenu("Build")]
        public virtual void Build() {
            if(_coroutine == null) {
                _coroutine = StartCoroutine(BuildEnumerator());
            }
        }

        public virtual IEnumerator BuildEnumerator() {
            _built = false;
            if(repeaters.Count > 0 && root) {
                _count = 0;

                for(int i = 0; i < repeaters.Count; i++) {
                    var repeater = repeaters[i];
                    root.DetachChildren();
                    if(!repeater.gameObject.IsExisting()) {
                        repeaters[i] = Instantiate(repeater);
                        _count++;
                        yield return null;
                    }
                }

                if(Application.isPlaying) root.DestroyChildren();
                else root.DestroyChildrenImmediately();

                while(repeaters.Count < maxCount) {
                    repeaters.Add(Instantiate(repeaters.PickRandomly()));
                    _count++;
                    yield return null;
                }

                for(int i = 0; i < repeaters.Count; i++) {
                    var repeater = repeaters[i];
                    repeater.handler = this;
                    repeater.name = $"{repeater.GetType().Name}{i}";
                    Position(
                        root, i == 0 ? null : repeaters[i - 1], repeater, offset);
                }

                _built = true;
            }

            _coroutine = null;
            yield break;
        }

        public static void Position(
            Transform root, Repeater previous, Repeater next, Vector3 offset) {
            if(!root) return;
            var position = previous ? previous.transform.position : root.position;
            next.transform.SetParent(root);
            next.transform.position = position;
            if(previous) next.transform.Translate(offset);
        }

        public virtual void Repeat() {
            if(!built) return;
            if(repeaters.Count > 0) {
                if(maxRepeat > 0) {
                    if(repeated < maxRepeat) repeated++;
                    else return;
                }

                var first = repeaters.Pop(0);
                Repeater last;
                if(repeaters.Count == 1) last = first; 
                else last = repeaters.Last();
                repeaters.Add(first);
                first.transform.position = last.transform.position;
                first.transform.Translate(offset);
                first.OnRepeat();
            }
        }

        public LoadingScreen.MonitorInfo OnMonitor(LoadingScreen loadingScreen) {
            if(!built) {
                return new LoadingScreen.MonitorInfo(
                    "RepeaterHandler",
                    count / maxCount
                );
            }

            return default;
        }
    }
}