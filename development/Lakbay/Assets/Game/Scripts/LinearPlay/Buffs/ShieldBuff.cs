/*
 * Date Created: Thursday, October 14, 2021 10:52 AM
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
    public class ShieldBuff : Buff {
        public override void OnAdd(Buffable buffable, float duration) {
        }

        public override void OnLinger(
            Buffable buffable, float duration, float elapsedTime) {
        }

        public override void OnRemove(Buffable buffable) {
        }

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var obstacle = collider.GetComponentInParent<Spawns.ObstacleSpawn>();
            if(obstacle) {
                obstacle.collided = true;
                obstacle.Break();
            }
        }
    }
}