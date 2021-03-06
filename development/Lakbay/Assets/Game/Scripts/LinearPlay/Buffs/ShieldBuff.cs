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
        protected readonly List<Material> _materials = new List<Material>();

        public Material effect;

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var obstacle = collider.GetComponentInParent<Spawns.ObstacleSpawn>();
            if(obstacle) {
                obstacle.collided = true;
                obstacle.Break();
            }
        }

        public override void OnAdd(Buffable buffable, float duration) {
            _materials.Clear();
            for(int i = 0; i < buffable.mainMaterials.Count; i++) {
                var material = buffable.mainMaterials[i];
                _materials.Add(material.material);
                material.material = effect;
            }
        }

        public override void OnLinger(
            Buffable buffable, float duration, float elapsedTime) {
        }

        public override void OnRemove(Buffable buffable) {
            for(int i = 0; i < buffable.mainMaterials.Count; i++) {
                if(_materials.Count == 0) break;
                var material = buffable.mainMaterials[i];
                material.material = _materials.Pop(0);
            }
        }
    }
}