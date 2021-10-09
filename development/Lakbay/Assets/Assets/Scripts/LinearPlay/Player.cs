/*
 * Date Created: Friday, October 8, 2021 3:43 PM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Utilities;

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    using Core;
    using Pixelplacement;

    [System.Serializable]
    public struct Slide {
        // Meter per Second
        [SerializeField]
        private float _speed;
        public float speed { get => _speed; set => _speed = value; }
        [SerializeField]
        private float _distance;
        public float distance { get => _distance; set => _distance = value; }
        [SerializeField]
        private RangedInt _step;
        public RangedInt step { get => _step; set => _step = value; }
    }

    [System.Serializable]
    public struct Travel {

    }

    public class Player : Entity {
        protected bool _sliding = false;
        public virtual bool sliding => _sliding;
        public Slide slide;

        public virtual IEnumerator SlideEnumerator(int step, int axis=0) {
            axis = Mathf.Clamp(axis, 0, 2);
            int dir = step < 0 ? -1 : 1;
            var step_ = slide.step;
            step_.value += step;
            slide.step = step_;

            var offset = axis == 0 ? Vector3.right
                : (axis == 1 ? Vector3.up : Vector3.forward);
            float targetDistance = slide.step.value * slide.distance;
            while(transform.position[axis] != targetDistance) {
                _sliding = true;
                transform.Translate(
                    offset * dir * slide.speed * timeScale * Time.fixedDeltaTime
                );
                var pos = transform.position;
                pos[axis] = dir == -1 ? Mathf.Max(pos[axis], targetDistance)
                    : Mathf.Min(pos[axis], targetDistance);
                transform.position = pos;
                yield return new WaitForFixedUpdate();
            }
            _sliding = false;
        }

        public virtual void Slide(int step, int axis=0) {
            if(!sliding) StartCoroutine(SlideEnumerator(step, axis));
        }

        public virtual void SlideLeft() => Slide(-1);

        public virtual void SlideRight() => Slide(1);

        public override void Update() {
            int slideStep = Input.GetKeyUp(KeyCode.LeftArrow)
                ? -1 : (Input.GetKeyUp(KeyCode.RightArrow) ? 1 : 0);
            if(slideStep == -1) SlideLeft();
            if(slideStep == 1) SlideRight();
        }
        protected float initialLength;

        public override void Start() {
            base.Awake();
            var spline = GameObject.FindObjectOfType<Utilities.Spline>();
            initialLength = spline.gameObject.GetLastChild().transform.position.z;
            spline.splineFollowers[0].percentage = 0.0f;
        }
        public override void FixedUpdate() {
            base.FixedUpdate();
            var spline = GameObject.FindObjectOfType<Utilities.Spline>();
            if(spline.splineFollowers[0].percentage < 1.0f) {
                spline.splineFollowers[0].percentage += (slide.speed / spline.Length) * Time.fixedDeltaTime;
            } else {
                float prevLength = spline.Length;
                var last = spline.gameObject.GetLastChild();
                var newLast = Instantiate(last, last.transform.parent);
                newLast.name = last.name;
                newLast.transform.Translate(Vector3.forward * initialLength);
                spline.CalculateLength();
                spline.splineFollowers[0].percentage = prevLength / spline.Length;
            }
        }
    }
}