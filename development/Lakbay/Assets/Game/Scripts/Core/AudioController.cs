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
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
using YamlDotNet.Serialization;

namespace Ph.CoDe_A.Lakbay.Core {
    using Utilities;
    using OnVolumeChange = UnityEvent<AudioController, float, float>;
    
    [Serializable]
    public class VolumeData {
        public string name = "";
        public List<AdditionalVolume> additionals = new List<AdditionalVolume>();
        public OnVolumeChange onChange =
            new OnVolumeChange();
        
        public VolumeData() {}

        public VolumeData(string name) {
            this.name = name;
        }
    }

    [Serializable]
    public class AdditionalVolume {
        public AudioMixer mixer;
        public List<string> volumes = new List<string>();

        public virtual void Set(float value) {
            if(!mixer) return;
            foreach(var vol in volumes) {
                mixer.SetVolume(vol, value);
            }
        }
    }

    public class AudioController : Controller {
        [YamlIgnore]
        public AudioMixer mixer;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        protected float _masterVolume = -1.0f;
        public List<VolumeData> volumes = new List<VolumeData>() {
            new VolumeData("musicVolume"),
            new VolumeData("soundVolume"),
        };
        public virtual float masterVolume {
            get => _masterVolume;
            set {
                value = Mathf.Clamp(value, 0.0f, 1.0f);
                float old = masterVolume;
                _masterVolume = value;
                foreach(var vol in volumes) {
                    if(!lastVolumes.ContainsKey(vol.name))
                        lastVolumes[vol.name] = 0.0f;
                    SetVolume(vol.name, lastVolumes[vol.name],
                        false, vol.additionals, vol.onChange);
                }

                if(old != value) {
                    onMasterVolumeChange?.Invoke(this, old, masterVolume);
                }
            }
        }
        public OnVolumeChange onMasterVolumeChange =
            new OnVolumeChange();

        protected Dictionary<string, float> lastVolumes =
            new Dictionary<string, float>();

        public override void Update() {
            base.Update();
        }

        public override void OnValidate() {
            base.OnValidate();
            masterVolume = _masterVolume;
        }

        public virtual void SetVolume(
            string name,
            float value,
            bool asLastVolume,
            IEnumerable<AdditionalVolume> additionals,
            OnVolumeChange onValueChange) {
            // Clamp the values using the masterVolume.
            value = Mathf.Clamp(value * masterVolume, 0.0f, 1.0f);
            float volume = GetVolume(name);
            if(volume == value) return;
            float old = volume;
            printLog($"Setting {name} to {value}...");
            mixer?.SetVolume(name, value);
            foreach(var additional in additionals) {
                additional.Set(value);
            }
            if(asLastVolume) {
                float newVolume = GetVolume(name);
                lastVolumes[name] = newVolume;
            }

            onValueChange?.Invoke(this, old, volume);
        }

        public virtual void SetVolume(
            string name,
            float value) {
            SetVolume(name, value, false);
        }

        public virtual void SetVolume(
            string name,
            float value,
            bool asLastVolume) {
            var match = volumes.FirstOrDefault((v) => v.name == name);
            SetVolume(name, value, asLastVolume,
                match?.additionals,
                match?.onChange);
        }

        public virtual float GetVolume(string name) {
            if(mixer) return mixer.GetVolume(name);
            return 0.0f;
        }
    }
}