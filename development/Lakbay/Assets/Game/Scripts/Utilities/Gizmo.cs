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

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utilities {
    // [ExecuteInEditMode]
    public class Gizmo : MonoBehaviour {
        [Serializable]
        public struct Value {
            public float min;
            public float max;

            public Value(float min, float max) {
                this.min = min;
                this.max = max;
            }
        }

        public bool active = true;
        [Tooltip("If on alpha.min is reached the closer the camera is.")]
        public bool invert = false;
        [Tooltip("The maximum distance needed to reach value.min.")]
        public float maxDistance = 10.0f;
        public Value value = new Value(0.15f, 1.0f);
        public new Camera camera;
        public UnityEvent<float> onValueChange = new UnityEvent<float>();

        public virtual void Update() {
            if(active && onValueChange != null && camera) {
                float distance = Mathf.Abs(
                    Vector3.Distance(
                        transform.position, camera.transform.position));
                float percentage = distance / maxDistance;
                percentage = !invert ? 1 - percentage : percentage;
                percentage = Mathf.Clamp(percentage, value.min, value.max);

                onValueChange.Invoke(percentage);
            }
        }
    }
}