/*
 * Date Created: Sunday, December 19, 2021 10:49 PM
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

namespace Ph.CoDe_A.Lakbay {
    using Utilities;
    using Core;
    using QuestionRunner;
    using QuestionRunner.Widgets;
    using SteppedApplication.Blowbagets;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Components;
    using UnityEngine.Localization.Settings;
    using UnityEngine.SceneManagement;
    using TMPro;

    public class CheatEngine : Controller {
        protected BuiltScene _lastScene;

        public CanvasGroup group;
        public LayoutGroup root;

        [SerializeField]
        protected TextMeshProUGUI _text;
        [SerializeField]
        protected Button _button;

        protected TextMeshProUGUI _qrText;

        public override void Awake() {
            base.Awake();
            if(!Debug.isDebugBuild) gameObject.SetActive(false);
        }

        public override void Update() {
            base.Update();
            group.blocksRaycasts = false;
            if(!Debug.isDebugBuild) {
                group.alpha = 0.0f;
                group.interactable = false;
            } else {
                group.alpha = 1.0f;
                group.interactable = true;

                var current = SceneController.GetCurrent();
                if(current != _lastScene) {
                    Build(current);
                    _lastScene = current;
                }

                Update(current);
            }
        }

        public virtual void Build(BuiltScene scene) {
            if(!root) return;
            root.transform.DestroyChildren();
            if(scene == BuiltScene.QuestionRunner) {
                _qrText = Instantiate(_text, root.transform);
            } else if(scene == BuiltScene.Blowbagets) {
                
            }
        }

        public virtual void Update(BuiltScene scene) {
            if(scene == BuiltScene.QuestionRunner) {
                _qrText?.SetText("Waiting for a Question...");
                var player = FindObjectOfType<QRPlayer>();
                if(player) {
                    if(player.questionUI.gameObject.activeSelf) {
                        var ui = player.questionUI;
                        var choices = ui.choices.GetComponentsInChildren<ChoiceWidget>();
                        var correct = (from c in choices
                            where c.choice.correct
                            select Array.IndexOf(choices, c)).Join(", "); 
                        _qrText?.SetText($"Correct Answer(s): {correct}");
                    }
                }
            } else if(scene == BuiltScene.Blowbagets) {
                
            }
        }
    }
}