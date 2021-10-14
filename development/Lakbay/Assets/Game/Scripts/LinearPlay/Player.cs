/*
 * Date Created: Sunday, October 10, 2021 8:11 AM
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

using Cinemachine;
using static Cinemachine.CinemachinePath;

using Utilities;

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

        public bool instanced => instances >= 0;

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
            buffable.printLog($"Casted: {buff.GetType().Name}, Instances remaining: {instances}.");
        }
    }

    public class Player : Core.Entity {
        public override float timeScale {
            get => base.timeScale;
            set {
                if(travel) travel.timeScale = value;
                if(slide) slide.timeScale = value;
                if(buffable) buffable.timeScale = value;
                base.timeScale = value;
            }
        }

        public List<Skill> skills = new List<Skill>();

        public virtual Travel travel => GetComponentInChildren<Travel>();
        public virtual Slide slide => GetComponentInChildren<Slide>();
        public virtual Buffable buffable => GetComponentInChildren<Buffable>();

        public virtual void StartTravel() => travel.Perform(true);

        public virtual void StopTravel() => travel.Perform(false);

        public virtual void ToggleTravel() {
            if(travel.performing) travel.Perform(false);
            else travel.Perform(true);
        }

        public virtual void Travel(bool toggle) {
            if(toggle) StartTravel();
            else StopTravel();
        }

        public virtual void SlideLeft() => slide?.Perform(-1);

        public virtual void SlideRight() => slide?.Perform(1);

        public virtual Skill GetSkillWithBuff(Type type) {
            var skill = skills.Find((s) => s.buff?.GetType() == type);
            return skill;
        }

        public virtual Skill GetSkillWithBuff<T>() where T : Buff
            => GetSkillWithBuff(typeof(T));

        public override void Update() {
            base.Update();
            if(travel) {
                bool spaced = Input.GetKeyUp(KeyCode.Space);
                if(spaced) ToggleTravel();
            }

            if(slide) {
                int dir = Input.GetKeyUp(KeyCode.LeftArrow)
                    ? -1 : (Input.GetKeyUp(KeyCode.RightArrow) ? 1 : 0);
                if(dir == -1) SlideLeft();
                else if(dir == 1) SlideRight();
            }
        }
    }
}