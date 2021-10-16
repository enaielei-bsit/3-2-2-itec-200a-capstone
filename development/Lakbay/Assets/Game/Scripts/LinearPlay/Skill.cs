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

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    [Serializable]
    public class Skill {
        public Sprite image;
        public string label = "";
        public string description = "";
        public bool stackable = false;
        public int instances = -1;
        public float duration = -1;
        public Buff buff;

        public bool instanced {
            get => instances >= 0;
            set => instances = !value ? -1 : instances; 
        }

        public Skill(
            Sprite image,
            string label,
            string description,
            bool stackable,
            int instances,
            float duration,
            Buff buff) {
            this.image = image;
            this.label = label;
            this.description = description;
            this.stackable = stackable;
            this.instances = instances;
            this.duration = duration;
            this.buff = buff;
        }

        public Skill(
            Sprite image, string label, string description,
            float duration, Buff buff)
            : this(
                image,
                label,
                description,
                false,
                -1,
                duration,
                buff
            ) {}

        public Skill(string label, string description, float duration, Buff buff)
            : this(default, label, description, duration, buff) {}

        public Skill(string label, string description, Buff buff)
            : this(label, description, -1, buff) {}

        public void Cast(Buffable buffable) {
            if(instanced && instances <= 0) return;
            instances--;
            buffable.Add(buff, duration, !stackable);
            buffable.printLog(@$"Casted: {buff.GetType().Name}, Instances remaining: {instances}.");
        }
    }
}