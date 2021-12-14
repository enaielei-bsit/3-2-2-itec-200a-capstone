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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    using Core;
    using Widgets;

    public class QuestionUI : Controller {
        [Serializable]
        public struct TimeTextColor {
            public float progress;
            public Color color;
        }

        protected Coroutine _timer;

        public float timeScale = 1.0f;
        public bool readOnly = false;

        public CanvasGroup tweenable;
        public Button dismiss;

        [Header("Time")]
        public Slider timeProgress;
        public TextMeshProUGUI time;
        public string timeFormat = "00.00";
        public List<TimeTextColor> timeTextColors = new List<TimeTextColor>();

        [Header("Question")]
        public Content questionContent;
        public Question question;

        [Header("Choices")]
        public bool shuffledChoices = true;
        public RectTransform choices;
        [SerializeField]
        protected ChoiceWidget _choice;

        [Space]
        public RectTransform legend;

        [Space]
        public UnityEvent<QuestionUI, IEnumerable<Choice>> onAnswer =
            new UnityEvent<QuestionUI, IEnumerable<Choice>>();

        [ContextMenu("Clear")]
        public virtual void Clear() {
            questionContent.Clear();
            time?.SetText("99.99");
            if(Application.isPlaying) this.choices.DestroyChildren();
            else this.choices.DestroyChildrenImmediately();
        }

        [ContextMenu("Build")]
        public virtual void Build() {
            Build(question);
        }

        public virtual void Build(Question question) {
            if(question != null) {
                Clear();
                this.question = question;
                questionContent.Build(question.content);
                if(!_choice || !this.choices) return;

                List<Choice> choices = question.choices.ToList();
                if(shuffledChoices) choices = choices.Shuffle().ToList();

                foreach(var choice in choices) {
                    var choiceWidget = Instantiate(
                        this._choice, this.choices.transform);
                    choiceWidget.Build(this, choice, readOnly);
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

        public virtual void Show(
            Question question,
            UnityAction<QuestionUI,IEnumerable<Choice>> onAnswer=null,
            bool readOnly=false) {
            this.readOnly = readOnly;
            Show();
            Build(question);
            this.onAnswer.RemoveAllListeners();
            if(onAnswer != null) this.onAnswer.AddListener(onAnswer);

            legend?.gameObject.SetActive(readOnly);
            dismiss?.gameObject.SetActive(readOnly);
            if(!readOnly) Run();
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
                if(timeProgress) timeProgress.value = 1.0f - question.progress;

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
        public virtual void Run() => Run(0.0f);

        public virtual void Run(float duration) {
            if(time && question != null) {
                if(question.time > 0.0f) {
                    if(_timer != null) StopCoroutine(_timer);

                    _timer = this.Run(
                        duration <= 0.0f ? question.time : duration,
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