/*
 * Date Created: Thursday, October 14, 2021 10:05 PM
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

namespace Ph.CoDe_A.Lakbay.LinearPlay.Buffs {
    public class MissileBuff : Buff {
        protected Missile _missile;

        public float travelDistance = 200.0f;
        public Missile missile;

        public override void OnAdd(Buffable buffable, float duration) {
            if(!missile) return;
            _missile = Instantiate(missile);
            _missile.transform.position = transform.position;
            _missile.travel.speed = travelDistance / Mathf.Max(duration, 0.001f);
            _missile.travel.Perform(true);
        }

        public override void OnLinger(
            Buffable buffable, float duration, float elapsedTime) {
        }

        public override void OnRemove(Buffable buffable) {
            if(_missile) {
                _missile.travel.Perform(false);
                Destroy(_missile.gameObject);
            }
        }
    }
}