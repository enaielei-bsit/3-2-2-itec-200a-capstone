/*
 * Date Created: Sunday, October 10, 2021 5:52 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Build.Reporting;
using UnityEditor.Localization;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

using Humanizer;

using Utilities;
using static UnityEditor.AddressableAssets.Settings.AddressableAssetSettings;

namespace Ph.CoDe_A.Lakbay {
    [CreateAssetMenu(fileName="Helper", menuName="Game/Editor/Helper")]
    public class Helper : ScriptableObject {
        // public static HelperSettings settings;

        // [MenuItem("Game/Window/Helper")]
        // public static void Open() {
        //     var window = EditorWindow.GetWindow<Helper>("Helper");
        //     window?.Show();
        // }
        [Serializable]
        public struct MarkingAsset {
            public Query query;
            public string group;
            [SerializeField]
            private string _labels;
            public string[] labels {
                get => ToParts(_labels);
                set => string.Join("; ", _labels, value.Join("; "));
            }
        }

        [Serializable]
        public struct Query {
            public string filter;
            [SerializeField]
            private string _ignoredExtensions;
            public string[] ignoredExtensions {
                get => ToParts(_ignoredExtensions);
                set => string.Join("; ", _ignoredExtensions, value.Join("; "));
            }

            public Query(string filter, params string[] ignoredExtensions) {
                this.filter = filter;
                _ignoredExtensions = ignoredExtensions.Join("; ");
            }

            public string[] GetPaths(params string[] folders) {
                var paths = EditorHelper.GetAssetPaths(
                    filter, folders,
                    ignoredExtensions);
                return paths.ToArray();
            }
        }

        [Header("Paths")]
        [SerializeField]
        protected string _GameAssetsPath = "Assets/Game";
        public string GameAssetsPath => _GameAssetsPath;
        [SerializeField]
        protected string _LocalizationAssetsPath = "Localization";
        public string LocalizationAssetsPath => string.Join(
            "/", new string[] {GameAssetsPath, _LocalizationAssetsPath});
        [SerializeField]
        protected string _LocalizationAssetsTablesPath = "Tables";
        public string LocalizationAssetsTablesPath => string.Join(
            "/", new string[] {LocalizationAssetsPath, _LocalizationAssetsTablesPath});
        [SerializeField]
        protected string _BuildPath = "Build";
        public string BuildPath => "Build";
        [SerializeField]
        protected string _BuildDebugPath = "Debug";
        public string BuildDebugPath => string.Join(
            "/", new string[] {BuildPath, _BuildDebugPath});
        [SerializeField]
        protected string _BuildReleasePath = "Release";
        public string BuildReleasePath => string.Join(
            "/", new string[] {BuildPath, _BuildReleasePath});

        public List<MarkingAsset> markingAssets = new List<MarkingAsset>() {
            new MarkingAsset() {
                query = new Query("t:Sprite"),
                group = "Images",
                labels = new string[] {"Image", "Sprite"}
            },
            new MarkingAsset() {
                query = new Query("t:TextAsset", "cs", "dll"),
                group = "Documents",
                labels = new string[] {"Document", "TextAsset"}
            },
            new MarkingAsset() {
                query = new Query("t:AudioClip"),
                group = "Audios",
                labels = new string[] {"Audio", "AudioClip"}
            }
        };

        public List<Query> localizingAssets = new List<Query>();

        public virtual void OnGUI() {
            // HelperSettings.instance = (HelperSettings) EditorGUILayout.ObjectField(
            //     "Settings", HelperSettings.instance, typeof(HelperSettings), true);
            if(GUILayout.Button("Mark Assets as Addressables")) {
                MarkAssetsAsAddressables();
            }
            if(GUILayout.Button("Localize Addressable Assets")) {
                LocalizeAddressableAssets();
            }
            if(GUILayout.Button("Normalize Addresses")) {
                NormalizeAddresses();
            }
        }

        // [MenuItem("Game/Addressables/Mark Assets as Addressables")]
        [ContextMenu("Addressables/Mark Assets as Addressables")]
        public virtual void MarkAssetsAsAddressables() {
            
            try {
                var settings = EditorHelper.addressableAssetSettings;
                var assets = this.markingAssets;
                foreach(var asset in assets) {
                    if(!string.IsNullOrEmpty(asset.query.filter)) {
                        var paths = asset.query.GetPaths(this.GameAssetsPath);

                        foreach(var path in paths) {
                            EditorUtility.DisplayProgressBar(
                                "Mark Assets as Addressables",
                                $"Asset: {path}",
                                (assets.IndexOf(asset) + 1) / assets.Count);

                            var entry =
                                settings.AddEntry(path, asset.group, asset.labels);
                        }
                    }
                }

            } catch {}

            EditorUtility.ClearProgressBar();
        }

        // [MenuItem("Game/Localization/Localize Addressable Assets")]
        [ContextMenu("Localization/Localize Addressable Assets")]
        public virtual void LocalizeAddressableAssets() {
            
            try {
                var settings = EditorHelper.addressableAssetSettings;
                var queries = this.localizingAssets;
                foreach(var query in queries) {
                    if(!string.IsNullOrEmpty(query.filter)) {
                        var paths = query.GetPaths(this.GameAssetsPath);

                        foreach(var path in paths) {
                            EditorUtility.DisplayProgressBar(
                                "Mark Assets as Addressables",
                                $"Asset: {path}",
                                (queries.IndexOf(query) + 1) / queries.Count);

                            var rasset =
                                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                            rasset?.Localize(rasset.GetType().Name, this.LocalizationAssetsTablesPath);
                        }
                    }
                }

                // settings.SetDirty(ModificationEvent.BatchModification,
                //     null, true, true);
            } catch {}

            EditorUtility.ClearProgressBar();
        }

        [MenuItem("Game/Addressables/Normalize Addresses")]
        // [ContextMenu("Addressables/Normalize Addresses")]
        public static void NormalizeAddresses() {
            
            try {
                var settings = EditorHelper.addressableAssetSettings;
                foreach(var group in settings.groups) {
                    if(group.ReadOnly) continue;
                    var entries = group.entries.ToList();

                    foreach(var entry in entries) {
                        EditorUtility.DisplayProgressBar(
                            "Normalize Addressable Addresses",
                            $"Old Address: {entry.address}\nNew Address: {entry.AssetPath}",
                            (entries.IndexOf(entry) + 1) / entries.Count);

                        entry.SetAddress(entry.AssetPath);
                        // settings.SetDirty(ModificationEvent.EntryModified,
                        //     entry.AssetPath, true, true);
                    }
                }

                // settings.SetDirty(ModificationEvent.BatchModification,
                //     null, true, true);
            } catch {}

            EditorUtility.ClearProgressBar();
        }

        [ContextMenu("Build/Android/Debug and Run")]
        public virtual void BuildAndroidDebugAndRun() => Build(this.BuildDebugPath, true);

        [ContextMenu("Build/Android/Release and Run")]
        public virtual void BuildAndroidReleaseAndRun() => Build(this.BuildReleasePath, false);

        [ContextMenu("Build/Android/Debug")]
        public virtual void BuildAndroidDebug() => Build(this.BuildDebugPath, true, false);

        [ContextMenu("Build/Android/Release")]
        public virtual void BuildAndroidRelease() => Build(this.BuildReleasePath, false, false);


        [ContextMenu("Build/Windows/Debug and Run")]
        public virtual void BuildWindowsDebugAndRun() => Build(this.BuildDebugPath, true, platform: BuildTarget.StandaloneWindows);

        [ContextMenu("Build/Windows/Release and Run")]
        public virtual void BuildWindowsReleaseAndRun() => Build(this.BuildReleasePath, false, platform: BuildTarget.StandaloneWindows);

        [ContextMenu("Build/Windows/Debug")]
        public virtual void BuildWindowsDebug() => Build(this.BuildDebugPath, true, false, platform: BuildTarget.StandaloneWindows);

        [ContextMenu("Build/Windows/Release")]
        public virtual void BuildWindowsRelease() => Build(this.BuildReleasePath, false, false, platform: BuildTarget.StandaloneWindows);

        // [MenuItem("Game/Mark Assets as Addressables and Localize")]
        [ContextMenu("Mark Assets as Addressables and Localize")]
        public virtual void MarkAssetsAsAddressablesAndLocalize() {
            MarkAssetsAsAddressables();
            LocalizeAddressableAssets();
        }

        public static void Build(
            string path, bool development=false,
            bool autoRun=true,
            BuildTarget platform=BuildTarget.Android) {
            AddressableAssetSettings.BuildPlayerContent();

            var now = DateTime.Now;
            PlayerSettings.bundleVersion = $"{now.Year:0000}.{now.Month:00}.{now.Day:00}";
            string name = PlayerSettings.productName, version = PlayerSettings.bundleVersion;
            string folder = path;
            
            string pathName = "";
            string outputName = $"{name.ToLower()}-v{version}";
            string platformPath = $"{folder}/{platform}";
            if(platform == BuildTarget.Android) {
                pathName = $"{platformPath}/{outputName}.apk";
            } else {
                pathName = $"{platformPath}/{outputName}";
            }
            var buildPlayerOptions = new BuildPlayerOptions() {
                scenes = EditorBuildSettings.scenes.Select((s) => s.path).ToArray(),
                locationPathName = pathName,
                target = platform,
            };
            
            if(development) buildPlayerOptions.options |= BuildOptions.Development;
            if(autoRun) buildPlayerOptions.options |= BuildOptions.AutoRunPlayer;

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded) {
                Debug.Log(@$"Build succeeded! OutputPath: '{summary.outputPath}', OutputSize: {summary.totalSize} bytes.");
            } else if (summary.result == BuildResult.Failed) {
                Debug.Log($"Build failed!");
            }
        }

        public static string[] ToParts(string str, string separator=";") {
            var parts = new string[] {};
            if(!string.IsNullOrEmpty(str))
                parts = str.Split(';')
                    .Select((e) => e.Trim()).ToArray();
            return parts;
        }
    }
}