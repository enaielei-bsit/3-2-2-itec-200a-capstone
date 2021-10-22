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

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    public class Buffable : Core.Entity {
        protected readonly Dictionary<Buff, Coroutine> _buffs =
            new Dictionary<Buff, Coroutine>();

        public GameObject root;
        public List<MaterialReference> mainMaterials = new List<MaterialReference>();
        public virtual Buff[] buffs => _buffs.Keys.ToArray();

        public override void Awake() {
            base.Awake();
            if(!root) root = gameObject;
        }

        public virtual void Add(
            Buff buff, float duration=-1, bool removePrevious=true) {
            if(!root) return;
            if(removePrevious) Remove(buff.GetType());
            var newBuff = Instantiate(buff, root.transform);

            newBuff.OnAdd(this, duration);
            _buffs[newBuff] = this.Run(
                (e) => duration < 0 || e < duration,
                onProgress: (e) => {
                    newBuff?.OnLinger(this, duration, e);
                    return Time.deltaTime * timeScale;
                },
                onFinish: (e) => Remove(newBuff)
            );
        }

        public virtual void Remove(Type type) {
            var buffs = _buffs.Keys.Where((b) => b.GetType() == type).ToArray();
            foreach(var buff in buffs) {
                Remove(buff);
            }
        }

        public virtual void Remove<T>() where T : Buff  => Remove(typeof(T));

        public virtual void Remove(Buff buff) {
            if(!_buffs.ContainsKey(buff)) return;
            var coroutine = _buffs[buff];
            StopCoroutine(coroutine);
            _buffs.Remove(buff);
            buff.OnRemove(this);
            Destroy(buff.gameObject);
        }

        public override void Update() {
            base.Update();
        }
    }
}