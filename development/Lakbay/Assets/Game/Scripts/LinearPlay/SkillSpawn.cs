/*
 * Date Created: Wednesday, October 13, 2021 6:49 AM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
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

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    public abstract class SkillSpawn : Spawn {
        public bool triggered = false;
        
        public override bool OnSpawn(
            Matrix matrix, GameObject cell, Vector2Int index, float chance) {
            var can = base.OnSpawn(matrix, cell, index, chance);
            if(can) {
                var skillSpawn = cell.GetComponentInChildren<SkillSpawn>();
                can = skillSpawn == null;

                if(can) {
                    var sp = cell.transform.parent
                        .GetComponentInChildren(GetType());
                    can = sp == null;
                }
            }
            return can;
        }

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var player = collider.GetComponentInParent<Player>();
            
            if(player) {
                if(!triggered) {
                    printLog("Player Hit!");
                    triggered = true;
                    OnTrigger(player);
                }
            }
        }

        public abstract void OnTrigger(Player player);
    }
}