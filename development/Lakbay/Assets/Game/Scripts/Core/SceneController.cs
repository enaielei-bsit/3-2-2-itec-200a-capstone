/*
 * Date Created: Saturday, December 4, 2021 6:00 PM
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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Utilities;

namespace Ph.CoDe_A.Lakbay.Core {
    public enum BuiltScene {
        None = -1,
        MainMenu,
        QuestionRunner,
        Blowbagets,
        ParallelParking,
        PerpendicularParking,
        BackInAngleParking,
        ThreePointTurn,
        ContentTester
    }

    public class SceneController : Controller, LoadingScreen.IMonitored {
        public static Scene current => SceneManager.GetActiveScene();

        protected string _currentLoadPath;
        public virtual string currentLoadPath => _currentLoadPath ?? "";
        public virtual string currentLoadName => GetSceneName(currentLoadPath);
        protected AsyncOperation _operation;
        public virtual AsyncOperation operation => _operation;
        protected Coroutine _coroutine;

        public virtual void Load(BuiltScene scene) => Load((int) scene);

        public virtual void Load(int sceneBuildIndex) =>
            Load(sceneBuildIndex, null);

        public virtual void Load(
            int sceneBuildIndex,
            Action<AsyncOperation> onStart=null,
            Action<AsyncOperation> onProgress=null,
            Action<AsyncOperation> onFinish=null,
            bool allowSceneActivation=true) {
            if(_coroutine == null) {
                _coroutine = StartCoroutine(
                    LoadEnumerator(
                        sceneBuildIndex,
                        (o) => {
                            _currentLoadPath =
                                SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex);
                            _operation = o;
                            onStart?.Invoke(o);
                        },
                        onProgress,
                        (o) => {
                            _currentLoadPath = default;
                            _operation = null;
                            _coroutine = null;
                            onFinish?.Invoke(o);
                        }
                    ));
            }
        }

        public virtual void Load(
            string scenePath,
            Action<AsyncOperation> onStart=null,
            Action<AsyncOperation> onProgress=null,
            Action<AsyncOperation> onFinish=null,
            bool allowSceneActivation=true) {
            Load(
                SceneUtility.GetBuildIndexByScenePath(scenePath),
                onStart, onProgress, onFinish, allowSceneActivation);
        }

        public static IEnumerator LoadEnumerator(
            int sceneBuildIndex,
            Action<AsyncOperation> onStart=null,
            Action<AsyncOperation> onProgress=null,
            Action<AsyncOperation> onFinish=null,
            bool allowSceneActivation=true) {
            bool valid = sceneBuildIndex.Within(
                0, SceneManager.sceneCountInBuildSettings - 1);
            if(!valid) {
                onStart?.Invoke(null);
                onFinish?.Invoke(null);
                yield break;
            }

            var path = SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex);
            var name = GetSceneName(path);

            var operation = SceneManager.LoadSceneAsync(path);
            operation.allowSceneActivation = allowSceneActivation;
            onStart?.Invoke(operation);
            while(!operation.isDone) {
                onProgress?.Invoke(operation);
                yield return null;
            }
            onFinish?.Invoke(operation);
        }

        public static IEnumerator LoadEnumerator(
            string scenePath,
            Action<AsyncOperation> onStart=null,
            Action<AsyncOperation> onProgress=null,
            Action<AsyncOperation> onFinish=null,
            bool allowSceneActivation=true) {
            return LoadEnumerator(
                SceneUtility.GetBuildIndexByScenePath(scenePath),
                onStart, onProgress, onFinish, allowSceneActivation);
        }

        public LoadingScreen.MonitorInfo OnMonitor(LoadingScreen loadingScreen) {
            if(_operation != null) {
                return new LoadingScreen.MonitorInfo(
                    currentLoadName,
                    _operation.progress
                );
            }

            return default;
        }

        public static string GetSceneName(string path) {
            return path.Split('/').Last().TrimEnd(".unity");
        }

        public static bool IsCurrent(BuiltScene scene) {
            return current.buildIndex == (int) scene;
        }

        public static BuiltScene GetCurrent() => (BuiltScene) current.buildIndex;
    }
}