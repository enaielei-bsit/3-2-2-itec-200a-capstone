/*
 * Date Created: Thursday, October 14, 2021 10:01 AM
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
    public class Buffable : Core.Controller {
        public float timeScale = 1.0f;
        protected readonly Dictionary<Buff, Coroutine> _buffs =
            new Dictionary<Buff, Coroutine>();

        public Transform root;
        public List<MaterialReference> mainMaterials = new List<MaterialReference>();
        public virtual Buff[] buffs => _buffs.Keys.ToArray();

        public override void Awake() {
            base.Awake();
            if(!root) root = transform;
        }

        public virtual Buff Add(
            Caster caster, Skill skill, Buff buff,
            float duration=-1, bool removePrevious=true) {
            if(!root) return null;
            if(removePrevious) Remove(caster, skill, buff.GetType());
            var newBuff = Instantiate(buff, root.transform);

            newBuff.OnAdd(caster, this, skill, duration);
            _buffs[newBuff] = this.Run(
                (e) => duration < 0 || e < duration,
                onProgress: (e) => {
                    newBuff?.OnLinger(caster, this, skill, duration, e);
                    return Time.deltaTime * timeScale;
                },
                onFinish: (e) => Remove(caster, skill, newBuff)
            );

            return newBuff;
        }

        public virtual void Remove(Caster caster, Skill skill, Type type) {
            var buffs = _buffs.Keys.Where((b) => b.GetType() == type).ToArray();
            foreach(var buff in buffs) {
                Remove(caster, skill, buff);
            }
        }

        public virtual void Remove<T>(Caster caster, Skill skill)
            where T : Buff  => Remove(caster, skill, typeof(T));

        public virtual void Remove(Caster caster, Skill skill, Buff buff) {
            if(!_buffs.ContainsKey(buff)) return;
            var coroutine = _buffs[buff];
            StopCoroutine(coroutine);
            _buffs.Remove(buff);
            buff.OnRemove(caster, this, skill);
            Destroy(buff.gameObject);
        }

        public override void Update() {
            base.Update();
        }
    }
}