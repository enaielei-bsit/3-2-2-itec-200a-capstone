/*
 * Date Created: Wednesday, October 13, 2021 5:05 PM
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
    public class ObstacleSpawn : Spawn {
        public ParticleSystem destruction;
        public bool collided = false;
        public int safeRowCount = 5;

        public override bool OnSpawn(
            Matrix matrix, GameObject cell, Vector2Int index, float chance) {
            bool can = base.OnSpawn(matrix, cell, index, chance);
            if(can) {
                var spawn = cell.GetComponentInChildren<Spawn>();
                can = spawn == null;
            }

            can = can && index.y >= safeRowCount;

            return can;
        }

        public override void OnCollisionEnter(Collision collision) {
            base.OnCollisionEnter(collision);
            if(!collided) {
                var player = collision.gameObject.GetComponentInParent<Player>();
                if(player && !collision.gameObject.GetComponentInParent<Buff>()) {
                    collided = true;
                    OnCollision(player);
                }
            }
        }

        public virtual void OnCollision(Player player) {
            printLog("Player took damage!");
            Break();
        }

        public virtual void Break() {
            foreach(var collider in GetComponentsInChildren<Collider>()) {
                var rb = collider.gameObject.EnsureComponent<Rigidbody>();
            };
        }

        public virtual void Destroy() {
            if(destruction) {
                var par = Instantiate(destruction);
                par.transform.position = transform.position;
                Destroy(par.gameObject, par.main.duration);
            }
            Destroy(gameObject);
        }
    }
}