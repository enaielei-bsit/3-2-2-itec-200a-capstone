/*
 * Date Created: Friday, October 8, 2021 7:22 AM
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

namespace Ph.CoDe_A.Lakbay.Core {
    public abstract class Initializer : Entity {
        protected bool _initialized = false;
        public virtual bool initialized => _initialized;

        public override void Awake() {
            base.Awake();
            if(!Game.initialized && Game.canInitialize) {
                DontDestroyOnLoad(gameObject);
                Game.initializers.Add(this);
                if(!initialized) {
                    Initialize();
                }
            }
        }

        public virtual void Initialize() {
        }
    }
}