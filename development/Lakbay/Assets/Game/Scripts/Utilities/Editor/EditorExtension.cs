/*
 * Date Created: Saturday, November 27, 2021 10:21 PM
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
using UnityEngine.Localization.Settings;

namespace Utilities {
    public static class EditorExtension {
        public static AddressableAssetGroup EnsureGroup(
            this AddressableAssetSettings settings, string name) {
            var group = settings.FindGroup(name);
            if(!group)
                group = settings.CreateGroup(
                    name, false, false, true, null,
                    new Type[] {typeof(ContentUpdateGroupSchema)}
                );

            return group;
        }

        public static string EnsureLabel(
            this AddressableAssetSettings settings, string name) {
            if(!settings.GetLabels().Contains(name))
                settings.AddLabel(name);
            return name;
        }

        public static AddressableAssetEntry AddEntry(
            this AddressableAssetSettings settings,
            string path, string group, params string[] labels) {
            var grp = settings.DefaultGroup;
            if(group != null && group.Length > 0)
                grp = settings.EnsureGroup(group);
            var entry = settings.CreateOrMoveEntry(
                AssetDatabase.AssetPathToGUID(path), grp);

            if(labels != null && labels.Length > 0) {
                foreach(string label in labels) {
                    settings.EnsureLabel(label);
                    if(!string.IsNullOrEmpty(label)) entry.labels.Add(label);
                }
            }

            return entry;
        }

        public static AddressableAssetEntry AddEntry(
            this AddressableAssetSettings settings,
            string path) {
            return settings.AddEntry(path, null);
        }

        public static void Localize(
            this UnityEngine.Object asset, string table, string path) {
            var loc = asset.name.GetLocale(EditorHelper.locales, out string newName);
            string code = loc ? code = loc.Identifier.Code : null;
            if(string.IsNullOrEmpty(code))
                // return;
                code = LocalizationSettings.ProjectLocale.Identifier.Code;

            path = path == null || path.Length == 0 ? "Assets" : path;

            var atc = EditorHelper.EnsureAssetTableCollection(table, path);
            string assetPath = AssetDatabase.GetAssetPath(asset);
            var paths = assetPath.Split('/').ToList();
            paths.Pop(paths.Count - 1);

            string name = newName.Trim();

            paths.Add(name);
            string key = paths.Join("/");
            
            atc.AddAssetToTable(
                new LocaleIdentifier(code.ToLower()),
                key,
                asset);
        }
        
        public static void Localize(
            this UnityEngine.Object asset, string table) {
            asset.Localize(table, null);
        }

        public static void Localize(
            this UnityEngine.Object asset) {
            asset.Localize(asset.GetType().Name);
        }

        // Source: https://forum.unity.com/threads/how-to-clear-all-the-entries-from-an-assettablecollection-using-the-api.1180174/#post-7556974
        public static void Clear(this AssetTableCollection collection) {
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
            EditorUtility.SetDirty((UnityEngine.Object)collection.SharedData);
        }
    }
}