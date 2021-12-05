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

        public override void OnCollisionEnter(Collision collision) {
            base.OnCollisionEnter(collision);
            if(!collided) {
                var player = collision.gameObject.GetComponentInParent<QRPlayer>();

                if(player && collision.collider.GetTrigger<SpawnTrigger>()) {
                    collided = true;
                    OnCollision(player);
                }
            }
        }

        public virtual void OnCollision(QRPlayer player) {
            printLog("Player took damage!");
            Break();
        }

        public virtual void Break() {
            foreach(var collider in GetComponentsInChildren<Collider>()) {
                var rb = collider.gameObject.EnsureComponent<Rigidbody>();
            };
        }
    }
}