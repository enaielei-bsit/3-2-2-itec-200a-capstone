/*
 * Date Created: Tuesday, November 30, 2021 12:50 PM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication.Blowbagets {
    using Cinemachine;
    using Core;
    using Utilities;

    public class SABBPlayer : SAPlayer {
        protected Vector3 _baseFollowOffset;

        [Header("Level")]
        public new CinemachineVirtualCamera camera;
        public virtual CinemachineOrbitalTransposer transposer =>
            camera?.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        [Header("Gameplay")]
        public float maxScale = 1.0f;
        public float maxRotation = 360.0f;

        public override void Build() {
            base.Build();
            _baseFollowOffset = transposer.m_FollowOffset;
        }
        
        public virtual void Rotate(float factor) {
            if(camera) {
                transposer.m_XAxis.Value = maxRotation * factor;
                printLog($"Rotating: {transposer.m_XAxis.Value}");
            }
        }

        public virtual void Scale(float factor) {
            if(camera) {
                transposer.m_FollowOffset = _baseFollowOffset + (Vector3.forward * (maxScale * factor));
                printLog($"Scaling: {transposer.m_FollowOffset.z}");
            }
        }
    }
}