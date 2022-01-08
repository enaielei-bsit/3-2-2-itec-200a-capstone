/*
 * Date Created: Friday, January 7, 2022 1:55 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Utilities;

namespace Ph.CoDe_A.Lakbay.Core {
    public class SettingsUI : Controller {
        [Header("Audio")]
        public Slider masterVolume;
        public Slider musicVolume;
        public Slider soundVolume;

        [Header("Video")]
        public TMP_Dropdown quality;
        public Toggle left;
        public Toggle right;
        public Toggle both;

        [Header("Accessibility")]
        public TMP_Dropdown language;

        public virtual new IEnumerator Start() {
            yield return new WaitUntil(() => Initialization.finished);

            // Audio...
            var audio = Session.audioController;
            if(audio) {
                masterVolume?.onValueChanged.AddListener(
                    (v) => audio.masterVolume = v);
                musicVolume?.onValueChanged.AddListener(
                    OnVolumeChange("musicVolume"));
                soundVolume?.onValueChanged.AddListener(
                    OnVolumeChange("soundVolume"));
            }

            // Video
            if(quality) {
                quality.ClearOptions();
                quality.AddOptions(QualitySettings.names.ToList());
            }

            // Accessibility
            if(language) {
                language.ClearOptions();
                var languages = Helper.locales.Select((l) => l.LocaleName)
                    .ToList();
                language.AddOptions(languages);
            }
        }

        public override void Update() {
            base.Update();
            var settings = Session.settingsController?.settings;
            var audio = Session.audioController;

            // Audio...
            if(audio) {
                masterVolume?.SetValueWithoutNotify(audio.masterVolume);
                musicVolume?.SetValueWithoutNotify(
                    audio.GetVolume("musicVolume"));
                soundVolume?.SetValueWithoutNotify(
                    audio.GetVolume("soundVolume"));
            }

            // Video...
            if(settings != null && settings.video != null) {
                if(quality) {
                    var names = QualitySettings.names;
                    int index = Array.IndexOf(names, settings.video.quality);
                    quality.SetValueWithoutNotify(index);
                }

                if(settings.video.orientation == Orientation.Left)
                    left?.SetIsOnWithoutNotify(true);
                else if(settings.video.orientation == Orientation.Right)
                    right?.SetIsOnWithoutNotify(true);
                else if(settings.video.orientation == Orientation.Both)
                    both?.SetIsOnWithoutNotify(true);
            }

            // Accessibility...
            if(settings != null && settings.accessibility != null) {
                var locales = Helper.locales;
                var selected = locales.FirstOrDefault((l)
                    => l.Identifier.Code == settings.accessibility.language);
                if(selected != null) {
                    language?.SetValueWithoutNotify(
                        Array.IndexOf(locales, selected));
                }
            }
        }

        public virtual UnityAction<float> OnVolumeChange(string name) {
            var audio = Session.audioController;
            if(!string.IsNullOrEmpty(name) && audio) {
                return (float v) => {
                    audio.SetVolume(name, v, true);
                    SaveSettings();
                };
            }
            return null;
        }

        public virtual void SaveSettings() {
            var settings = Session.settingsController;
            if(settings) {
                settings.UpdateSettings();
                settings.ApplySettings();
                settings.Save();
            }
        }

        public virtual void SetQuality(int index) {
            var settings = Session.settingsController?.settings;
            if(settings != null && settings.video != null) {
                settings.video.quality = QualitySettings.names[index];
            }
            SaveSettings();
        }

        public virtual void SetOrientation(Orientation orientation) {
            var settings = Session.settingsController?.settings;
            if(settings != null && settings.video != null) {
                settings.video.orientation = orientation;
            }
            SaveSettings();
        }

        public virtual void SetOrientation(int orientation) {
            SetOrientation((Orientation) orientation);
            SaveSettings();
        }

        public virtual void SetLanguage(int index) {
            var locales = Helper.locales;
            if(!index.Within(0, locales.Length - 1)) return;
            var settings = Session.settingsController?.settings;
            if(settings != null && settings.accessibility != null) {
                settings.accessibility.language = locales[index].Identifier.Code;
            }
            SaveSettings();
        }
    }
}