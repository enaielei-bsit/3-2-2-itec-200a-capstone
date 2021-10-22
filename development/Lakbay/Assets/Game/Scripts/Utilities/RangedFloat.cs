/*
 * Date Created: Sunday, October 10, 2021 10:27 AM
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
    [Serializable]
    public struct RangedFloat {
        [SerializeField]
        private float _value;
        [SerializeField]
        private float _min;
        [SerializeField]
        private float _max;

        public float value {
            get => _value;
            set {
                _value = Mathf.Clamp(value, min, max);
            }
        }
        public float min {
            get => _min;
            set {
                _min = Mathf.Min(value, max);
                this.value = this.value;        
            }
        }
        public float max {
            get => _max;
            set {
                _max = Mathf.Max(value, min);
                this.value = this.value;        
            }
        }
        public bool isMin => value == min;
        public bool isMax => value == max;
        public bool set => isMin || isMax;

        public RangedFloat(float value, float min, float max) {
            _value = value;
            _min = min;
            _max = max;

            this.min = this.min;
            this.max = this.max;
            this.value = this.value;
        }

        public RangedFloat(float value, float max) : this(value, 0, max) {}

        public RangedFloat(float value) : this(value, 1) {}
    }
}