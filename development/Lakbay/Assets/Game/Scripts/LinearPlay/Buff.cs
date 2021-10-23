/*
 * Date Created: Thursday, October 14, 2021 10:09 AM
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

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    public abstract class Buff : Core.Entity {
        public abstract void OnAdd(
            Caster caster, Buffable target, Skill skill,
            float duration);
        public abstract void OnLinger(
            Caster caster, Buffable target, Skill skill,
            float duration, float elapsedTime);
        public abstract void OnRemove(
            Caster caster, Buffable target, Skill skill);
    }
}