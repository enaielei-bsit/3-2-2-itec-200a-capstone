/*
 * Date Created: Monday, November 8, 2021 3:01 PM
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

namespace Ph.CoDe_A.Lakbay.Core {
    public class InteractableImageWidget : ImageWidget {
        public string caption = "";
        public virtual Button button => GetComponentInChildren<Button>();

        public override void Awake() {
            base.Awake();
            if(button) {
                button.onClick.AddListener(View);
            }
        }

        public virtual void View(ImageViewer viewer) {
            if(!viewer) return;
            viewer.Show(component.sprite, caption);
        }

        public virtual void View() => View(FindObjectOfType<ImageViewer>());
    }
}