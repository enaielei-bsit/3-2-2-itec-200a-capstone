/*
 * Date Created: Wednesday, January 5, 2022 1:46 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.Tailgating {
    using Utilities;
    using Core;
    using TMPro;

    [RequireComponent(typeof(CinemachinePathFollower))]
    public class FrontVehicle : Controller {
        protected Coroutine _countdown;
        protected Coroutine _travel;

        public virtual bool travelling => _travel != null;
        public virtual bool countingDown => _countdown != null;

        public virtual CinemachinePathFollower follower =>
            GetComponent<CinemachinePathFollower>();

        public float checkingTime = 1.0f;

        [Space]
        public float countdown = 3.0f;
        public float countdownTimeScale = 0.5f;
        public string countdownFormat = "0";

        [Space]
        public float travelDuration = 90.0f;
        public float travelTimeScale = 1.0f;
        public float travelSlowdown = 0.5f;

        [Space]
        public DangerZoneTrigger dangerZoneTrigger;
        public TextMeshPro countdownText;

        public virtual void StartCountdown() {
            StopCountdown();
            travelTimeScale *= travelSlowdown;
            countdownText?.gameObject.SetActive(true);
            dangerZoneTrigger?.gameObject.SetActive(true);
            _countdown = this.Run(
                countdown,
                onProgress: (d, e) => {
                    countdownText?.SetText((d - e).ToString(countdownFormat));
                    return Time.deltaTime * countdownTimeScale;
                },
                onFinish: (d, e) => {
                    countdownText?.SetText(0.ToString(countdownFormat));
                    countdownText?.gameObject.SetActive(false);

                    EnableDangerZoneColliders();
                    Invoke("Reset", checkingTime);
                    Invoke("StopCountdown", checkingTime);
                }
            );
        }

        public virtual void StopCountdown() {
            if(_countdown != null) {
                travelTimeScale /= travelSlowdown;
                StopCoroutine(_countdown);
                _countdown = null;
            }
        }

        public virtual void SetDangerZoneColliders(bool enabled) {
            var trigger = dangerZoneTrigger;
            if(trigger) {
                foreach(var collider in trigger.GetComponentsInChildren<Collider>()) {
                    collider.enabled = enabled;
                }
            }
        }

        public virtual void EnableDangerZoneColliders() =>
            SetDangerZoneColliders(true);

        public virtual void DisableDangerZoneColliders() =>
            SetDangerZoneColliders(false);

        public override void Awake() {
            base.Awake();
            Reset();
        }

        public virtual void Reset() {
            dangerZoneTrigger?.gameObject.SetActive(false);
            DisableDangerZoneColliders();
            countdownText?.gameObject.SetActive(false);
        }

        public virtual void StartTravel() {
            StopTravel();
            _travel = this.Run(
                travelDuration,
                onProgress: (d, e) => {
                    follower.value = e / d;
                    return Time.deltaTime * travelTimeScale;
                },
                fixedUpdate: true
            );
        }

        public virtual void StopTravel() {
            if(_travel != null) {
                StopCoroutine(_travel);
                _travel = null;
            }
        }

        public override void OnTriggerExit(Collider collider) {
            base.OnTriggerExit(collider);
            var trigger = collider.GetTrigger<LandmarkTrigger>();
            if(trigger && !trigger.triggered) {
                trigger.triggered = true;
                StartCountdown();
            }
        }
    }
}