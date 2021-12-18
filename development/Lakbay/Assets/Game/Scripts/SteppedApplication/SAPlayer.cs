/*
 * Date Created: Saturday, December 18, 2021 4:32 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © #YEAR# #COMPANY_NAME#. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.SteppedApplication {
    using Core;
    using Utilities;

    public class SAPlayer : Player {
        [Header("Level")]
        public PrePlayUI prePlayUI;
        public GameMenuUI gameMenuUI;

        public override void Build() {
            base.Build();
            prePlayUI?.gameObject.SetActive(true);
        }

        public virtual void Pause(bool screen) {
            gameMenuUI?.gameObject.SetActive(screen);
        }

        public virtual void Resume(bool screen) {
            gameMenuUI?.gameObject.SetActive(screen);
        }

        public virtual void Play(bool screen) {
            prePlayUI?.gameObject.SetActive(screen);
        }
    }
}