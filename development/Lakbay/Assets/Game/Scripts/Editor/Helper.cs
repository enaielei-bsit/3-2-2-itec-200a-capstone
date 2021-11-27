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
        public struct Asset {
            public string type;
            public string group;
            public string labels;
            public string ignoredExtensions;
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

        [Header("Marking Addressables")]
        public List<Asset> assets = new List<Asset>() {
            new Asset() {
                type = "Sprite", group = "Images", labels = "Image; Sprite"
            },
            new Asset() {
                type = "TextAsset", group = "Documents", labels = "Document; TextAsset",
                ignoredExtensions = "cs; dll"
            },
            new Asset() {
                type = "AudioClip", group = "Audios", labels = "Audio; AudioClip"
            }
        };

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
                foreach(var asset in this.assets) {
                    if(!string.IsNullOrEmpty(asset.type)) {
                        var exts = new string[] {};
                        if(!string.IsNullOrEmpty(asset.ignoredExtensions))
                            exts = asset.ignoredExtensions.Split(";")
                                .Select((e) => e.Trim()).ToArray();

                        var labels = new string[] {};
                        if(!string.IsNullOrEmpty(asset.labels))
                            labels = asset.labels.Split(";")
                                .Select((e) => e.Trim()).ToArray();

                        var paths = EditorHelper.GetAssetPaths(
                            $"t:{asset.type}", new string[] {this.GameAssetsPath},
                            exts);

                        foreach(var path in paths) {
                            EditorUtility.DisplayProgressBar(
                                "Mark Assets as Addressables",
                                $"Asset: {path}",
                                (this.assets.IndexOf(asset) + 1) / this.assets.Count);

                            settings.AddEntry(path, asset.group, labels);
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
                var groups = settings.groups.ToList();

                foreach(var group in groups) {
                    if(group.ReadOnly) continue;

                    var entries = group.entries.ToArray();
                    foreach(var entry in entries) {
                        if(entry == null) continue;

                        EditorUtility.DisplayProgressBar(
                            "Localize Addressable Assets",
                            $"Group: {group.Name}\nAsset: {entry.AssetPath}",
                            (Array.IndexOf(entries, entry) + 1) / entries.Length);

                        var asset = AssetDatabase.LoadAssetAtPath(
                            entry.AssetPath, entry.MainAssetType);
                        asset?.Localize(
                            entry.MainAssetType.Name,
                            this.LocalizationAssetsTablesPath);
                    }
                }
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
                    }
                }
            } catch {}

            EditorUtility.ClearProgressBar();
        }

        // [MenuItem("Game/Build/Debug")]
        [ContextMenu("Build/Debug")]
        public virtual void BuildDebug() => Build(this.BuildDebugPath, true);

        // [MenuItem("Game/Build/Release")]
        [ContextMenu("Build/Release")]
        public virtual void BuildRelease() => Build(this.BuildReleasePath, true);

        // [MenuItem("Game/Mark Assets as Addressables and Localize")]
        [ContextMenu("Mark Assets as Addressables and Localize")]
        public virtual void MarkAssetsAsAddressablesAndLocalize() {
            MarkAssetsAsAddressables();
            LocalizeAddressableAssets();
        }

        public static void Build(string path, bool development=false) {
            AddressableAssetSettings.BuildPlayerContent();

            var now = DateTime.Now;
            PlayerSettings.bundleVersion = $"{now.Year:0000}.{now.Month:00}.{now.Day:00}";
            string name = PlayerSettings.productName, version = PlayerSettings.bundleVersion;
            string folder = path;

            var buildPlayerOptions = new BuildPlayerOptions() {
                scenes = EditorBuildSettings.scenes.Select((s) => s.path).ToArray(),
                locationPathName = $"{folder}/{name.ToLower()}-v{version}.apk",
                target = BuildTarget.Android,
            };
            buildPlayerOptions.options |= BuildOptions.AutoRunPlayer;
            if(development) buildPlayerOptions.options |= BuildOptions.Development;

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded) {
                Debug.Log(@$"Build succeeded! OutputPath: '{summary.outputPath}', OutputSize: {summary.totalSize} bytes.");
            } else if (summary.result == BuildResult.Failed) {
                Debug.Log($"Build failed!");
            }
        }
    }
}