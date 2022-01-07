/*
 * Date Created: Friday, January 7, 2022 1:11 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core {
    public class MainMenuUI : Controller {
        public virtual string gameVersion => Application.version;
        public virtual string gameName => Application.productName.ToLower();
        public virtual bool debugBuild => Debug.isDebugBuild;

        public override void Awake() {
            base.Awake();
        }
    }
}