/*
 * Date Created: Monday, November 22, 2021 11:46 AM
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
    public class Slide : Core.DirectionalMovement {
        protected int _index;
        public int index => _index;
        public int minIndex = -1;
        public int maxIndex = 1;
        public float offset = 4.0f;
        [Min(0.001f)]
        public float duration = 0.2f;

        public override void Update() {
            base.Update();

            bool left = Input.GetKeyUp(KeyCode.LeftArrow);
            bool right = Input.GetKeyUp(KeyCode.RightArrow);

            if(left) PerformLeft(offset, duration);
            if(right) PerformRight(offset, duration);
        }

        public virtual void Perform(float offset, float duration) {
            if(performing) return;

            float initial = transform.position.x;
            float target = initial + offset;

            duration = Mathf.Max(0.001f, duration);
            x.speed = offset / duration;
            
            if((x.direction == -1 && index == minIndex)
                || x.direction == 1 && index == maxIndex) return;

            _index += x.direction;

            Perform(true);
            this.Run(
                duration,
                onProgress: (d, e) => {
                    return Time.deltaTime * timeScale;
                },
                onFinish: (d, e) => {
                    Perform(false);
                    var pos = transform.position;
                    pos.x = target;
                    transform.position = pos;
            });
        }

        public virtual void PerformLeft(float offset, float duration) =>
            Perform(Mathf.Abs(offset) * -1, duration);

        public virtual void PerformRight(float offset, float duration) =>
            Perform(Mathf.Abs(offset), duration);
    }
}