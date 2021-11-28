/*
 * Date Created: Sunday, November 28, 2021 4:08 PM
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
    public class Trigger : Controller {
        public bool includeChildren = true;

        public virtual bool IsTheTrigger(Collider collider, Component trigger=null) {
            trigger = !trigger ? collider.GetComponentInParent(GetType()) : trigger;
            return !includeChildren  ? collider.transform == transform
                : trigger == this;
        }
    }

    public static class TriggerExtension {
        public static Component GetTrigger(this Collider collider, Type type) {
            var trigger = collider.GetComponentInParent(type);
            if(trigger) {
                if((trigger as Trigger).IsTheTrigger(collider, trigger)) 
                    return trigger;
            }

            return null;
        }

        public static T GetTrigger<T>(this Collider collider) where T : Trigger {
            return (T) GetTrigger(collider, typeof(T));
        }
    }
}