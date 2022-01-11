/*
 * Date Created: Tuesday, November 23, 2021 7:50 AM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Spawns {
    using Core;

    public class ObstacleSpawn : QRSpawn {
        public bool collided = false;
        public UnityEvent onTrigger = new UnityEvent();
        public UnityEvent onBreak = new UnityEvent();

        public override void OnCollisionEnter(Collision collision) {
            base.OnCollisionEnter(collision);
            if(!collided) {
                var player = collision.gameObject.GetComponentInParent<QRPlayer>();

                if(player && collision.collider.GetTrigger<SpawnTrigger>()) {
                    collided = true;
                    onTrigger?.Invoke();
                    OnCollision(player);
                }
            }
        }

        public override bool OnSpawnCheck(
            Spawner spawner, Transform[] locations, Transform location) {
            if(base.OnSpawnCheck(spawner, locations, location)) {
                return !Session.qrLevel.done;
            }

            return false;
        }

        public virtual void OnCollision(QRPlayer player) {
            if(player.lives > 0) {
                player.lives--;
                printLog($"Player took damage! Current Lives: {player.lives}");
            }

            if(player.lives <= 0) {
                player.gameOverUI?.gameObject.SetActive(true);
                player.Pause();
            }
            
            Break();
        }

        public virtual void Break() {
            onBreak?.Invoke();
            foreach(var collider in GetComponentsInChildren<Collider>()) {
                var rb = collider.gameObject.EnsureComponent<Rigidbody>();
            };
        }
    }
}