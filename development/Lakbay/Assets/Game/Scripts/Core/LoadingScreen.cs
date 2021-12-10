/*
 * Date Created: Friday, December 3, 2021 11:32 PM
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

using Pixelplacement;
using TMPro;

namespace Ph.CoDe_A.Lakbay.Core {
    using Pixelplacement.TweenSystem;
    using Utilities;

    [RequireComponent(typeof(Canvas)), RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : Controller {
        public class MonitorInfo {
            public string text;
            public float progress;

            public MonitorInfo(string text, float progress) {
                this.text = text;
                this.progress = progress;
            }
        }

        public interface IMonitored {
            MonitorInfo OnMonitor(LoadingScreen loadingScreen);
        }

        protected Coroutine _hideDelay;
        protected TweenBase _progressTween;
        protected IMonitored _monitored;
        protected bool _hideOnNull;

        public virtual CanvasGroup group => GetComponent<CanvasGroup>();
        public virtual bool showing {
            get => group.alpha > 0 && group.interactable && group.blocksRaycasts;
            set {
                group.alpha = value ? 1f : 0f;
                group.interactable = value;
                group.blocksRaycasts = value;
            }
        }
        public TextMeshProUGUI text;
        public Slider progress;
        public float hideDelay = 1.0f;

        public override void Awake() {
            base.Awake();
        }

        public virtual void Show() {
            if(!showing) showing = true;
        }

        public virtual void Show(string text, float progress) {
            Show();
            progress = Mathf.Clamp(progress, 0.0f, 1.0f);
            if(this.text) {
               this.text.SetText(text); 
            }

            if(this.progress) {
                if(_progressTween != null) _progressTween.Finish();
                _progressTween = Tween.Value(
                    this.progress.value,
                    progress, (v) => this.progress.value = v, 0.05f, 0.0f);
            }
        }

        public virtual void Hide() {
            if(showing) {
                showing = false;
            }
        }

        public virtual void Hide(float delay) {
            if(delay > 0.0f) {
                Invoke("Hide", delay);
            } else Hide();
        }

        public virtual void Monitor(IMonitored monitored, bool hideOnNull=true) {
            _monitored = monitored;
            _hideOnNull = hideOnNull;
            if(_monitored != null) Show();
        }
        
        public virtual void Monitor(GameObject gameObjec, bool hideOnNull=true) {
            var monitored = gameObject.GetComponents(typeof(IMonitored))
                .Select((m) => m as IMonitored);
            if(monitored != null && monitored.Count() != 0) {
                Monitor(monitored.First());
            }
        }

        public virtual void Unmonitor() => _monitored = null;

        public override void Update() {
            base.Update();
            if(_monitored != null) {
                var info = _monitored.OnMonitor(this);
                if(info != null) {
                    Show(info.text, info.progress);
                } else {
                    Unmonitor();
                    if(_hideOnNull) Hide(hideDelay);
                }
            }
        }
    }
}