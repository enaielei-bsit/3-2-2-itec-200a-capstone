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
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;

    [Serializable]
    public struct TimeSkybox {
        public TimeOfDay time;
        public Material skybox;
        public Light sun;
    }

    public class QRPlayer : Player {
        protected readonly List<float> _timeScales = new List<float>();
        
        public virtual int level => Session.qrLevel
            ? Session.qrLevelIndex + 1 : 0;
        public virtual int levels => Session.qrLevels.Count();
        public virtual int goal => Session.qrLevel
            ? Session.qrLevel.questions.Count : 0;
        public virtual int score => Session.qrScore;
        public virtual int maxScore => Session.qrMaxScore;

        [Header("Level")]
        public MessageBoxUI notification;
        public MessageBoxUI messageBoxUI;
        public GameMenuUI gameMenuUI;
        public GameOverUI gameOverUI;
        public PrePlayUI prePlayUI;
        public QRPostPlayUI qrPostPlayUI;
        public QRInGameUI qrInGameUI;
        public QuestionUI questionUI;
        public Transform initial;
        [HideInInspector]
        public RepeaterHandler repeaterHandler;

        [Header("Events")]
        public LocalizeTextAssetEvent questionsEvent;

        [Header("Gameplay")]
        public Travel travel;
        public Slide slide;
        public Buffable buffable;
        public Caster caster;
        public new CinemachineVirtualCamera camera;
        public int maxLives = 3;
        [Min(0)]
        public int lives = 3;

        [Space]
        public List<TimeSkybox> times = new List<TimeSkybox>();

        [Space]
        public LocalizedString intro;

        public override void Awake() {
            base.Awake();
        }

        public override void Update() {
            base.Update();
        }

        public virtual void InitializeLevels() {
            var levels = Session.database.Get<QRLevel>().Values
                .Where((l) => l.category == Session.mode)
                .OrderBy((l) => l.index);
            Session.qrLevels.Clear();
            Session.qrLevels.AddRange(levels);
            foreach(var level in Session.qrLevels) {
                level.LoadQuestions(level.questionsFile?.LoadAsset());
            }
            Session.qrLevelIndex = 0;
        }

        public override void Build() {
            base.Build();
            if(initial) {
                if(Session.qrLevelIndex == -1) InitializeLevels();

                if(Session.qrLevel) {
                    ResetLevel();
                    Session.qrLevel.player = this;

                    var level = Session.qrLevel;
                    repeaterHandler = Instantiate(
                        level._repeaterHandler, initial);
                    var questions = level.questions;
                    Session.qrLevel.repeaterHandler = repeaterHandler;

                    repeaterHandler.Build();
                    Session.loadingScreen?.Monitor(repeaterHandler);

                    qrInGameUI?.Build();

                    // Update questionUI if it is present.
                    questionsEvent?.Subscribe(
                            Session.qrLevel.questionsFile, UpdateQuestionUI);

                    SetTime(Session.qrLevel.time);
                    prePlayUI?.Show();
                    if(Session.qrLevelIndex == 0)
                        messageBoxUI?.ShowMessage(intro);
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

        public override void Pause() {
            _Save();
            caster.timeScale = 0.0f;
            buffable.timeScale = 0.0f;
            travel.timeScale = 0.0f;
            slide.timeScale = 0.0f;
        }

        public virtual void Pause(bool screen) {
            gameMenuUI?.gameObject.SetActive(screen);
            Pause();
        }

        public override void Resume() {
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

        public virtual void Resume(bool screen) {
            gameMenuUI?.gameObject.SetActive(screen);
            Resume();
        }

        public virtual void Proceed() {
            if(Session.qrLevelIndex != Session.qrLevels.Count - 1) {
                Session.qrLevelIndex++;
                LoadScene();
            } else {
                LoadScene(BuiltScene.Blowbagets);
            }
        }

        public virtual void ResetLevel(int index) {
            if(index.Within(0, Session.qrLevels.Count - 1))
                Session.qrLevels[index].Reset();
        }

        public virtual void ResetLevel() => ResetLevel(Session.qrLevelIndex);

        public virtual void Play(bool screen) {
            prePlayUI?.gameObject.SetActive(screen);
            travel?.Perform(true);
        }

        public virtual void Restart(bool current) {
            if(current) {
                ResetLevel();
            } else {
                Session.qrLevelIndex = 0;
                foreach(int level in Session.qrLevels.Select((l, i) => i))
                    ResetLevel(level);
            }
            Restart();
        }

        public override void End() {
            Session.qrLevels.Clear();
            base.End();
        }

        public virtual void UpdateQuestionUI(TextAsset asset) {
            if(questionUI) {
                if(questionUI.gameObject.activeSelf) {
                    questionUI.Build();
                }
            }
        }

        public virtual void SetTime(TimeOfDay time) {
            var ftime = times.Find((t) => t.time == time);
            if(ftime.skybox) {
                var sun = RenderSettings.sun;
                if(sun) {
                    if(ftime.sun) {
                        Destroy(sun.gameObject);
                        Instantiate(ftime.sun);
                    }
                }
                RenderSettings.skybox = ftime.skybox;
            }
        }
    }
}