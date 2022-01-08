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
    using UnityEngine.Localization.Settings;
    using Utilities;

    public enum Orientation {
        Left, Right, Both
    }

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

        [Serializable]
        public class Video {
            public string quality = "Low";
            public Orientation orientation = Orientation.Left;

            public Video() {}

            public Video(string quality="Low", Orientation orientation=Orientation.Left) {
                this.quality = quality;
                this.orientation = orientation;
            }
        }

        [Serializable]
        public class Accessibility {
            public string language = "en";

            public Accessibility() {}
            
            public Accessibility(string language="en") {
                this.language = language;
            }
        }

        public Audio audio = new Audio();
        public Video video = new Video();
        public Accessibility accessibility = new Accessibility();

        public Settings() {}

        public Settings(Audio audio, Video video, Accessibility accessibility) {
            this.audio = audio ?? new Audio();
            this.video = video ?? new Video();
            this.accessibility = accessibility ?? new Accessibility();
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

            // Update Video..
            // settings.video.orientation = GetOrientation();
            // var names = QualitySettings.names;
            // settings.video.quality = names[QualitySettings.GetQualityLevel()];

            // // Update Accessibility
            // settings.accessibility.language =
            //     LocalizationSettings.SelectedLocale.Identifier.Code;
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
            
            // Audio
            if(audio && settings.audio != null) {
                audio.masterVolume = settings.audio.GetVolume("masterVolume");
                foreach(var vol in audio.volumes) {
                    float volume = settings.audio.GetVolume(vol.name);
                    audio.SetVolume(
                        vol.name,
                        volume,
                        true
                    );
                }
            }

            // Video
            if(settings.video != null) {
                int quality = Array.IndexOf(
                    QualitySettings.names, settings.video.quality);
                QualitySettings.SetQualityLevel(quality);
                SetOrientation(settings.video.orientation);
            }

            // Accessibility
            if(settings.accessibility != null) {
                var selected = Helper.locales.FirstOrDefault(
                    (l) => l.Identifier.Code == settings.accessibility.language);
                if(selected != null) {
                    LocalizationSettings.SelectedLocale = selected;
                }
            }
        }

        public virtual Settings.Audio GetAudioDefault() {
            var volumes = new Dictionary<string, float>();
            volumes.Add("masterVolume", 1.0f);
            if(Session.audioController) {
                foreach(var vol in Session.audioController.volumes) {
                    volumes.Add(vol.name, 0.5f);
                }
            }

            return new Settings.Audio(volumes);
        }

        public virtual Settings.Video GetVideoDefault() {
            return new Settings.Video();
        }

        public virtual Settings.Accessibility GetAccessibilityDefault() {
            return new Settings.Accessibility();
        }

        public virtual Settings GetDefault() {
            var settings = new Settings(
                GetAudioDefault(),
                GetVideoDefault(),
                GetAccessibilityDefault()
            );

            return settings;
        }

        public virtual void SetOrientation(Orientation orientation) {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;

            if(orientation == Orientation.Left || orientation == Orientation.Both)
                Screen.autorotateToLandscapeLeft = true;
            if(orientation == Orientation.Right || orientation == Orientation.Both)
                Screen.autorotateToLandscapeRight = true;
        }

        public virtual Orientation GetOrientation() {
            return Screen.autorotateToLandscapeLeft && Screen.autorotateToLandscapeRight
                ? Orientation.Both : (
                    Screen.autorotateToLandscapeLeft ? Orientation.Left
                    : Orientation.Right
                );
        }
    }
}