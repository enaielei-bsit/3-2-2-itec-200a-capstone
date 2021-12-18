/*
 * Date Created: Saturday, December 18, 2021 4:34 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utilities {
    [ExecuteInEditMode]
    public class Gizmo : MonoBehaviour {
        [Serializable]
        public struct Alpha {
            public float min;
            public float max;

            public Alpha(float min, float max) {
                this.min = min;
                this.max = max;
            }
        }

        public bool active = true;
        [Tooltip("If on alpha.min is reached the closer the camera is.")]
        public bool invert = false;
        [Tooltip("The maximum distance needed to reach alpha.min.")]
        public float maxDistance = 10.0f;
        public Alpha alpha = new Alpha(0.15f, 1.0f);
        public new Camera camera;
        public SpriteRenderer sprite;

        public virtual void Update() {
            if(active && sprite && camera) {
                float distance = Mathf.Abs(
                    Vector3.Distance(
                        transform.position, camera.transform.position));
                float percentage = distance / maxDistance;
                percentage = !invert ? 1 - percentage : percentage;
                percentage = Mathf.Clamp(percentage, alpha.min, alpha.max);
                var color = sprite.color;
                color.a = percentage;
                sprite.color = color;
            }
        }
    }
}