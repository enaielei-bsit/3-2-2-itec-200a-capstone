/*
 * Date Created: Tuesday, December 7, 2021 7:05 PM
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
    using Utilities;
    using TMPro;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Components;

    public class PrePlayUI : Controller {
        public LocalizeStringEvent header; 
        public LocalizeStringEvent subheader;

        public override void Awake() {
            base.Awake();
        }

        public virtual void Show(
            IEnumerable<object> headerArgs,
            IEnumerable<object> subheaderArgs) {
            header?.StringReference.Set(headerArgs);
            subheader?.StringReference.Set(subheaderArgs);
            gameObject.SetActive(true);
        }

        public virtual void Hide() => gameObject.SetActive(false);
    }
}