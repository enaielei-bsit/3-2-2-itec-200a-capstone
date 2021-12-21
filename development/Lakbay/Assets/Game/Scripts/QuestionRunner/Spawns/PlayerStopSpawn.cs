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
        [Min(0)]
        public float delay = 3.0f;
        [Min(0)]
        public int offset = 5;

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var player = collider.GetComponentInParent<QRPlayer>();
            
            if(player && collider.GetTrigger<SpawnTrigger>()) { 
                if(!triggered) {
                    triggered = true;
                    if(player.camera) {
                        var lck = player.camera.GetComponent<CinemachineLock>();
                        if(!lck.zPosition.locked) {
                            if(Session.qrLevelIndex != Session.qrLevels.Count - 1)
                                player.Invoke("Proceed", delay);
                            else player.qrPostPlayUI?.Invoke("Show", delay);
                            
                            player.travel?.Invoke("StopPerforming", delay);
                            lck.zPosition.Lock(player.camera.transform.position.z);
                        }
                    }

                    player.qrInGameUI?.gameObject.SetActive(false);

                    if(player.repeaterHandler
                        && player.repeaterHandler.maxRepeat != 0) {
                        player.repeaterHandler.maxRepeat = 0;
                    }
                }
            }
        }

        public override bool OnSpawnCheck(
            Spawner spawner, Transform[] locations, Transform location) {
            if(base.OnSpawnCheck(spawner, locations, location)) {
                int count = Session.qrLevel.repeaterHandler.repeated;
                int target = Session.qrLevel.lastStop + offset;
                return Session.qrLevel.done
                    && count >= target;
            }

            return false;
        }
    }
}