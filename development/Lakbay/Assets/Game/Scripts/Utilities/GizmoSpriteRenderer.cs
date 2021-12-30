/*
 * Date Created: Thursday, December 30, 2021 2:15 PM
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

namespace Utilities {
    [RequireComponent(typeof(SpriteRenderer))]
    public class GizmoSpriteRenderer : MonoBehaviour, IGizmoObject {
        public virtual SpriteRenderer sprite => GetComponent<SpriteRenderer>();

        public void SetValue(float value) {
            var color = sprite.color;
            color.a = value;
            sprite.color = color;
        }
    }
}