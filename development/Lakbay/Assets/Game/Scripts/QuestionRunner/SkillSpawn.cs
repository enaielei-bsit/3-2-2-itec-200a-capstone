/*
 * Date Created: Wednesday, October 13, 2021 6:49 AM
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
    using Core;

    [RequireComponent(typeof(Collider))]
    public abstract class SkillSpawn : QRSpawn {
        public bool triggered = false;

        public override bool OnSpawnCheck(
            Core.Spawner spawner, Transform[] locations, Transform location) {
            bool can = base.OnSpawnCheck(spawner, locations, location);
            if(can) {
                var existing =
                    Array.Find(locations, (l) => l.GetComponentInChildren<SkillSpawn>());
                if(existing) return false;
                return true;
            }

            return false;
        }

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var player = collider.GetComponentInParent<QRPlayer>();
            
            if(player && collider.GetTrigger<SpawnTrigger>()) { 
                if(!triggered) {
                    triggered = true;
                    OnTrigger(player);
                }
            }
        }

        public virtual void OnTrigger(QRPlayer player) {
        }
    }

    public abstract class SkillSpawn<T> : SkillSpawn where T : Buff {
        public virtual Type buffType => typeof(T);

        public override void OnTrigger(QRPlayer trigger) {
            base.OnTrigger(trigger);
            var skill = trigger.caster.GetSkillWithBuff(buffType);
            if(skill != null && skill.buff) {
                if(skill.instanced) {
                    skill.instances += 1;
                    printLog($"Player received a skill instance. Instances: {skill.instances}.");
                }
                gameObject.SetActive(false);
            }
        }
    }
}