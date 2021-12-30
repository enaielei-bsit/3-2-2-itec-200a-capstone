/*
 * Date Created: Thursday, December 30, 2021 1:46 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Utilities {
    [RequireComponent(typeof(TextMeshPro))]
    public class GizmoText : MonoBehaviour, IGizmoObject {
        public virtual TextMeshPro text => GetComponent<TextMeshPro>();

        public void SetValue(float value) {
            var color = text.color;
            color.a = value;
            text.color = color;
        }
    }
}