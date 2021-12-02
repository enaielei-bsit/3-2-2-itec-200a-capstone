/*
 * Date Created: Monday, November 22, 2021 2:02 AM
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

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    public class Player : Core.Controller {
        public Travel travel => GetComponentInChildren<Travel>();
        public Slide slide => GetComponentInChildren<Slide>();
        public Buffable buffable => GetComponentInChildren<Buffable>();
        public Caster caster => GetComponentInChildren<Caster>();

        public override void Awake() {
            base.Awake();
            printLog("first");
        }

        public override void Update() {
            base.Update();
        }
    }
}