/*
 * Date Created: Saturday, October 16, 2021 4:16 AM
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
    [CreateAssetMenu(
        fileName="Skill",
        menuName="Game/LinearPlay/Skill"
    )]
    public class Skill : ScriptableObject {
        protected float _cooldownProgress = 0.0f;

        public Sprite image;
        public string label = "";
        public string description = "";
        public bool stackable = false;
        public int instances = -1;
        public float duration = -1;
        public float cooldown = 0.0f;
        public Buff buff;

        public bool instanced {
            get => instances >= 0;
            set => instances = !value ? -1 : instances; 
        }
        public virtual float cooldownProgress => _cooldownProgress;

        public Skill(
            Sprite image,
            string label,
            string description,
            bool stackable,
            int instances,
            float duration,
            float cooldown,
            Buff buff) {
            this.image = image;
            this.label = label;
            this.description = description;
            this.stackable = stackable;
            this.instances = instances;
            this.duration = duration;
            this.cooldown = cooldown;
            this.buff = buff;
        }

        public Skill(
            Sprite image, string label, string description,
            float duration, float cooldown, Buff buff)
            : this(
                image,
                label,
                description,
                false,
                -1,
                duration,
                cooldown,
                buff
            ) {}

        public Skill(string label, string description,
            float duration, float cooldown, Buff buff)
            : this(default, label, description, duration, cooldown, buff) {}

        public Skill(string label, string description, Buff buff)
            : this(label, description, -1, 0.0f, buff) {}

        public virtual void Cast(Buffable buffable) {
            if((instanced && instances <= 0) || cooldownProgress != 0.0f) return;
            instances--;
            buffable.Add(buff, duration, !stackable);
            if(cooldown > 0) {
                buffable.Run(
                    cooldown,
                    onProgress: (d, e) => {
                        _cooldownProgress = e / d;
                        return Time.deltaTime * buffable.timeScale;
                    },
                    onFinish: (d, e) => _cooldownProgress = 0.0f
                );
            }
            buffable.printLog(@$"Casted: {buff.GetType().Name}, Instances remaining: {instances}.");
        }
    }
}