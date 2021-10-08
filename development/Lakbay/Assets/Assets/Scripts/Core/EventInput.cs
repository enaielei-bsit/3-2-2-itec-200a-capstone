/*
 * Date Created: Friday, October 8, 2021 7:28 AM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core {
    public class EventInput : Initializer {
        public EventSystem eventSystem;

        public override void Initialize() {
            base.Initialize();
            if(!this.eventSystem) return;
            var eventSystem = FindObjectOfType<EventSystem>();
            if(!eventSystem) {
                DontDestroyOnLoad(Instantiate(this.eventSystem));
            }
            _initialized = true;
        }
    }
}