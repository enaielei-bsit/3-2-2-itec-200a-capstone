/*
 * Date Created: Friday, October 8, 2021 9:03 AM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using TMPro;

using Utilities;

namespace Ph.CoDe_A.Lakbay.Core {
    public class LoadingScreen : Initializer {
        public Slider progress;
        public TextMeshProUGUI details;
        public TextMeshProUGUI percentage;

        public override void Initialize() {
            base.Initialize();
            Game.loadingScreen = this;
            gameObject.EnsureComponent<CanvasGroup>();
            Hide();
            _initialized = true;
        }

        public virtual void Show(
            float progress=0.0f,
            string details=""
        ) {
            GetComponent<CanvasGroup>().alpha = 1.0f;
            if(this.progress) {
                this.progress.value = progress;
                this.progress.minValue = 0.0f;
                this.progress.maxValue = 1.0f;
            }

            this.details?.SetText(details);
            this.percentage?.SetText(this.progress.value.ToString("P2"));
        }

        public virtual void Hide() {
            GetComponent<CanvasGroup>().alpha = 0.0f;
        }
    }
}