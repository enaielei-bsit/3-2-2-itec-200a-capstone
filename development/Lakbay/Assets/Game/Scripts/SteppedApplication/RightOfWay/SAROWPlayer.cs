/*
 * Date Created: Thursday, January 6, 2022 2:58 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System.Collections.Generic;

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.RightOfWay
{
    using Cinemachine;
    using Core;
    using UnityEngine.Localization;

    public class SAROWPlayer : SASteppedVehiclePlayer
    {
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

        [Header("Messages")]
        public LocalizedString didntHitPedestrian;
        public LocalizedString usedSignalLight;
        public LocalizedString hitPedestrian;
        public LocalizedString noSignalLight;

        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            var ptrigger = collider.GetTrigger<PedestrianTrigger>();
            if (ptrigger)
            {
                _staying = true;
                if (!pedestriansStartedWalking)
                {
                    WalkPedestrians();
                }

                if (playerCamera && pedestrianCamera)
                {
                    int playerPrio = playerCamera.Priority;
                    int pedestrianPrio = pedestrianCamera.Priority;

                    if (pedestrianPrio <= playerPrio)
                    {
                        pedestrianCamera.Priority = playerPrio + 1;
                        if (gizmoCamera)
                            gizmoCamera.fieldOfView =
                                pedestrianCamera.m_Lens.FieldOfView;
                    }
                }
            }

            var tl = collider.GetTrigger<TurnLeftTrigger>();
            var tr = collider.GetTrigger<TurnRightTrigger>();
            if (((tl && staticSignalLight != SignalLight.Left)
                || (tr && staticSignalLight != SignalLight.Right))
                && !failed)
            {
                if (tl) tl.gameObject.SetActive(false);
                if (tr) tr.gameObject.SetActive(false);
                _failed = true;
                Reset();
                // gameOverUI?.gameObject.SetActive(true);
                gameOverUI?.ShowFailed(noSignalLight);
            }
        }

        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
            var ptrigger = collider.GetTrigger<PedestrianTrigger>();
            if (ptrigger)
            {
                _staying = false;

                if (playerCamera && pedestrianCamera)
                {
                    int playerPrio = playerCamera.Priority;
                    int pedestrianPrio = pedestrianCamera.Priority;

                    if (playerPrio <= pedestrianPrio)
                    {
                        playerCamera.Priority = pedestrianPrio + 1;
                        if (gizmoCamera)
                            gizmoCamera.fieldOfView =
                                playerCamera.m_Lens.FieldOfView;
                    }
                }
            }
        }

        public override void Proceed()
        {
            LoadScene(BuiltScene.TrafficSignalRules);
        }

        public virtual void WalkPedestrians()
        {
            _pedestriansStartedWalking = true;
            foreach (var pedestrian in pedestrians)
            {
                pedestrian.StartWalking();
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnObstacleHit()
        {
            base.OnObstacleHit();
            Reset();
            gameOverUI?.ShowFailed(hitPedestrian);
        }

        public override void OnPark()
        {
            base.OnPark();
            Session.checkpointController?.SaveCheckpoint(
                new Checkpoint(Session.mode, BuiltScene.TrafficSignalRules)
            );
            gameOverUI?.ShowPassed(
                usedSignalLight, didntHitPedestrian
            );
        }
    }
}