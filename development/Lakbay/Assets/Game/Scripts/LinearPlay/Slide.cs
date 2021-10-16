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

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    public class Slide : Core.Entity {
        protected bool _performing;

        public float origin = 0.0f;
        // Meter per Second
        public float speed = 30.0f;
        // Meter
        public float distance = 4.0f;
        public Axis axis = Axis.X;
        public RangedInt step = new RangedInt(0, -1, 1);
        public virtual bool performing => _performing;

        public override void Start() {
            base.Start();
            origin = transform.position[(int) axis];
        }

        public virtual void Perform(int step) {
            if(performing) return;

            int axisIndex = (int) axis;
            int direction = step < 0 ? -1 : 1;
            var offset = axis == Axis.X ? Vector3.right
                : (axis == Axis.Y ? Vector3.up : Vector3.forward);
            this.step.value += step;
            float targetPos = origin + (
                distance * this.step.value);

            this.Run(
                (e) => transform.position[axisIndex] != targetPos,
                onProgress: (e) => {
                    _performing = true;
                    var trans = offset * speed * direction * timeScale
                        * Time.deltaTime * Time.timeScale;
                    transform.Translate(trans);
                    
                    var pos = transform.position;
                    pos[axisIndex] = direction == -1
                        ? Mathf.Max(pos[axisIndex], targetPos)
                        : Mathf.Min(pos[axisIndex], targetPos);
                    transform.position = pos;
                    return Time.deltaTime * timeScale;
                },
                onFinish: (e) => _performing = false,
                fixedUpdate: true
            );
        }
    }
}