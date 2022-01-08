/*
 * Date Created: Friday, January 7, 2022 2:13 PM
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
    using System.IO;
    using Utilities;

    [Serializable]
    public class Settings {
        [Serializable]
        public class Audio {
            public Dictionary<string, float> volumes = new Dictionary<string, float>();

            public Audio() {}

            public Audio(Dictionary<string, float> volumes) {
                volumes = volumes ?? new Dictionary<string, float>();
                foreach(var vol in volumes) {
                    SetVolume(vol.Key, vol.Value);
                }
            }

            public virtual float GetVolume(string volume) {
                if(volumes.TryGetValue(volume, out float value)) {
                    return value;
                }

                return 0.0f;
            }

            public virtual void SetVolume(string volume, float value) {
                volumes[volume] = Mathf.Clamp(value, 0.0f, 1.0f);
            } 
        }

        public Audio audio = new Audio();

        public Settings() {}

        public Settings(Audio audio) {
            this.audio = audio ?? new Audio();
        }
    }

    public class SettingsController : Controller {
        [SerializeField]
        protected string _filePath = "settings.yaml";
        public virtual string filePath => 
            $"{Application.persistentDataPath}/{_filePath}";

        public Settings settings = new Settings();

        public override void Awake() {
            base.Awake();
            settings = GetDefault();
        }

        [ContextMenu("Reset Settings")]
        public virtual void ResetToDefault() {
            settings = GetDefault();
        }

        [ContextMenu("Update Settings")]
        public virtual void UpdateSettings() {
            // Update Audio...
            var audio = Session.audioController;
            var volumes = new Dictionary<string, float>();
            if(audio) {
                volumes.Add("masterVolume", audio.masterVolume);
                foreach(var vol in audio.volumes) {
                    volumes.Add(vol.name, audio.GetVolume(vol.name));
                }
            }
            settings.audio = new Settings.Audio(volumes);
        }

        [ContextMenu("Save Settings")]
        public virtual void Save() {
            Helper.WriteFile(filePath, settings.SerializeAsYaml());
        }

        [ContextMenu("Load Settings")]
        public virtual void Load() {
            if(File.Exists(filePath)) {
                printLog($"`{filePath}` was found. Trying to load it...");
                try {
                    settings = Helper.ReadFile(filePath).DeserializeAsYaml<Settings>();
                    printLog($"Sucessfully loaded the file.");
                } catch {
                    printLog($"There was something wrong with the file. Resetting to default instead...");
                    settings = GetDefault();
                }
            } else {
                printLog($"`{filePath}` was not found. Creating it first using the default...");
                Helper.WriteFile(filePath, GetDefault().SerializeAsYaml());
                printLog("Successfully created the file.");
                Load();
            }

            ApplySettings();
        }

        public virtual void ApplySettings() {
            var audio = Session.audioController;

            if(audio && settings.audio != null) {
                audio.masterVolume = settings.audio.GetVolume("masterVolume");
                foreach(var vol in audio.volumes) {
                    float volume = settings.audio.GetVolume(vol.name);
                    audio.SetVolume(
                        vol.name,
                        volume >= 1.0f ? volume * audio.masterVolume : volume,
                        true
                    );
                }
            }
        }

        public virtual Settings.Audio GetAudioDefault() {
            var volumes = new Dictionary<string, float>();
            volumes.Add("masterVolume", 1.0f);
            if(Session.audioController) {
                foreach(var vol in Session.audioController.volumes) {
                    volumes.Add(vol.name, 1.0f);
                }
            }

            return new Settings.Audio(volumes);
        }

        public virtual Settings GetDefault() {
            var settings = new Settings(
                GetAudioDefault()
            );

            return settings;
        }
    }
}