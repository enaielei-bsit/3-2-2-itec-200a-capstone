/*
 * Date Created: Thursday, October 7, 2021 9:43 PM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
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

namespace Ph.CoDe_A.Lakbay.Core {
    public class Game : Entity {
        public static readonly List<Initializer> initializers =
            new List<Initializer>(); 
        protected static int _initialSceneIndex = 1;
        protected static bool _initialized = false;
        public static bool initialized => _initialized;
        public static bool canInitialize =>
            SceneManager.GetActiveScene().buildIndex == 0;
        public static Repository repository;
        public static Repository repo => repository;
        public static LoadingScreen loadingScreen;

        public virtual IEnumerator LoadSceneEnumerator(
            int index=0,
            LoadSceneMode mode=LoadSceneMode.Single,
            bool activated=true,
            Action<AsyncOperation> onStart=default,
            Action<AsyncOperation> onProgress=default,
            Action<AsyncOperation> onFinish=default
        ) {
            var operation = SceneManager.LoadSceneAsync(index, mode);
            operation.allowSceneActivation = activated;
            
            onStart?.Invoke(operation);
            while(!operation.isDone) {
                onProgress?.Invoke(operation);
                yield return new WaitForEndOfFrame();
            }

            onFinish?.Invoke(operation);
        }

        public virtual void LoadScene(
            int index=0,
            LoadSceneMode mode=LoadSceneMode.Single,
            bool activated=true,
            Action<AsyncOperation> onStart=default,
            Action<AsyncOperation> onProgress=default,
            Action<AsyncOperation> onFinish=default
        ) {
            StartCoroutine(LoadSceneEnumerator(
                index, mode, activated, onStart, onProgress, onFinish));
        }

        public virtual void Initialize() {
        }

        public override void Awake() {
            base.Awake();
            if(!initialized) {
                int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
                if(canInitialize) {
                    printLog("Initializing...");
                    DontDestroyOnLoad(gameObject);
                    Initialize();
                } else {
                    printLog("Moving to the Initialization Scene...");
                    _initialSceneIndex = sceneBuildIndex;
                    LoadScene();
                }
            }
        }

        public override void Update() {
            base.Update();
            if(!initialized && canInitialize) {
                bool allSet = true;
                if(initializers.Count != 0) {
                    allSet = initializers.All((i) => i.initialized);
                }

                if(allSet) {
                    _initialized = allSet;
                    printLog("All set! Moving to the Initial Scene...");
                    LoadScene(
                        _initialSceneIndex,
                        onProgress: (o) => {
                            loadingScreen?.Show(o.progress, "Loading Scene...");
                        },
                        onFinish: (o) => loadingScreen?.Hide()
                    );
                }
            }
        }
    }
}