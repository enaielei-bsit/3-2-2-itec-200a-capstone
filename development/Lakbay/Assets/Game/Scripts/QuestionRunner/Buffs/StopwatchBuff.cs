/*
 * Date Created: Thursday, October 14, 2021 10:05 PM
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
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Buffs {
    public class StopwatchBuff : Buff {

        public float speedFactor = 0.5f;
        protected Volume _volume;
        public Volume volume;

        public override void OnAdd(
            Caster caster, Buffable target, Skill skill,
            float duration) {
            if(volume) _volume = Instantiate(volume, transform);
            var player = target.GetComponent<QRPlayer>();
            if(player && player.travel) player.travel.timeScale *= speedFactor;
        }

        public override void OnLinger(
            Caster caster, Buffable target, Skill skill,
            float duration, float elapsedTime) {
            base.OnLinger(caster, target, skill, duration, elapsedTime);
        }

        public override void OnRemove(
            Caster caster, Buffable target, Skill skill) {
            var player = target.GetComponent<QRPlayer>();
            if(player && player.travel) player.travel.timeScale /= speedFactor;
        }
    }
}