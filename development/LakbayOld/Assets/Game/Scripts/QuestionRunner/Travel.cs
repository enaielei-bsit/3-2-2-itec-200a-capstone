/*
 * Date Created: Sunday, October 10, 2021 7:37 PM
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
    public class Travel : Core.Entity {
        protected bool _performing;
        protected int _direction;
        protected Vector3 _offset;

        // Meter per Second
        public float speed = 20.0f;
        public Axis axis = Axis.Z;
        public AxisDirection axisDirection = AxisDirection.Positive;
        public virtual bool performing => _performing;

        public virtual void Perform(bool toggle) {
            if(toggle && !performing) {
                _direction = (int) axisDirection;
                _offset = axis == Axis.X ? Vector3.right
                    : (axis == Axis.Y ? Vector3.up : Vector3.forward);
                _performing = true;
            } else if(!toggle && performing) {
                _performing = false;
            }
        }

        protected virtual void _Perform(int direction, Vector3 offset) {
            var trans = offset * speed * direction * timeScale
                * Time.deltaTime;
            transform.Translate(trans);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if(performing) _Perform(_direction, _offset);
        }
    }
}