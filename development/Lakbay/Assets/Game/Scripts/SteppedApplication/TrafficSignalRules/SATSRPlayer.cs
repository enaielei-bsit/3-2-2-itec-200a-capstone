/*
 * Date Created: Friday, January 7, 2022 2:35 AM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication.TrafficSignalRules {
    using Cinemachine;
    using Core;
    using TMPro;

    public enum CameraView {Player, TrafficLight}

    public class SATSRPlayer : SASteppedVehiclePlayer {
        [Space]
        public float trafficLightRedDuration = 20.0f;
        public float trafficLightYellowDuration = 20.0f;
        public float trafficLightGreenDuration = 20.0f;
        
        [Space]
        public float checkSpeedDelay = 3.0f;
        public float fullStopMaxSpeed = 0.0f;
        public float slowdownMaxSpeed = 5.0f;
        public float accelerateMinSpeed = 3.0f;
        public TrafficLight trafficLight;
        protected bool _triggeredTrafficLight = false;
        protected bool _failed = false;
        public virtual bool failed => _failed;
        protected bool _staying = false;
        protected float _checkStartTime = 0.0f;
        protected bool _done = false;
        public virtual  bool staying => _staying;

        public CinemachineVirtualCamera playerCamera;
        public CinemachineVirtualCamera trafficLightCamera;
        public Camera gizmoCamera;
        
        [Space]
        public string stateCountdownFormat = "00";
        public TextMeshPro stateCountdown;

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var tltrigger = collider.GetTrigger<TrafficLightTrigger>();
            if(tltrigger && !_triggeredTrafficLight && !failed) {
                _triggeredTrafficLight = true;
                trafficLight?.ToggleState(
                    TrafficLight.State.Yellow, trafficLightYellowDuration);
                Invoke("SetCameraToTrafficLight", 1.0f);
                _checkStartTime = Time.time;
                SetCamera(CameraView.TrafficLight);
            }
        }

        public override void OnTriggerStay(Collider collider) {
            base.OnTriggerStay(collider);
            var tltrigger = collider.GetTrigger<TrafficLightTrigger>();
            if(tltrigger && !failed && _triggeredTrafficLight) {
                _staying = true;
                CheckSpeed();
            }
        }

        public override void OnTriggerExit(Collider collider) {
            base.OnTriggerExit(collider);
            var tltrigger = collider.GetTrigger<TrafficLightTrigger>();
            if(tltrigger && !failed && _triggeredTrafficLight) {
                _staying = false;
                if(trafficLight.state == TrafficLight.State.Yellow
                    || trafficLight.state == TrafficLight.State.Red) {
                    _failed = true;
                    TriggerGameOver();
                } else {
                    _done = true;
                }
                tltrigger.gameObject.SetActive(false);
            }
        }

        public override void Awake() {
            base.Awake();
            if(trafficLight) trafficLight.state = TrafficLight.State.Green;
        }

        public virtual void ToggleSignalLight(
            TrafficLight light,
            TrafficLight.State old,
            TrafficLight.State @new) {
            if(@new == TrafficLight.State.Green) {
                light.state = TrafficLight.State.None;
            } else if(@new == TrafficLight.State.Yellow) {
                light.ToggleState(TrafficLight.State.Red, trafficLightRedDuration);
                _checkStartTime = Time.time;
            } else if(@new == TrafficLight.State.Red) {
                light.ToggleState(TrafficLight.State.Green, trafficLightGreenDuration);
                _checkStartTime = Time.time;
                if(stateCountdown) stateCountdown.enabled = false;
                Invoke("SetCameraToPlayer", 0.0f);
            }
        }

        public virtual void CheckSpeed() {
            if(!isEngineRunning || !_triggeredTrafficLight || failed) return;
            float delta = Time.time - _checkStartTime;
            if(delta < checkSpeedDelay) return;
            float speed = Mathf.Floor(vehicle.Speed);
            if(trafficLight.state == TrafficLight.State.Yellow
                && speed > slowdownMaxSpeed) {
                _failed = true;
                TriggerGameOver();
            } else if(trafficLight.state == TrafficLight.State.Red
                && speed > fullStopMaxSpeed) {
                _failed = true;
                TriggerGameOver();
            } else if(trafficLight.state == TrafficLight.State.Green
                && speed < accelerateMinSpeed) {
                _failed = true;
                TriggerGameOver();
            }
        }

        public virtual void TriggerGameOver() {
            Reset();
            gameOverUI?.gameObject.SetActive(true);
            trafficLight?.StopToggleState();
        }

        public virtual void SetCamera(CameraView view) {
            var cam = playerCamera;
            if(view == CameraView.Player) {
                cam = playerCamera;
                if(cam) cam.Priority = 1;
                if(trafficLightCamera) trafficLightCamera.Priority = 0;
            } else if(view == CameraView.TrafficLight) {
                cam = trafficLightCamera;
                if(cam) cam.Priority = 1;
                if(playerCamera) playerCamera.Priority = 0;
            }

            if(gizmoCamera) {
                gizmoCamera.fieldOfView = cam.m_Lens.FieldOfView;
            }
        }

        public virtual void SetCameraToPlayer() =>
            SetCamera(CameraView.Player);

        public virtual void SetCameraToTrafficLight() =>
            SetCamera(CameraView.TrafficLight);

        public override void Proceed() {
            LoadScene(BuiltScene.MainMenu);
        }

        public override void Update() {
            base.Update();
            if(trafficLight && stateCountdown) {
                float duration = trafficLight.duration;
                float elapsed = trafficLight.elapsedTime;
                if(duration > 0.0f) {
                    stateCountdown.gameObject.SetActive(true);
                    stateCountdown.SetText((duration - elapsed).ToString(
                        stateCountdownFormat
                    ));
                } else stateCountdown.gameObject.SetActive(false);
            }
        }
    }
}