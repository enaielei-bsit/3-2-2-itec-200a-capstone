/*
 * Date Created: Thursday, December 23, 2021 1:22 PM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication.ParallelParking {
    using Utilities;
    using Core;
    using EVP;

    public class SAPPPlayer : SAVehiclePlayer {
        protected bool _failed = false;
        public virtual bool failed => _failed;

        public virtual void Proceed() {
            LoadScene(BuiltScene.ParallelParking);
        }

        public override void OnCollisionEnter(Collision collision) {
            base.OnCollisionEnter(collision);
            var collider = collision.collider;
            var target = collider.GetTrigger<TargetTrigger>();
            var obstacle = collider.GetTrigger<ObstacleTrigger>();
            if(target) {

            } else if(obstacle) {
                if(!failed) {
                    _failed = true;
                    gameOverUI?.gameObject.SetActive(true);
                }
            }
        }
    }
}