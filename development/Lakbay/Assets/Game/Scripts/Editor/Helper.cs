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
    public static class Helper {
        public const string GameAssetsPath = "Assets/Game";
        public const string LocalizationAssetsPath = GameAssetsPath + "/Localization";
        public const string BuildPath = "Build";
        public static readonly Dictionary<string, Tuple<string, string[]>> AssetMappings
            = new Dictionary<string, Tuple<string, string[]>>() {
                {"TextAsset", new Tuple<string, string[]>(
                    "Document",
                    new string[] {".cs", ".dll"})},
                {"Sprite", new Tuple<string, string[]>(
                    "Image",
                    new string[] {})},
                {"AudioClip", new Tuple<string, string[]>(
                    "Audio",
                    new string[] {})}
            };
        public static readonly Type[] AssetTypes = new Type[] {
            typeof(TextAsset),
            // typeof(Texture),
            typeof(Sprite),
            typeof(AudioClip),
        };

        public static List<string> GetAssetPaths(
            Type type, params string[] excludedExtensions) {
            var assetType = type;
            var gassets = AssetDatabase.FindAssets(
                $"t:{assetType.Name}", new string[] {GameAssetsPath});
            var assetPaths = gassets.Select((g) => AssetDatabase.GUIDToAssetPath(g))
                .Where((p) => excludedExtensions.Select((e) =>
                !p.EndsWith(e)).All()).ToList();

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
            foreach(var type in AssetTypes)
                _MarkAssetsAsAddressables(type,
                    AssetMappings.ContainsKey(type.Name)
                    ? AssetMappings[type.Name].Item2
                    : new string[] {});
        }

        private static void _MarkAssetsAsAddressables(Type type,
            params string[] excludedExtensions) {
            var assetType = type;
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if(!settings) return;
            var assetPaths = GetAssetPaths(assetType, excludedExtensions);
            
            string groupName = AssetMappings.ContainsKey(assetType.Name)
                ? AssetMappings[assetType.Name].Item1 : assetType.Name;
            var group = settings.FindGroup(groupName);
            if(!group)
                group = settings.CreateGroup(
                    groupName, false, false, true, null,
                    new Type[] {typeof(ContentUpdateGroupSchema)}
                );

            var entries = new List<AddressableAssetEntry>();
            foreach(var path in assetPaths) {
                var asset = AssetDatabase.LoadAssetAtPath(path, assetType);

                EditorUtility.DisplayProgressBar(
                    "Mark Assets as Addressables",
                    $"Marking Assets as Addressables ({assetType.Name})...",
                    (assetPaths.IndexOf(path) + 1) / assetPaths.Count
                );
                var entry = settings.CreateOrMoveEntry(
                    AssetDatabase.AssetPathToGUID(path), group);
                entries.Add(entry);

                // string label = AssetMappings.ContainsKey(assetType.Name)
                //     ? AssetMappings[assetType.Name].Item1 : assetType.Name;
                string label = assetType.Name;
                if(!settings.GetLabels().Contains(label))
                    settings.AddLabel(label);

                entry.labels.Add(label);
            }

            NormalizeAddressableAddresses();
            EditorUtility.ClearProgressBar();
        }

        private static void _MarkAssetsAsAddressables<T>(params string[] excludedExtensions)
            where T : UnityEngine.Object {
            _MarkAssetsAsAddressables(typeof(T), excludedExtensions);
        }

        [MenuItem("Game/Localization/Localize Assets")]
        public static void LocalizeAssets() {
            foreach(var type in AssetTypes)
                _LocalizeAssets(type,
                    AssetMappings.ContainsKey(type.Name)
                    ? AssetMappings[type.Name].Item2
                    : new string[] {});
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

            var now = DateTime.Now;
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

        private static void _LocalizeAssets(Type type,
            params string[] excludedExtensions) {
            var assetType = type;
            
            string tableName = AssetMappings.ContainsKey(assetType.Name)
                ? AssetMappings[assetType.Name].Item1 : assetType.Name;

            var atc = LocalizationEditorSettings.GetAssetTableCollection(tableName);
            if(!atc)
                atc = LocalizationEditorSettings.CreateAssetTableCollection(
                    tableName, LocalizationAssetsPath + "/Tables"
                );
            // else Clear(atc);
            var assetPaths = GetAssetPaths(type, excludedExtensions);

            var locales = LocalizationEditorSettings.GetLocales().ToArray();
            var codes = locales.ToCodes();

            foreach(var path in assetPaths) {
                var asset = AssetDatabase.LoadAssetAtPath(path, assetType);
                var paths = path.Split('/').ToList();
                paths.Pop(paths.Count - 1);

                string code = asset.name.Humanize().Split(' ').Last();
                // Debug.Log(codes.Join(", "));
                if(Array.Find(codes, (c) => c.ToLower() == code.ToLower()) == null) {
                    continue;
                }
                string name = asset.name.TrimEnd(code);

                if(asset) {
                    EditorUtility.DisplayProgressBar(
                        "Localize Assets",
                        $"Localizing Assets ({assetType.Name})...",
                        (assetPaths.IndexOf(path) + 1) / assetPaths.Count
                    );

                    paths.Add(name);
                    string key = paths.Join("/");
                    
                    // Debug.Log(key + " " + code.ToLower());
                    atc.AddAssetToTable(
                        new LocaleIdentifier(code.ToLower()),
                        key,
                        asset);
                }
            }

            NormalizeAddressableAddresses();
            EditorUtility.ClearProgressBar();
        }

        private static void _LocalizeAssets<T>(params string[] excludedExtensions)
            where T : UnityEngine.Object {
            _LocalizeAssets(typeof(T), excludedExtensions);
        }

        // Source: https://forum.unity.com/threads/how-to-clear-all-the-entries-from-an-assettablecollection-using-the-api.1180174/#post-7556974
        public static void Clear(AssetTableCollection assetTableCollection) {
            var collection = assetTableCollection;
            var tables = collection.AssetTables.ToArray();
            foreach(var table in tables) {
                var tableEntries = table.Values.ToArray();
                foreach(var tableEntry in tableEntries) {
                    // Remove the asset from Addressables
                    if (!string.IsNullOrEmpty(tableEntry.Guid)) {
                        collection.RemoveAssetFromTable(table, tableEntry.KeyId);
                    }
                }
        
                // Clear all entries
                table.Clear();
                EditorUtility.SetDirty(table);
            }
        
            // Clear all keys
            collection.SharedData.Entries.Clear();
            EditorUtility.SetDirty(collection.SharedData);
        }
        
        [MenuItem("Game/Address and Localize Assets")]
        public static void AddressAndLocalizeAssets() {
            MarkAssetsAsAddressables();
            LocalizeAssets();
        }
    }
}