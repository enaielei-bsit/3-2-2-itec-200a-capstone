/*
 * Date Created: Tuesday, January 4, 2022 9:01 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.Tailgating {
    using Utilities;
    using Core;

    public class SATGPlayer : SAVehiclePlayer {
        protected bool _failed = false;
        public virtual bool failed => _failed;
        protected bool _done = false;
        public bool done => _done;

        [Space]
        public bool hasMaintainingDistance = true;
        public float maintainingDistance = 50.0f;

        [Space]
        public CinemachineLock cameraLock;
        public FrontVehicle frontVehicle;
        public List<LandmarkTrigger> triggers = new List<LandmarkTrigger>();

        public override void OnCollisionEnter(Collision collision) {
            base.OnCollisionEnter(collision);
            var trigger = collision.collider.GetTrigger<ObstacleTrigger>();
            if(trigger && !failed && !done) {
                _failed = true;
                TriggerGameOver();
            }
        }

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var trigger = collider.GetTrigger<DangerZoneTrigger>();
            if(trigger && !failed && !done) {
                _failed = true;
                TriggerGameOver();
            }

            if(!failed) {
                var stop = collider.GetTrigger<StopTrigger>();
                if(stop && !done) {
                    _done = true;
                    inGameUI?.gameObject.SetActive(false);
                    if(cameraLock) {
                        cameraLock.xPosition.Lock(cameraLock.transform.position.x);
                        cameraLock.yPosition.Lock(cameraLock.transform.position.y);
                        cameraLock.zPosition.Lock(cameraLock.transform.position.z);
                    }
                    frontVehicle.StopTravel();
                    frontVehicle.StopCountdown();
                    Invoke("Proceed", 3.0f);
                }
            }
        }

        public override void Update() {
            base.Update();
            if(frontVehicle) {
                if(isEngineRunning && currentGear == GearBox.Drive
                    && vehicle.input.Throttle > 0.0f
                    && !frontVehicle.travelling) {
                    frontVehicle.StartTravel();
                }

                if(hasMaintainingDistance) {
                    float max = Mathf.Abs(maintainingDistance);
                    float distance = Mathf.Abs(Vector3.Distance(
                        transform.position, frontVehicle.transform.position
                    ));

                    if(distance > max && !failed && !done) {
                        _failed = true;
                        TriggerGameOver();
                    }
                }
            }
        }

        public virtual void TriggerGameOver(bool screen) {
            frontVehicle.StopTravel();
            frontVehicle.StopCountdown();
            Reset();
            gameOverUI?.gameObject.SetActive(screen);
        }

        public virtual void TriggerGameOver() => TriggerGameOver(true);

        public virtual void Proceed() {
            LoadScene(BuiltScene.RightOfWay);
        }
    }
}