/*
 * Date Created: Friday, October 8, 2021 4:05 PM
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

namespace Utilities {
    [System.Serializable]
    public struct RangedInt {
        [SerializeField]
        private int _min;
        public int min {
            get => _min;
            set {
                _min = Mathf.Min(value, max);
                this.value = this.value;
            }
        }
        [SerializeField]
        private int _value;
        public int value {
            get => _value;
            set => _value = Mathf.Clamp(value, min, max);
        }
        [SerializeField]
        private int _max;
        public int max {
            get => _max;
            set {
                _max = Mathf.Max(value, min);
                this.value = this.value;
            }
        }

        public RangedInt(int value, int min, int max) {
            _value = value;
            _min = min;
            _max = max;

            this.min = this.min;
            this.max = this.max;
            this.value = this.value;
        }

        public RangedInt(int value, int max) : this(value, 0, max) {
        }
    }
}