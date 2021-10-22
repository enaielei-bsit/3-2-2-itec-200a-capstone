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
    public class Player : Core.Entity {
        public override float timeScale {
            get => base.timeScale;
            set {
                if(travel) travel.timeScale = value;
                if(slide) slide.timeScale = value;
                if(buffable) buffable.timeScale = value;
                if(caster) caster.timeScale = value;
                base.timeScale = value;
            }
        }
        public virtual Travel travel => GetComponentInChildren<Travel>();
        public virtual Slide slide => GetComponentInChildren<Slide>();
        public virtual Buffable buffable => GetComponentInChildren<Buffable>();
        public virtual Caster caster => GetComponentInChildren<Caster>();

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