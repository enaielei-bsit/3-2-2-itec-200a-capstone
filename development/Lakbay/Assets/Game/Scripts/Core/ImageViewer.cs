/*
 * Date Created: Monday, November 22, 2021 8:25 AM
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
    public class ImageViewer : Viewer<Image, Sprite> {
        public override void Show(Sprite sprite) {
            base.Show(sprite);
            if(component) {
                component.sprite = sprite;
            }
        }
    }
}