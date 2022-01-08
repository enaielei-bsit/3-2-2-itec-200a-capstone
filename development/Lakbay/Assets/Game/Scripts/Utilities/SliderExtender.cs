/*
 * Date Created: Saturday, January 8, 2022 7:52 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Utilities {
    [RequireComponent(typeof(Slider))]
    [ExecuteInEditMode]
    public class SliderExtender : MonoBehaviour {
        public virtual Slider slider => GetComponent<Slider>();
        [Tooltip("Use {0} to interpolate the actual value.")]
        public bool active = true;
        public string stringFormat = "{0}";
        public UnityEvent<string> onValueChange = new UnityEvent<string>();

        public virtual void OnValueChange(float value) {
            try {
                onValueChange?.Invoke(
                    !string.IsNullOrEmpty(stringFormat)
                    ? string.Format(stringFormat, value) : value.ToString());
            } catch {
                Debug.LogWarning("Something's wrong with the `stringFormat`.");
            }
        }
        
        public virtual void Update() {
            if(slider && active) {
                OnValueChange(slider.value);
            }
        }
    }
}