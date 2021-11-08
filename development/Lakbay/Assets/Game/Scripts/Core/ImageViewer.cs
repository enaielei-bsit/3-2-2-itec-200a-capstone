/*
 * Date Created: Monday, November 8, 2021 2:28 PM
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
    public class ImageViewer : Widget<Image> {
        public TextWidget caption;

        public virtual void Show(Sprite sprite) => Show(sprite, null);

        public virtual void Show(Sprite sprite, string caption) {
            if(!sprite || !component) return;
            gameObject.SetActive(true);
            component.sprite = sprite;
            if(caption != null || caption.Length > 0) {
                this.caption?.gameObject.SetActive(true);
                this.caption?.component?.SetText(caption);
            } else this.caption?.gameObject.SetActive(false);
        }

        public virtual void Hide() {
            gameObject.SetActive(false);
        }
    }
}