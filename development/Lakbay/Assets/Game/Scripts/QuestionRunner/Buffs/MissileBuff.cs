/*
 * Date Created: Thursday, October 14, 2021 10:05 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Buffs
{
    public class MissileBuff : Buff
    {
        protected Missile _missile;
        public Missile missile;

        public override void OnAdd(
            Caster caster, Buffable target, Skill skill,
            float duration)
        {
            if (!missile) return;
            float travelDistance = (skill as MissileSkill).travelDistance;
            _missile = Instantiate(missile);
            _missile.transform.position = transform.position;
            _missile.travel.speed = travelDistance / Mathf.Max(duration, 0.001f);
            _missile.travel.Perform(true);
        }

        public override void OnLinger(
            Caster caster, Buffable target, Skill skill,
            float duration, float elapsedTime)
        {
            base.OnLinger(caster, target, skill, duration, elapsedTime);
            if (_missile) _missile.travel.timeScale = target.timeScale;
        }

        public override void OnRemove(
            Caster caster, Buffable target, Skill skill)
        {
            if (_missile)
            {
                _missile.travel.Perform(false);
                Destroy(_missile.gameObject);
            }
        }
    }
}