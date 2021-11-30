/*
 * Date Created: Monday, November 29, 2021 6:14 AM
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
    using Utilities;

    public class QRController : Core.Controller {
        public Transform repeaterHandlerLocation;
        [HideInInspector]
        public RepeaterHandler repeaterHandler;
        public Player player;
        protected readonly List<float> _timeScales = new List<float>();

        public override void Awake() {
            base.Awake();
        }

        public override void Start() {
            base.Start();
        }

        public virtual void Build() {
            if(repeaterHandlerLocation) {
                if(Session.qrLevel) {
                    repeaterHandler = Instantiate(
                       Session.qrLevel.repeaterHandler, repeaterHandlerLocation);

                    repeaterHandler.Build();
                }
            }
        }

        protected virtual void _Save(Player player) {
            if(player) {
                _timeScales.Clear();
                _timeScales.AddRange(new float[] {
                    player.caster.timeScale,
                    player.buffable.timeScale,
                    player.slide.timeScale,
                    player.travel.timeScale,
                });
            }
        }

        public virtual void Pause(Player player) {
            if(!player) return;
            _Save(player);
            player.caster.timeScale = 0.0f;
            player.buffable.timeScale = 0.0f;
            player.travel.timeScale = 0.0f;
            player.slide.timeScale = 0.0f;
        }

        public virtual void Resume(Player player) {
            if(player) {
                try {
                    player.caster.timeScale = _timeScales.Pop();
                    player.buffable.timeScale = _timeScales.Pop();
                    player.travel.timeScale = _timeScales.Pop();
                    player.slide.timeScale = _timeScales.Pop();
                } catch {
                    player.caster.timeScale = 1.0f;
                    player.buffable.timeScale = 1.0f;
                    player.travel.timeScale = 1.0f;
                    player.slide.timeScale = 1.0f;
                }
            }
        }

        public virtual void Pause() {
            if(player) Pause(player);
        }

        public virtual void Resume() {
            if(player) Resume(player);
        }
    }
}