/*
 * Date Created: Thursday, October 7, 2021 3:24 AM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

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
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

using Humanizer;

using Utilities;

namespace Ph.CoDe_A.Lakbay {
    
    public static class Helper {
        public const string GameAssetsPath = "Assets/Assets";
        public const string LocalizationAssetsPath = GameAssetsPath + "/Localization";
        public const string BuildPath = "Build";
        public static readonly Dictionary<string, string[]> ExcludedAssetExtensions = 
            new Dictionary<string, string[]>() {
                {"TextAsset", new string[] {
                    ".cs"
                }}
            };
        public static readonly System.Type[] AssetTypes = new System.Type[] {
            typeof(TextAsset),
            typeof(Texture),
            typeof(Sprite),
            typeof(AudioClip),
        };

        public static List<string> GetAssetPaths(System.Type type, params string[] excludedExtensions) {
            var assetType = type;
            var gassets = AssetDatabase.FindAssets(
                $"t:{assetType.Name}", new string[] {GameAssetsPath});
            var assetPaths = gassets.Select((g) => AssetDatabase.GUIDToAssetPath(g))
                .Where((p) => excludedExtensions.Select(
                    (e) => !p.EndsWith(e)).All()).ToList();

            return assetPaths;
        }

        public static List<string> GetAssetPaths<T>(params string[] excludedExtensions) {
            return GetAssetPaths(typeof(T), excludedExtensions);
        }

        [MenuItem("Game/Addressables/Normalize Addressable Addresses")]
        public static void NormalizeAddressableAddresses() {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            foreach(var group in settings.groups) {
                foreach(var entry in group.entries) {
                    entry.SetAddress(entry.AssetPath);
                }
            }
        }

        [MenuItem("Game/Addressables/Mark Assets as Addressables")]
        public static void MarkAssetsAsAddressables() {
            foreach(var type in AssetTypes) {
                _MarkAssetsAsAddressables(
                    type,
                    ExcludedAssetExtensions.ContainsKey(type.Name)
                    ? ExcludedAssetExtensions[type.Name]
                    : new string[] {}
                );
            }
        }

        private static void _MarkAssetsAsAddressables(
            System.Type type,
            params string[] excludedExtensions) {
            var assetType = type;
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if(!settings) return;
            var assetPaths = GetAssetPaths(type, excludedExtensions);

            var group = settings.FindGroup(assetType.Name);

            var entries = new List<AddressableAssetEntry>();
            foreach(var path in assetPaths) {
                var asset = AssetDatabase.LoadAssetAtPath(path, assetType);
                if(!group)
                    group = settings.CreateGroup(
                        assetType.Name, false, false, true, null,
                        new System.Type[] {typeof(ContentUpdateGroupSchema)}
                    );

                EditorUtility.DisplayProgressBar(
                    "Mark Assets as Addressables",
                    $"Marking Assets as Addressables ({assetType.Name})...",
                    (assetPaths.IndexOf(path) + 1) / assetPaths.Count
                );
                var entry = settings.CreateOrMoveEntry(
                    AssetDatabase.AssetPathToGUID(path), group);
                entries.Add(entry);

                if(!settings.GetLabels().Contains(assetType.Name))
                    settings.AddLabel(assetType.Name);

                entry.labels.Add(assetType.Name);
            }

            NormalizeAddressableAddresses();
            EditorUtility.ClearProgressBar();
        }

        private static void _MarkAssetsAsAddressables<T>(
            params string[] excludedExtensions)
            where T : UnityEngine.Object {
        }

        [MenuItem("Game/Localization/Localize Assets")]
        public static void LocalizeAssets() {
            foreach(var type in AssetTypes) {
                _LocalizeAssets(
                    type,
                    ExcludedAssetExtensions.ContainsKey(type.Name)
                    ? ExcludedAssetExtensions[type.Name]
                    : new string[] {}
                );
            }
        }

        [MenuItem("Game/Build/Release")]
        private static void BuildRelease() {
            _Build();
        }

        [MenuItem("Game/Build/Development")]
        private static void BuildDevelopment() {
            _Build(true);
        }

        private static void _Build(bool development=false) {
            AddressableAssetSettings.BuildPlayerContent();

            var now = System.DateTime.Now;
            PlayerSettings.bundleVersion = $"{now.Year:0000}.{now.Month:00}.{now.Day:00}";
            string name = PlayerSettings.productName, version = PlayerSettings.bundleVersion;
            string folder = BuildPath + "/" + (development ? "Development" : "Release");

            var buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select((s) => s.path).ToArray();
            foreach(var scene in buildPlayerOptions.scenes) Debug.Log(scene);
            buildPlayerOptions.locationPathName = $"{folder}/{name.ToLower()}-v{version}.apk";
            buildPlayerOptions.target = BuildTarget.Android;
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

        private static void _LocalizeAssets(System.Type type, params string[] excludedExtensions) {
            var localeCodes = LocalizationEditorSettings.GetLocales().ToCodes();
            var codes = localeCodes.Select(
                (c) => c.ToLower());
            var assetType = type;
            var atc = LocalizationEditorSettings.GetAssetTableCollection(assetType.Name);
            if(atc){
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(atc));
            }
            atc = LocalizationEditorSettings.CreateAssetTableCollection(
                assetType.Name, LocalizationAssetsPath + "/Tables"
            );
            var assetPaths = GetAssetPaths(type, excludedExtensions);

            foreach(var path in assetPaths) {
                var asset = AssetDatabase.LoadAssetAtPath(path, assetType);
                var paths = path.Split('/').ToList();
                paths.Pop(paths.Count - 1);

                string code = asset.name.Humanize().Split(" ").Last();
                string name = asset.name.TrimEnd(code);
                name = name.TrimEnd(' ', '-', '_');
                bool inCodes = codes.Contains(code.ToLower());

                if(asset && inCodes) {
                    EditorUtility.DisplayProgressBar(
                        "Localize Assets",
                        $"Localizing Assets ({assetType.Name})...",
                        (assetPaths.IndexOf(path) + 1) / assetPaths.Count
                    );

                    paths.Add(name);
                    string key = paths.Join("/");
                    atc.AddAssetToTable(
                        atc.GetTable(new LocaleIdentifier(code.ToLower())) as AssetTable,
                        key,
                        asset);
                }
            }

            NormalizeAddressableAddresses();
            EditorUtility.ClearProgressBar();
        }

        private static void _LocalizeAssets<T>(params string[] excludedExtensions) where T : UnityEngine.Object {
            _LocalizeAssets(typeof(T), excludedExtensions);
        }

        [MenuItem("Game/Address and Localize Assets")]
        public static void AddressAndLocalizeAssets() {
            MarkAssetsAsAddressables();
            LocalizeAssets();
        }
    }
}