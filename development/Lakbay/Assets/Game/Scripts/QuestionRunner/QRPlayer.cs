/*
 * Date Created: Monday, November 22, 2021 2:02 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    using Core;

    public class QRPlayer : Controller {
        protected readonly List<float> _timeScales = new List<float>();

        [Header("Level")]
        public QRPrePlayUI prePlayUI;
        public QRInGameUI inGameUI;
        public GameMenuUI gameMenuUI;
        public Transform repeaterHandlerLocation;
        [HideInInspector]
        public RepeaterHandler repeaterHandler;

        [Header("Gameplay")]
        public Travel travel;
        public Slide slide;
        public Buffable buffable;
        public Caster caster;
        public new CinemachineVirtualCamera camera;
        public int maxLives = 3;
        [Min(0)]
        public int lives = 3;

        public override void Awake() {
            base.Awake();
        }

        public override void Update() {
            base.Update();
        }

        public new virtual IEnumerator Start() {
            yield return new WaitUntil(() => Initialization.finished);
            Build();
            prePlayUI.gameObject.SetActive(true);
        }

        public virtual void Build() {
            if(repeaterHandlerLocation) {
                if(Session.qrLevelIndex == 0) {
                    var levels = Session.database.Get<QRLevel>().Values
                        .Where((l) => l.category == Session.mode);
                    Session.qrLevels.Clear();
                    Session.qrLevels.AddRange(levels);
                }

                if(Session.qrLevel) {
                    Session.qrLevel.player = this;

                    var level = Session.qrLevel;
                    repeaterHandler = Instantiate(
                        level._repeaterHandler, repeaterHandlerLocation);
                    var questions = level.questions;
                    Session.qrLevel.repeaterHandler = repeaterHandler;

                    repeaterHandler.Build();
                    Session.loadingScreen?.Monitor(repeaterHandler);

                    inGameUI?.Build();
                }
            }
        }

        protected virtual void _Save() {
            _timeScales.Clear();
            _timeScales.AddRange(new float[] {
                caster.timeScale,
                buffable.timeScale,
                travel.timeScale,
                slide.timeScale,
            });
        }

        public virtual void Pause() {
            _Save();
            caster.timeScale = 0.0f;
            buffable.timeScale = 0.0f;
            travel.timeScale = 0.0f;
            slide.timeScale = 0.0f;
        }

        public virtual void Pause(bool showUI) {
            if(showUI) gameMenuUI?.gameObject.SetActive(true);
            Pause();
        }

        public virtual void Resume() {
            try {
                caster.timeScale = _timeScales.Pop();
                buffable.timeScale = _timeScales.Pop();
                travel.timeScale = _timeScales.Pop();
                slide.timeScale = _timeScales.Pop();
            } catch {
                caster.timeScale = 1.0f;
                buffable.timeScale = 1.0f;
                travel.timeScale = 1.0f;
                slide.timeScale = 1.0f;
            }
        }

        public virtual void Resume(bool hideUI) {
            if(hideUI) gameMenuUI?.gameObject.SetActive(false);
            Resume();
        }

        public virtual void Proceed() {
            if(Session.qrLevelIndex != Session.qrLevels.Count - 1) {
                Session.qrLevelIndex++;

                Session.sceneController.Load(2);
                Session.loadingScreen?.Monitor(Session.sceneController);
            }
        }

        public virtual void ResetLevel(int index) {
            if(index.Within(0, Session.qrLevels.Count - 1))
                Session.qrLevels[index].Reset();
        }

        public virtual void ResetLevel() => ResetLevel(Session.qrLevelIndex);

        public virtual void PrePlay() {
            prePlayUI?.gameObject.SetActive(false);
            travel?.Perform(true);
        }
    }
}