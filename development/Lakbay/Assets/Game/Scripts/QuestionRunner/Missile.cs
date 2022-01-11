/*
 * Date Created: Thursday, October 14, 2021 11:22 PM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    [RequireComponent(typeof(Collider))]
    public class Missile : Core.Controller {
        public ParticleSystem destruction;
        public Travel travel;

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var obstacle = collider.GetComponentInParent<Spawns.ObstacleSpawn>();
            if(obstacle) {
                if(destruction) {
                    var par = Instantiate(destruction);
                    par.transform.position = obstacle.transform.position;
                    Destroy(par.gameObject, par.main.duration);
                }

                obstacle.collided = true;
                obstacle.Break();
                // Destroy(obstacle.gameObject);
            }
        }
    }
}