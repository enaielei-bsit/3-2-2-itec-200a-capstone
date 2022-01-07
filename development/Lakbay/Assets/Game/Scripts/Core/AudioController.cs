/*
 * Date Created: Friday, January 7, 2022 2:06 PM
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
    [Serializable]
    public class Audio {
        protected float _masterVolume = -1.0f; 
        public virtual float masterVolume {
            get => _masterVolume;
            set {
                value = Mathf.Clamp(value, 0.0f, 1.0f);
                if(_masterVolume == value) return;
                _masterVolume = value;
            }
        }
        protected float _musicVolume = -1.0f; 
        public virtual float musicVolume {
            get => _musicVolume;
            set {
                value = Mathf.Clamp(value, 0.0f, 1.0f);
                if(_musicVolume == value) return;
                _musicVolume = value;
            }
        }
        protected float _soundVolume = -1.0f; 
        public virtual float soundVolume {
            get => _soundVolume;
            set {
                value = Mathf.Clamp(value, 0.0f, 1.0f);
                if(_soundVolume == value) return;
                _soundVolume = value;
            }
        }

        public Audio() : this(1.0f) {}

        public Audio(
            float masterVolume=1.0f,
            float musicVolume=1.0f, float soundVolume=1.0f) {
            Update(masterVolume, musicVolume, soundVolume);
        }

        public virtual void Update(
            float masterVolume=1.0f,
            float musicVolume=1.0f, float soundVolume=1.0f) {
            this.masterVolume = masterVolume;
            this.musicVolume = musicVolume;
            this.soundVolume = soundVolume;
        }
    }

    public class AudioController : Controller {
        public new Audio audio = new Audio(-1.0f, -1.0f, -1.0f);
    }
}