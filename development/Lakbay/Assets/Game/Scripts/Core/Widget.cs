/*
 * Date Created: Sunday, October 10, 2021 8:08 AM
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
    public abstract class Widget : Base {
    }

    public class Widget<T> : Widget where T : Component {
        public T component;

        public override void Awake() {
            base.Awake();
            if(!component) component = GetComponentInChildren<T>();
        }

        public override void OnValidate() {
            base.OnValidate();
            if(!component) component = GetComponentInChildren<T>();
        }
    }
}