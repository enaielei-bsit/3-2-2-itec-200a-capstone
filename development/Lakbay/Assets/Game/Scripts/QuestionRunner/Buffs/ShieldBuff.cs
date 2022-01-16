/*
 * Date Created: Thursday, October 14, 2021 10:52 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Buffs
{
    public class ShieldBuff : Buff
    {
        protected readonly List<Material> _materials = new List<Material>();

        public Material effect;
        protected Volume _volume;
        public Volume volume;

        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            var obstacle = collider.GetComponentInParent<Spawns.ObstacleSpawn>();
            if (obstacle)
            {
                obstacle.collided = true;
                obstacle.Break();
            }
        }

        public override void OnAdd(
            Caster caster, Buffable target, Skill skill,
            float duration)
        {
            if (volume) _volume = Instantiate(volume, transform);
            _materials.Clear();
            for (int i = 0; i < target.mainMaterials.Count; i++)
            {
                var material = target.mainMaterials[i];
                _materials.Add(material.material);
                material.material = effect;
            }
        }

        public override void OnLinger(
            Caster caster, Buffable target, Skill skill,
            float duration, float elapsedTime)
        {
            base.OnLinger(caster, target, skill, duration, elapsedTime);
        }

        public override void OnRemove(
            Caster caster, Buffable target, Skill skill)
        {
            for (int i = 0; i < target.mainMaterials.Count; i++)
            {
                if (_materials.Count == 0) break;
                var material = target.mainMaterials[i];
                material.material = _materials.Pop(0);
            }
        }
    }
}