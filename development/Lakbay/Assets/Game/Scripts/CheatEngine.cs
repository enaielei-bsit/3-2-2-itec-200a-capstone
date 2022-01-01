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
        protected BuiltScene _lastScene = BuiltScene.None;

        public CanvasGroup group;
        public LayoutGroup root;
        public RectTransform scenes;
        public Button toggleLanguage;
        public virtual TextMeshProUGUI toggleLanguageText =>
            toggleLanguage?.GetComponentInChildren<TextMeshProUGUI>();

        [Space]
        [SerializeField]
        protected TextMeshProUGUI _text;
        [SerializeField]
        protected Button _button;

        protected TextMeshProUGUI _qrText;
        protected bool _ready = false;

        public override void Awake() {
            base.Awake();
            if(!Debug.isDebugBuild) gameObject.SetActive(false);
        }

        public new virtual IEnumerator Start() {
            yield return LocalizationSettings.InitializationOperation;
            _ready = true;
        }

        public override void Update() {
            base.Update();
            if(!_ready) return;
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
            if(toggleLanguage) {
                var text = toggleLanguageText;
                text?.SetText(LocalizationSettings.SelectedLocale.name);
                toggleLanguage.onClick.RemoveAllListeners();
                toggleLanguage.onClick.AddListener(ToggleLanguage);
            }

            root.transform.DestroyChildren();
            if(scene == BuiltScene.QuestionRunner) {
                _qrText = Instantiate(_text, root.transform);
            } else if(scene == BuiltScene.Blowbagets) {
                
            }

                    
            if(this.scenes) {
                this.scenes.DestroyChildren();
                var scenes = Enum.GetValues(typeof(BuiltScene)).Cast<BuiltScene>();
                foreach(var sc in scenes) {
                    if((int) sc < 0) continue;
                    var scb = Instantiate(_button, this.scenes);
                    var text = scb.GetComponentInChildren<TextMeshProUGUI>();
                    text.SetText(sc.ToString());
                    scb.onClick.AddListener(LoadScene(sc));
                }
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

        public virtual void ToggleLanguage() {
            var next = GetNextLocale();
            LocalizationSettings.SelectedLocale = next;
            toggleLanguageText?.SetText(LocalizationSettings.SelectedLocale.name);
        }

        public static Locale GetNextLocale(Locale locale) {
            var locales = LocalizationSettings.AvailableLocales.Locales;
            var current = LocalizationSettings.SelectedLocale;
            int index = locales.IndexOf(current);
            int nextIndex = index + 1;
            nextIndex = nextIndex >= locales.Count ? 0 : nextIndex;
            var next = locales[nextIndex];
            return next;
        }

        public static Locale GetNextLocale() =>
            GetNextLocale(LocalizationSettings.SelectedLocale);
        
        public static UnityAction LoadScene(BuiltScene scene) {
            return () => FindObjectOfType<Player>().LoadScene(scene);
        }
    }
}