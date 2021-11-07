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

using TMPro;

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Widgets {
    using Core;

    public class QuestionWidget : ContentBuilder {
        public TextMeshProUGUI time;
        public GameObject choices;
        public ChoiceWidget choice;
        public Color timeStarting = Color.white;
        public Color timeEnding = Color.red;
        public bool shuffledChoices = true;
        public UnityEvent<QuestionWidget, IEnumerable<Choice>> onAnswer =
            new UnityEvent<QuestionWidget, IEnumerable<Choice>>();
        public Question question;
        protected Coroutine _timer;

        [ContextMenu("Build Question")]
        public override void Build() {
            Build(question);
        }

        [ContextMenu("Build Question from File")]
        protected override void BuildFromFile() {
            if(_file) Build(_file.text.DeserializeAsYaml<Question>());
        }

        public virtual void Build(Question question) {
            if(question != null) {
                this.question = question;
                Build(question.content);
                if(!choice || !this.choices) return;
                if(Application.isPlaying) this.choices.DestroyChildren();
                else this.choices.DestroyChildrenImmediately();

                List<Choice> choices = question.choices.ToList();
                if(shuffledChoices) choices = choices.Shuffle().ToList();

                foreach(var choice in choices) {
                    if(choices.Count == 0) break;
                    var choiceWidget = Instantiate(
                        this.choice, this.choices.transform);
                    choiceWidget.Build(this, choice);
                }
            }
        }

        public virtual void Answer(params Choice[] choices) {
            if(question == null) return;
            question.Answer(choices);
            if(_timer != null) StopCoroutine(_timer);
            onAnswer?.Invoke(this, choices);
            gameObject.SetActive(false);
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
                    ).ToString("N2")
                );

                float progress = question.elapsedTime / question.time;
                if(progress <= 0.65f) time.color = timeStarting;
                else time.color = timeEnding;

                if(progress == 1.0f) {
                    // No answer...
                    Answer(-1);
                }
            } else {
                if(time.gameObject.activeSelf) time.gameObject.SetActive(false);
            }
        }

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