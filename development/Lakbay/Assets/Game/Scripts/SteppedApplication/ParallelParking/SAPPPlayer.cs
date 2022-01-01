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
        protected bool _done = false;
        public virtual bool done => _done;

        public Vector3 targetPositionOffset = new Vector3(3.0f, 3.0f, 3.0f);
        public Vector3 targetRotationOffset = new Vector3(7.0f, 7.0f, 7.0f);

        public virtual void Proceed() {
            LoadScene(BuiltScene.ParallelParking);
        }

        public override void OnCollisionEnter(Collision collision) {
            base.OnCollisionEnter(collision);
            var collider = collision.collider;
            var obstacle = collider.GetTrigger<ObstacleTrigger>();
            if(obstacle) {
                if(!failed && !done) {
                    _failed = true;
                    gameOverUI?.gameObject.SetActive(true);
                }
            }
        }

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
        }

        public override void OnTriggerStay(Collider collider) {
            base.OnTriggerStay(collider);
            var target = collider.GetTrigger<TargetTrigger>();
            if(target && !failed) {
                var position = target.transform.position;
                var offset = targetPositionOffset;
                var min = position - offset;
                var max = position + offset;
                bool posOk = transform.position.Within(min, max);

                var rotation = target.transform.rotation.eulerAngles;
                offset = targetRotationOffset;
                min = rotation - offset;
                max = rotation + offset;
                bool rotOk = transform.rotation.eulerAngles.Within(min, max);

                printLog($"Position: {posOk}, Rotation: {rotOk}");
                if(posOk && rotOk && !done && Mathf.Floor(vehicle.Speed) == 0.0f) {
                    _done = true;
                    SetIgnition(false);
                    inGameUI?.gameObject.SetActive(false);
                    Invoke("Proceed", 3.0f);
                }
            }
        }

        public override void OnTriggerExit(Collider collider) {
            base.OnTriggerExit(collider);
        }

        public override void Update() {
            base.Update();
        }
    }
}