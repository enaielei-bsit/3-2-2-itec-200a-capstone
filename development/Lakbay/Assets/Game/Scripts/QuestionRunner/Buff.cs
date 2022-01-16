/*
 * Date Created: Thursday, October 14, 2021 10:09 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

namespace Ph.CoDe_A.Lakbay.QuestionRunner
{
    public abstract class Buff : Core.Controller
    {
        protected float _progress = 0.0f;
        public virtual float progress => _progress;

        public abstract void OnAdd(
            Caster caster, Buffable target, Skill skill,
            float duration);
        public virtual void OnLinger(
            Caster caster, Buffable target, Skill skill,
            float duration, float elapsedTime)
        {
            _progress = duration > 0.0f ? elapsedTime / duration : 0.0f;
        }
        public abstract void OnRemove(
            Caster caster, Buffable target, Skill skill);
    }
}