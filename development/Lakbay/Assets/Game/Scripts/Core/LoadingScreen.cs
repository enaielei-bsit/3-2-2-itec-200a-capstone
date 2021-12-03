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
        protected TweenBase progressTween;

        public virtual bool showing => gameObject.activeSelf;
        public TextMeshProUGUI text;
        public Slider progress;

        public override void Awake() {
            base.Awake();
        }

        public virtual void Show(string text, float progress) {
            progress = Mathf.Clamp(progress, 0.0f, 1.0f);
            if(!gameObject.activeSelf) gameObject.SetActive(true);
            if(this.text) {
               this.text.SetText(text); 
            }

            if(this.progress) {
                if(progressTween != null) progressTween.Finish();
                progressTween = Tween.Value(
                    this.progress.value,
                    progress, (v) => this.progress.value = v, 0.05f, 0.0f);
            }
        }

        public virtual void Hide() {
            if(gameObject.activeSelf) gameObject.SetActive(false);
        }
    }
}