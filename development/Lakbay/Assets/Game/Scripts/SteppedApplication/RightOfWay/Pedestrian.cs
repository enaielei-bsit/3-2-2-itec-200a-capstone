/*
 * Date Created: Thursday, January 6, 2022 3:55 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

using Utilities;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.RightOfWay
{
    [RequireComponent(typeof(CinemachinePathFollower))]
    public class Pedestrian : Core.Controller
    {
        protected Coroutine _walk;

        public virtual CinemachinePathFollower follower =>
            GetComponent<CinemachinePathFollower>();
        public float walkDuration = 20.0f;
        public float walkTimeScale = 1.0f;
        public Animator animator;

        public override void Awake()
        {
            StopWalking();
        }

        public virtual void StartWalking(float duration)
        {
            StopWalking();
            if (animator) animator.enabled = true;
            _walk = this.Run(
                duration,
                onProgress: (d, e) =>
                {
                    follower.value = e / d;
                    return Time.deltaTime * walkTimeScale;
                },
                onFinish: (d, e) =>
                {
                    follower.value = 1.0f;
                    StopWalking();
                },
                fixedUpdate: true
            );
        }

        public virtual void StartWalking() => StartWalking(walkDuration);

        public virtual void StopWalking()
        {
            if (animator) animator.enabled = false;
            if (_walk != null)
            {
                StopCoroutine(_walk);
                _walk = null;
            }
        }
    }
}