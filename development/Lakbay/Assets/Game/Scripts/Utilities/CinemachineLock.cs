/*
 * Date Created: Saturday, November 13, 2021 12:43 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using Cinemachine;
using System;
using UnityEngine;

namespace Utilities
{
    // Source: https://forum.unity.com/threads/follow-only-along-a-certain-axis.544511/#post-3591751
    /// <summary>
    /// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
    /// </summary>
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class CinemachineLock : CinemachineExtension
    {
        [Serializable]
        public struct Axis
        {
            public bool locked;
            public float value;

            public void Lock(float value)
            {
                Lock();
                this.value = value;
            }

            public void Lock()
            {
                locked = true;
            }

            public void Unlock() => locked = false;
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
            ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var vec = state.RawPosition;
                vec.x = _GetAxisValue(xPosition, vec.x);
                vec.y = _GetAxisValue(yPosition, vec.y);
                vec.z = _GetAxisValue(zPosition, vec.z);
                state.RawPosition = vec;
            }
            else if (stage == CinemachineCore.Stage.Aim)
            {
                var vec = state.RawOrientation.eulerAngles;
                vec.x = _GetAxisValue(xRotation, vec.x);
                vec.y = _GetAxisValue(yRotation, vec.y);
                vec.z = _GetAxisValue(zRotation, vec.z);
                state.RawOrientation = Quaternion.Euler(vec);
            }
        }

        protected virtual float _GetAxisValue(Axis axis, float defaultValue = 0.0f)
        {
            return axis.locked ? axis.value : defaultValue;
        }
    }
}