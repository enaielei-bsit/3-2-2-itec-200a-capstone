/*
 * Date Created: Friday, October 15, 2021 10:25 AM
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

namespace Ph.CoDe_A.Lakbay.LinearPlay.Spawns {
    public class QuestionSpawn : Spawn {
        public bool triggered = false;

        public override bool OnSpawn(
            Matrix matrix, GameObject cell, Vector2Int index, float chance) {
            bool can = base.OnSpawn(matrix, cell, index, chance);
            if(can) {
                printLog("heheh");
                var spawns = cell.GetComponentsInChildren<Spawn>();
                if(Application.isPlaying) cell.DestroyChildren();
                else cell.DestroyChildrenImmediately();

                // if(can) {
                //     var sp = cell.transform.parent
                //         .GetComponentInChildren(GetType());
                //     can = sp == null;
                // }
            }

            return can;
        }

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var player = collider.GetComponentInParent<Player>();
            
            if(player && !player.GetComponentInParent<Buff>()) {
                if(!triggered) {
                    triggered = true;
                    OnTrigger(player);
                }
            }
        }

        public virtual void OnTrigger(Player player) {
            player.timeScale = 0.0f;
            gameObject.SetActive(false);
        }
    }
}