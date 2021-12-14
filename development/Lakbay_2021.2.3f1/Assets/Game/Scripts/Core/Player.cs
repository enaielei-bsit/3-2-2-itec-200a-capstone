/*
 * Date Created: Friday, December 10, 2021 2:21 PM
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

namespace Ph.CoDe_A.Lakbay.Core {
    public class Player : Controller {
        public new virtual IEnumerator Start() {
            yield return new WaitUntil(() => Initialization.finished);
            Build();
        }

        public virtual void Build() {

        }

        public static void LoadScene(BuiltScene scene) {
            Session.sceneController?.Load(scene);
            Session.loadingScreen?.Monitor(Session.sceneController);
        }

        public static void LoadScene(int scene) => LoadScene((BuiltScene) scene);
        
        public static void LoadScene() => LoadScene(
            SceneController.current.buildIndex);

        public static void SetMode(GameMode mode) {
            Session.mode = mode;
            Debug.Log($"Mode selected: {Session.mode}");
        }

        public static void SetMode(int mode) => SetMode((GameMode) mode);

        public static void SetMode(bool mode) => 
            SetMode(mode ? 1 : 0);

        public static void Play(GameMode mode) {
            SetMode(mode);
            LoadScene(BuiltScene.QuestionRunner);
        }

        public static void Play(int mode) => Play((GameMode) mode);
    }
}