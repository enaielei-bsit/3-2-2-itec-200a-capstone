/*
 * Date Created: Wednesday, November 24, 2021 6:33 AM
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
    using QuestionRunner;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceLocations;

    public class Database : Core.Controller {
        public delegate void LoadOnPreAndPost(IResourceLocation[] locations);
        public delegate void LoadOnProgress<T>(
            IResourceLocation[] locations,
            IResourceLocation location, AsyncOperationHandle<T> handler);

        protected readonly Dictionary<string, object> _assets =
            new Dictionary<string, object>();
        protected IResourceLocation[] _currentLocations;
        protected IResourceLocation _currentLocation;
        protected AsyncOperationHandle _currentHandle;

        public virtual IResourceLocation[] currentLocations
            => _currentLocations;
        public virtual IResourceLocation currentLocation
            => _currentLocation;
        public virtual AsyncOperationHandle currentHandle
            => _currentHandle;
        public virtual float loadingProgress {
            get {
                if(_currentLocations == null || _currentLocations.Length == 0.0f
                    || _currentLocation == null)
                    return 0.0f;
                return Array.IndexOf(currentLocations, currentLocation)
                    / (float) (_currentLocations.Length - 1);
            }
        }
        public virtual float assetLoadingProgress {
            get {
                try {return currentHandle.PercentComplete;}
                catch {} 
                return 0.0f;
            }
        }
        public virtual bool loading {
            get => loadingProgress != 1.0f && currentLocations != null
                && currentLocations.Length > 0;
        }

        public virtual IEnumerator LoadEnumerator<T>(
            object key,
            LoadOnPreAndPost onStart=default,
            LoadOnProgress<T> onProgress=default,
            LoadOnPreAndPost onFinish=default,
            bool ignoreExisting=true) {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(
                key, typeof(T));
            yield return locationsHandle;
            // while(!locationsHandle.IsDone) yield return new WaitForEndOfFrame();
            
            var locations = (from location in locationsHandle.Result
                where !ignoreExisting || !Has(location.PrimaryKey)
                select location).ToArray();
            // var locations = locationsHandle.Result.ToArray();
            _currentLocations = locations;

            onStart?.Invoke(locations);
            foreach(var location in locations) {
                var handle = Addressables.LoadAssetAsync<T>(location);
                _currentLocation = location;
                _currentHandle = handle;
                onProgress?.Invoke(locations, location, handle);
                yield return handle;
                // while(!handle.IsDone) yield return new WaitForEndOfFrame();

                if(Has(location.PrimaryKey)) {
                    Addressables.Release(_assets[location.PrimaryKey]);
                }
                _assets[location.PrimaryKey] = handle.Result;
            }
            _currentLocations = default;
            _currentLocation = default;
            _currentHandle = default;
            onFinish?.Invoke(locations);
            
            Addressables.Release(locationsHandle);
        }

        public virtual IEnumerator LoadEnumerator<T>(
            LoadOnPreAndPost onStart=default,
            LoadOnProgress<T> onProgress=default,
            LoadOnPreAndPost onFinish=default,
            bool ignoreExisting=true
        ) {
            return LoadEnumerator(
                typeof(T).Name, onStart, onProgress, onFinish, ignoreExisting
            );
        }

        public virtual Coroutine Load<T>(
            object key,
            LoadOnPreAndPost onStart=default,
            LoadOnProgress<T> onProgress=default,
            LoadOnPreAndPost onFinish=default,
            bool ignoreExisting=true) {
            return StartCoroutine(LoadEnumerator(
                key, onStart, onProgress, onFinish, ignoreExisting
            ));
        }

        public virtual Coroutine Load<T>(
            LoadOnPreAndPost onStart=default,
            LoadOnProgress<T> onProgress=default,
            LoadOnPreAndPost onFinish=default,
            bool ignoreExisting=true) {
            return Load(
                typeof(T).Name, onStart, onProgress, onFinish, ignoreExisting);
        }

        public virtual bool Has(string key) => Get(key, out object asset);

        public virtual bool Get<T>(string key, out T asset) {
            asset = default;
            if(_assets.ContainsKey(key)) asset = (T) _assets[key];
            return asset != null;
        }

        public virtual T Get<T>(string key) {
            Get(key, out T asset);
            return asset;
        }

        public override void Awake() {
            base.Awake();
        }
    }
}