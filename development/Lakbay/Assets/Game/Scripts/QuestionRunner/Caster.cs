/*
 * Date Created: Saturday, October 16, 2021 11:18 PM
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
    public class Caster : Core.Controller {
        [SerializeField]
        protected List<Skill> _skills = new List<Skill>();

        public virtual Skill[] skills => _skills.ToArray();

        public override void Awake() {
            base.Awake();
            var newSkills = _skills.Select((s) => Instantiate(s)).ToList();
            _skills.Clear();
            _skills.AddRange(newSkills);
        }

        public virtual Skill Add(Skill skill) {
            _skills.Add(Instantiate(skill));
            return _skills.Last();
        }

        public virtual Skill Add(Type type) {
            _skills.Add(ScriptableObject.CreateInstance(type) as Skill);
            return _skills.Last();
        }

        public virtual Skill Add<T>() {
            return Add(typeof(T));
        }

        public virtual void Remove(Skill skill) {
            while(_skills.Contains(skill)) _skills.Remove(skill);
        }

        public virtual void Clear() => _skills.Clear();

        public virtual Skill GetSkillWithBuff(Type type) {
            var skill = _skills.Find((s) => s.buff?.GetType() == type);
            return skill;
        }

        public virtual Skill GetSkillWithBuff<T>() where T : Buff
            => GetSkillWithBuff(typeof(T));
    }
}