/*
 * Date Created: Monday, November 22, 2021 11:46 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;
using UnityEngine.Events;

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner
{
    public class Slide : Core.DirectionalMovement
    {
        protected float _initial;
        public virtual float initial => _initial;
        protected float _target;
        public virtual float target => _target;
        protected int _index;
        public int index => _index;
        public int minIndex = -1;
        public int maxIndex = 1;
        public float offset = 4.0f;
        [Min(0.001f)]
        public float duration = 0.2f;

        [Space]
        public UnityEvent onSlideLeft = new UnityEvent();
        public UnityEvent onSlideRight = new UnityEvent();

        public override void Update()
        {
            base.Update();

            bool left = IInput.keyboard.leftArrowKey.wasPressedThisFrame;
            bool right = IInput.keyboard.rightArrowKey.wasPressedThisFrame;

            if (left) PerformLeft(offset, duration);
            if (right) PerformRight(offset, duration);
        }

        public virtual void Perform(float offset, float duration)
        {
            if (performing) return;

            _initial = transform.position.x;
            _target = initial + offset;

            duration = Mathf.Max(0.001f, duration);
            xAxis.speed = offset / duration;

            if ((xAxis.direction == -1 && index == minIndex)
                || xAxis.direction == 1 && index == maxIndex) return;

            _index += xAxis.direction;

            Perform(true);
            if (xAxis.direction == -1) onSlideLeft?.Invoke();
            else onSlideRight?.Invoke();
            this.Run(
                duration,
                onProgress: (d, e) =>
                {
                    return Time.deltaTime * timeScale;
                },
                onFinish: (d, e) =>
                {
                    Perform(false);
                    var pos = transform.position;
                    pos.x = target;
                    transform.position = pos;
                }, fixedUpdate: true
            );
        }

        public virtual void PerformLeft(float offset, float duration) =>
            Perform(Mathf.Abs(offset) * -1, duration);

        public virtual void PerformRight(float offset, float duration) =>
            Perform(Mathf.Abs(offset), duration);

        public virtual void PerformLeft()
        {
            PerformLeft(offset, duration);
        }

        public virtual void PerformRight()
        {
            PerformRight(offset, duration);
        }
    }
}