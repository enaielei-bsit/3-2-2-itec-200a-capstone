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

using Utilities;

namespace Ph.CoDe_A.Lakbay.LinearPlay.Buffs {
    public class ShieldBuff : Buff {
        protected readonly List<Color> _materialColors = new List<Color>();

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var obstacle = collider.GetComponentInParent<Spawns.ObstacleSpawn>();
            if(obstacle) {
                obstacle.collided = true;
                obstacle.Break();
            }
        }

        public override void OnAdd(Buffable buffable, float duration) {
            _materialColors.Clear();
            foreach(var material in buffable.mainMaterials) {
                _materialColors.Add(material.material.color);
                // material.material.color = Color.red;
            }
        }

        public override void OnLinger(
            Buffable buffable, float duration, float elapsedTime) {
        }

        public override void OnRemove(Buffable buffable) {
            foreach(var material in buffable.mainMaterials) {
                material.material.color = _materialColors.Pop(0);
            }
        }
    }
}