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

        public bool targetRotationIsBothWays = true;
        [Min(0.0f)]
        public float targetPositionOffset = 2.0f;
        [Min(0.0f)]
        public float targetRotationOffset = 5.0f;

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
                var targetPos = target.transform.position;
                var thisPos = transform.position;
                float posOffset = Mathf.Abs(targetPositionOffset);
                float distance =
                    Mathf.Abs(Vector3.Distance(thisPos, targetPos));
                bool posOk = distance <= posOffset;

                var targetRot = target.transform.rotation;
                var thisRot = transform.rotation;
                float rotOffset = Mathf.Abs(targetRotationOffset);
                float angle =
                    Mathf.Abs(Quaternion.Angle(thisRot, targetRot));
                bool rotOk = angle <= rotOffset;
                if(!rotOk && targetRotationIsBothWays) {
                    var rtargetRot = targetRot * Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    angle = Mathf.Abs(Quaternion.Angle(rtargetRot, thisRot));
                    rotOk = angle <= rotOffset;
                }

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
            // printLog(transform.rotation.eulerAngles.normalized);
        }
    }
}