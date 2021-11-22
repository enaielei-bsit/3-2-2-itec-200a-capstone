/*
 * Date Created: Monday, November 22, 2021 9:29 AM
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

namespace Ph.CoDe_A.Lakbay.Core {
    public class DirectionalMovement : Controller {
        [Serializable]
        public struct Axis {
            // Meter per Second
            public bool active;
            public float speed;
            public int direction => speed < 0 ? -1 : 1;
        }

        [SerializeField]
        protected bool _performing = false;
        public virtual bool performing => _performing;

        public float timeScale = 1.0f;
        public Axis x;
        public Axis y;
        public Axis z;

        public override void Update() {
            base.Update();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if(performing) {
                if(x.active) _Perform(0, x.direction, x.speed);
                if(y.active) _Perform(1, y.direction, y.speed);
                if(z.active) _Perform(2, z.direction, z.speed);
            }
        }

        public virtual void Perform(bool toggle) => _performing = toggle;

        protected virtual void _Perform(
            int axis, int direction, float speed) {
            var offset = speed * timeScale
                * Time.deltaTime;
            var vec = Vector3.zero;
            vec[axis] = offset;
            transform.Translate(vec);
        }
    }
}