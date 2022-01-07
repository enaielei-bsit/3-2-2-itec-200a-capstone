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

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core {
    public class SettingsUI : Controller {
        [Header("Audio")]
        public Slider masterVolume;
        public Slider musicVolume;
        public Slider soundVolume;

        public virtual new IEnumerator Start() {
            yield return new WaitUntil(() => Initialization.finished);
            var audio = Session.audioController;
            if(audio) {
                masterVolume?.onValueChanged.AddListener(
                    (v) => audio.masterVolume = v);
                musicVolume?.onValueChanged.AddListener(
                    OnVolumeChange("musicVolume"));
                soundVolume?.onValueChanged.AddListener(
                    OnVolumeChange("soundVolume"));
            }
        }

        public override void Update() {
            base.Update();
            var audio = Session.audioController;
            if(audio) {
                masterVolume?.SetValueWithoutNotify(audio.masterVolume);
                musicVolume?.SetValueWithoutNotify(
                    audio.GetVolume("musicVolume"));
                soundVolume?.SetValueWithoutNotify(
                    audio.GetVolume("soundVolume"));
            }
        }

        public virtual UnityAction<float> OnVolumeChange(string name) {
            var audio = Session.audioController;
            if(!string.IsNullOrEmpty(name) && audio) {
                return (float v) => audio.SetVolume(name, v, true);
            }
            return null;
        }
    }
}