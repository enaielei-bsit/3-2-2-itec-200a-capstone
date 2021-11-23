/*
 * Date Created: Tuesday, November 23, 2021 10:50 AM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    [RequireComponent(typeof(Collider))]
    public class Repeater : Core.Controller {
        protected bool _triggered = false;
        public virtual bool triggered => _triggered;
        protected bool _occupied = false;
        public virtual bool occupied => _occupied;

        public RepeaterHandler handler;

        public override void Awake() {
            base.Awake();
        }

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var trigger = GetTrigger(collider);
            if(trigger && !_triggered) {
                _triggered = true;
            }
        }

        public override void OnTriggerStay(Collider collider) {
            base.OnTriggerStay(collider);
            var trigger = GetTrigger(collider);
            if(trigger) _occupied = true;
        }

        public override void OnTriggerExit(Collider collider) {
            base.OnTriggerExit(collider);
            var trigger = GetTrigger(collider);
            if(trigger) {
                _occupied = false;

                if(_triggered) {
                    _triggered = false;
                    handler?.Repeat();
                }
            }
        }

        public static RepeaterTrigger GetTrigger(Collider collider) {
            var trigger = collider.GetComponentInParent<RepeaterTrigger>();
            if(trigger && !trigger.includeChildren
                && collider.transform != trigger.transform) return null;

            return trigger;
        }
    }
}