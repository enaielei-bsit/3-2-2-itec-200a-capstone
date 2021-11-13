/*
 * Date Created: Saturday, November 13, 2021 12:43 PM
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

using Cinemachine;

namespace Utilities {
    // Source: https://forum.unity.com/threads/follow-only-along-a-certain-axis.544511/#post-3591751
    /// <summary>
    /// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
    /// </summary>
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class CinemachineLock : CinemachineExtension {
        [Serializable]
        public struct Axis {
            public bool locked;
            public float value;
        }

        [Header("Position")]
        public Axis xPosition;
        public Axis yPosition;
        public Axis zPosition;

        [Header("Rotation")]
        public Axis xRotation;
        public Axis yRotation;
        public Axis zRotation;
    
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state, float deltaTime) {
            if(stage == CinemachineCore.Stage.Body) {
                var vec = state.RawPosition;
                vec.x = _GetLockValue(xPosition, vec.x);
                vec.y = _GetLockValue(yPosition, vec.y);
                vec.z = _GetLockValue(zPosition, vec.z);
                state.RawPosition = vec;
            } else if(stage == CinemachineCore.Stage.Aim) {
                var vec = state.RawOrientation.eulerAngles;
                vec.x = _GetLockValue(xRotation, vec.x);
                vec.y = _GetLockValue(yRotation, vec.y);
                vec.z = _GetLockValue(zRotation, vec.z);
                state.RawOrientation = Quaternion.Euler(vec);
            }
        }

        protected virtual float _GetLockValue(Axis axis, float defaultValue=0.0f) {
            return axis.locked ? axis.value : defaultValue;
        }
    }
}