/*
 * Date Created: Monday, November 22, 2021 8:32 AM
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
    public abstract class Viewer<T0, T1> : Controller where T0 : Component {
        public T0 component;

        public virtual void Show(T1 value) {
            gameObject.SetActive(true);
        }

        public virtual void Hide() => gameObject.SetActive(false);
    }
}