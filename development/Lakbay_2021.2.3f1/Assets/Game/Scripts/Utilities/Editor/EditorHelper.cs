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

namespace Utilities {
    public static class EditorHelper {
        public static AddressableAssetSettings addressableAssetSettings
            => AddressableAssetSettingsDefaultObject.Settings;
        public static Locale[] locales => LocalizationEditorSettings.GetLocales().ToArray();
        
        public static IEnumerable<string> GetAssetPaths(
            string query, string[] folders, params string[] ignoredExtensions) {
            
            var gassets = new string[] {};
            if(folders != null && folders.Length != 0)
                gassets = AssetDatabase.FindAssets(
                query, folders);
            else gassets = AssetDatabase.FindAssets(query);
            
            var assetPaths = gassets.Select(
                (g) => AssetDatabase.GUIDToAssetPath(g));
            if(ignoredExtensions != null && ignoredExtensions.Length > 0)
                assetPaths = assetPaths.Where((p) => ignoredExtensions.Select((e) =>
                    !p.EndsWith("." + e)).All());;
            return assetPaths;
        }

        public static IEnumerable<string> GetAssetPaths(
            string query, params string[] folders) {
            return GetAssetPaths(query, folders, null);
        }

        public static IEnumerable<object> GetAssets(
            Type type, params string[] paths) {
            return paths.Select((p) => AssetDatabase.LoadAssetAtPath(p, type));
        }

        public static IEnumerable<T> GetAssets<T>(params string[] paths) {
            return GetAssets(typeof(T), paths).Select((a) => (T) a);
        }

        public static AssetTableCollection EnsureAssetTableCollection(
            string name, string path) {
            var atc = LocalizationEditorSettings.GetAssetTableCollection(name);
            if(!atc)
                atc = LocalizationEditorSettings.CreateAssetTableCollection(
                    name, path
                );
            return atc;
        }
    }
}