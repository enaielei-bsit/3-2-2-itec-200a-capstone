/*
 * Date Created: Sunday, October 10, 2021 6:00 PM
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

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    public class Slide : Core.Entity {
        protected bool _performing;
        protected int _direction, _axisIndex;
        protected float _targetPosition;
        protected Vector3 _offset;

        public float origin = 0.0f;
        // Meter per Second
        public float speed = 30.0f;
        // Meter
        public float distance = 4.0f;
        public Axis axis = Axis.X;
        public RangedInt step = new RangedInt(0, -1, 1);
        public Animator animator;
        public virtual bool performing => _performing;

        public override void Start() {
            base.Start();
            origin = transform.position[(int) axis];
        }

        public virtual void Perform(int step) {
            if(performing) return;
            _axisIndex = (int) axis;
            _direction = step < 0 ? -1 : 1;

            if((_direction == -1 && this.step.isMin)
                || (_direction == 1 && this.step.isMax)) return;
                
            _offset = axis == Axis.X ? Vector3.right
                : (axis == Axis.Y ? Vector3.up : Vector3.forward);
            this.step.value += step;
            float targetDistance = distance * this.step.value;
            _targetPosition = origin + targetDistance;

            if(animator) {
                animator.SetFloat(
                    "timeScale",
                    Mathf.Abs(
                        _targetPosition - transform.position[_axisIndex])
                        * 0.5f * timeScale);
                // animator.SetTrigger(_direction == -1 ? "left" : "right");
                animator.Play(_direction == -1 ? "SlideLeft" : "SlideRight");
            }

            _performing = true;
        }

        protected virtual void _Perform(
            int direction, int axisIndex, float targetPosition, Vector3 offset) {
            _performing = transform.position[_axisIndex] != targetPosition;

            var trans = offset * speed * direction * timeScale
                * Time.deltaTime;
            transform.Translate(trans);
            
            var pos = transform.position;
            pos[axisIndex] = direction == -1
                ? Mathf.Max(pos[axisIndex], targetPosition)
                : Mathf.Min(pos[axisIndex], targetPosition);
            transform.position = pos;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if(performing) _Perform(
                _direction, _axisIndex, _targetPosition, _offset);
        }
    }
}