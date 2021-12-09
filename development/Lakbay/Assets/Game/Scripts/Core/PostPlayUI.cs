/*
 * Date Created: Monday, December 6, 2021 12:04 PM
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
    using Pixelplacement;

    public class PostPlayUI : Controller {
        public CanvasGroup tweenable;

        public virtual void Show() {
            gameObject.SetActive(true);
            if(tweenable) {
                Tween.CanvasGroupAlpha(
                    tweenable, 0.0f, 1.0f, 1.0f, 0.0f
                );
            }
        }

        public virtual void Hide() {
            gameObject.SetActive(false);
        }
    }
}