/*
 * Date Created: Thursday, November 4, 2021 6:20 PM
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

using Pixelplacement;
using TMPro;

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Widgets {
    using Core;

    public class QuestionWidget : Content {
        [Serializable]
        public struct TimeTextColor {
            public float progress;
            public Color color;
        }

        public float timeScale = 1.0f;

        public CanvasGroup tweenable;
        public TextMeshProUGUI time;
        public RectTransform choices;
        public ChoiceWidget choice;
        public List<TimeTextColor> timeTextColors = new List<TimeTextColor>();
        public bool shuffledChoices = true;
        public UnityEvent<QuestionWidget, IEnumerable<Choice>> onAnswer =
            new UnityEvent<QuestionWidget, IEnumerable<Choice>>();
        public Question question;
        protected Coroutine _timer;
        public string timeFormat = "00.00";

        [ContextMenu("Clear Question")]
        public override void Clear() {
            base.Clear();
            time?.SetText("99.99");
            if(Application.isPlaying) this.choices.DestroyChildren();
            else this.choices.DestroyChildrenImmediately();
        }

        [ContextMenu("Build Question")]
        public override void Build() {
            Build(question);
        }

        public virtual void Build(Question question) {
            if(question != null) {
                Clear();
                this.question = question;
                Build(question.content);
                if(!choice || !this.choices) return;

                List<Choice> choices = question.choices.ToList();
                if(shuffledChoices) choices = choices.Shuffle().ToList();

                foreach(var choice in choices) {
                    var choiceWidget = Instantiate(
                        this.choice, this.choices.transform);
                    choiceWidget.Build(this, choice);
                }
            }
        }

        public virtual void Show() {
            gameObject.SetActive(true);
            if(!tweenable) return;
            // Tween.CanvasGroupAlpha(tweenable, 0.0f, 1.0f, 0.25f, 0.0f);
            // Tween.LocalScale(
            //     tweenable, Vector3.zero, Vector3.one,
            //     0.25f, 0.0f
            // );
        }

        public virtual void Hide() {
            gameObject.SetActive(false);
            // Tween.LocalScale(
            //     transform, Vector3.zero,
            //     0.25f, 0.0f,
            //     completeCallback: () => gameObject.SetActive(false)
            // );
        }

        public virtual void Answer(params Choice[] choices) {
            if(question == null) return;
            question.Answer(choices);
            if(_timer != null) StopCoroutine(_timer);
            onAnswer?.Invoke(this, choices);
            Hide();
            FindObjectOfType<ImageViewer>()?.Hide();
        }

        public virtual void Answer(params int[] indices) {
            Answer(indices.Select((i) => 
                i > 0 && i < indices.Length ? question.choices[i]
                : null).ToArray());
        }

        public override void Update() {
            base.Update();
            if(_timer != null && time) {
                if(!time.gameObject.activeSelf)
                    time.gameObject.SetActive(true);
                
                time.SetText(
                    Mathf.Min(
                        question.time - question.elapsedTime,
                        99.99f
                    ).ToString(timeFormat)
                );

                float progress = question.elapsedTime / question.time;
                // if(progress <= 0.65f) time.color = timeStarting;
                // else time.color = timeEnding;
                if(timeTextColors != null && timeTextColors.Count > 0) {
                    var ttc = timeTextColors.Find(
                        (ttc) => progress <= ttc.progress);
                    time.color = ttc.color;
                    // Tween.Color(time, ttc.color, 0.25f, 0.0f);
                }

                if(progress == 1.0f) {
                    // No answer...
                    Answer(-1);
                }
            } else {
                if(time.gameObject.activeSelf) time.gameObject.SetActive(false);
            }
        }

        [ContextMenu("Run Question")]
        public virtual void Run() {
            if(time && question != null) {
                if(question.time > 0.0f) {
                    if(_timer != null) StopCoroutine(_timer);

                    _timer = this.Run(
                        question.time,
                        onProgress: (d, e) => {
                            question.elapsedTime = e;
                            return Time.deltaTime * timeScale;
                        },
                        onFinish: (d, e) => question.elapsedTime = d
                    );
                }
            }
        }
    }
}