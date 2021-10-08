/*
 * Date Created: Thursday, October 7, 2021 9:51 PM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core {
    public class Repository : Initializer {
        protected OrderedDictionary _assets = new OrderedDictionary();

        public virtual object this[string key] {
            get => Get<object>(key);
        }

        public virtual IEnumerator LoadEnumerator<T0, T1>(
            T1 key,
            Action<IList<IResourceLocation>> onStart=default,
            Action<IList<IResourceLocation>, IResourceLocation> onProgress=default,
            Action<IList<IResourceLocation>> onFinish=default
        ) {
            if(_assets.Contains(key)) yield break;

            IList<IResourceLocation> locations = new List<IResourceLocation>();
            var resLocHandle = Addressables.LoadResourceLocationsAsync(key);
            resLocHandle.Completed += (h) => locations = h.Result;

            while(!resLocHandle.IsDone) yield return new WaitForEndOfFrame();
            Addressables.Release(resLocHandle);
            
            onStart?.Invoke(locations);
            foreach(var location in locations) {
                onProgress?.Invoke(locations, location);
                if(_assets.Contains(location.PrimaryKey)) continue;

                var handle = Addressables.LoadAssetAsync<T0>(location);
                while(!handle.IsDone) {
                    yield return new WaitForEndOfFrame();
                }

                _assets[location.PrimaryKey] = handle.Result;
                // Addressables.Release(handle);
            }
            onFinish?.Invoke(locations);
        }

        public virtual IEnumerator LoadEnumerator<T>(
            Action<IList<IResourceLocation>> onStart=default,
            Action<IList<IResourceLocation>, IResourceLocation> onProgress=default,
            Action<IList<IResourceLocation>> onFinish=default
        ) {
            return LoadEnumerator<T, string>(
                typeof(T).Name, onStart, onProgress, onFinish
            );
        }

        public virtual void Load<T0, T1>(
            T1 key,
            Action<IList<IResourceLocation>> onStart=default,
            Action<IList<IResourceLocation>, IResourceLocation> onProgress=default,
            Action<IList<IResourceLocation>> onFinish=default
        ) {
            StartCoroutine(
                LoadEnumerator<T0, T1>(key, onStart, onProgress, onFinish));
        }

        public virtual void Load<T>(
            Action<IList<IResourceLocation>> onStart=default,
            Action<IList<IResourceLocation>, IResourceLocation> onProgress=default,
            Action<IList<IResourceLocation>> onFinish=default
        ) {
            Load<T, string>(typeof(T).Name, onStart, onProgress, onFinish);
        }

        public virtual void Clear() {
            if(_assets.Count == 0) return;
            var keys = _assets.Keys.Cast<string>();
            foreach(var key in keys) {
                Remove(key);
            }
        }

        public virtual void Remove(string key) {
            if(!_assets.Contains(key)) return;
            Addressables.Release(_assets[key]);
            _assets.Remove(key);
        }

        public virtual T Get<T>(string key) {
            if(!_assets.Contains(key)) return default;
            print("found", _assets[key]);
            return (T) _assets[key];
        }

        public override void Initialize() {
            Load<TextAsset>(
                onStart: (l) => printLog($"Loading TextAsset(s)..."),
                onFinish: (l) => Load<Texture>(
                    onStart: (l) => printLog($"Loading Texture(s)..."),
                    onFinish: (l) => Load<Sprite>(
                        onStart: (l) => printLog($"Loading Sprite(s)..."),
                        onFinish: (l) => Load<AudioClip>(
                            onStart: (l) => printLog($"Loading AudioClip(s)..."),
                            onFinish: (l) => {
                                _initialized = true;
                            }
                        )
                    )
                )
            );
        }
    }
}