/*
 * Date Created: Sunday, November 28, 2021 2:42 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay {
    [CreateAssetMenu(fileName="HelperSettings", menuName="Game/Editor/Helper Settings")]
    public class HelperSettings : ScriptableObject {
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
    }
}