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

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    public class Travel : Core.Entity {
        protected Coroutine _coroutine;

        // Meter per Second
        public float speed = 20.0f;
        public Axis axis = Axis.Z;
        public AxisDirection axisDirection = AxisDirection.Positive;
        public virtual bool performing => _coroutine != null;

        public virtual void Perform(bool toggle) {
            if(toggle && !performing) {
                int direction = (int) axisDirection;
                var offset = axis == Axis.X ? Vector3.right
                    : (axis == Axis.Y ? Vector3.up : Vector3.forward);

                _coroutine = this.Run(
                    (e) => true,
                    onProgress: (e) => {
                        var trans = offset * speed * direction * timeScale
                            * Time.deltaTime * Time.timeScale;
                        transform.Translate(trans);

                        return Time.deltaTime * timeScale;
                    },
                    fixedUpdate: true
                );
            } else if(!toggle && performing) {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }
}