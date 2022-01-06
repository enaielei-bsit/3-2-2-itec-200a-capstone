/*
 * Date Created: Thursday, January 6, 2022 2:58 AM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication.RightOfWay {
    using Cinemachine;
    using Core;

    public class SAROWPlayer : SASteppedVehiclePlayer {
        protected bool _failed = false;
        public virtual bool failed => _failed;
        protected bool _pedestriansStartedWalking = false;
        protected bool _staying = false;

        [Space]
        public CinemachineVirtualCamera playerCamera;
        public CinemachineVirtualCamera pedestrianCamera;
        public Camera gizmoCamera;

        public virtual bool pedestriansStartedWalking =>
            _pedestriansStartedWalking;
        
        [Space]
        public List<Pedestrian> pedestrians = new List<Pedestrian>();

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var ptrigger = collider.GetTrigger<PedestrianTrigger>();
            if(ptrigger) {
                _staying = true;
                if(!pedestriansStartedWalking) {
                    WalkPedestrians();
                }

                if(playerCamera && pedestrianCamera) {
                    int playerPrio = playerCamera.Priority;
                    int pedestrianPrio = pedestrianCamera.Priority;

                    if(pedestrianPrio <= playerPrio) {
                        pedestrianCamera.Priority = playerPrio + 1;
                        if(gizmoCamera)
                            gizmoCamera.fieldOfView =
                                pedestrianCamera.m_Lens.FieldOfView;
                    }
                }
            }

            var tl = collider.GetTrigger<TurnLeftTrigger>();
            var tr = collider.GetTrigger<TurnRightTrigger>();
            if(((tl && staticSignalLight != SignalLight.Left)
                || (tr && staticSignalLight != SignalLight.Right))
                && !failed) {
                if(tl) tl.gameObject.SetActive(false);
                if(tr) tr.gameObject.SetActive(false);
                _failed = true;
                Reset();
                gameOverUI?.gameObject.SetActive(true);
            }
        }

        public override void OnTriggerExit(Collider collider) {
            base.OnTriggerExit(collider);
            var ptrigger = collider.GetTrigger<PedestrianTrigger>();
            if(ptrigger) {
                _staying = false;

                if(playerCamera && pedestrianCamera) {
                    int playerPrio = playerCamera.Priority;
                    int pedestrianPrio = pedestrianCamera.Priority;

                    if(playerPrio <= pedestrianPrio) {
                        playerCamera.Priority = pedestrianPrio + 1;
                        if(gizmoCamera)
                            gizmoCamera.fieldOfView =
                                playerCamera.m_Lens.FieldOfView;
                    }
                }
            }
        }

        public override void Proceed() {
            LoadScene();
        }

        public virtual void WalkPedestrians() {
            _pedestriansStartedWalking = true;
            foreach(var pedestrian in pedestrians) {
                pedestrian.StartWalking();
            }
        }

        public override void Update() {
            base.Update();
        }
    }
}