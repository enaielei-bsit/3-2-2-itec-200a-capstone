/*
 * Date Created: Monday, December 6, 2021 10:13 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ph.CoDe_A.Lakbay.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Spawns {
    
    [RequireComponent(typeof(Collider))]
    public class PlayerStopSpawn : QRSpawn {
        public bool triggered = false;

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var player = collider.GetComponentInParent<QRPlayer>();
            
            if(player && collider.GetTrigger<SpawnTrigger>()) { 
                if(!triggered) {
                    triggered = true;
                    if(player.camera) {
                        var lck = player.camera.GetComponent<CinemachineLock>();
                        lck.zPosition.locked = true;
                        lck.zPosition.value = player.camera.transform.position.z;
                    }

                    if(player.repeaterHandler) {
                        player.repeaterHandler.maxRepeat = 0;
                    }

                    Session.stopEnd = player.repeaterHandler.repeated;
                }
            }
        }
    }
}