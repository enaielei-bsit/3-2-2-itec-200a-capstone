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

        public virtual void LoadScene(BuiltScene scene) {
            Session.sceneController?.Load(scene);
            Session.loadingScreen?.Monitor(Session.sceneController);
        }

        public virtual void LoadScene(int scene) => LoadScene((BuiltScene) scene);
        
        public virtual void LoadScene() => LoadScene(
            SceneController.current.buildIndex);

        public virtual void SetMode(GameMode mode) {
            Session.mode = mode;
            Debug.Log($"Mode selected: {Session.mode}");
        }

        public virtual void SetMode(int mode) => SetMode((GameMode) mode);

        public virtual void SetMode(bool mode) => 
            SetMode(mode ? 1 : 0);

        public virtual void Play(GameMode mode) {
            SetMode(mode);
            LoadScene(BuiltScene.QuestionRunner);
        }

        public virtual void Play(int mode) => Play((GameMode) mode);

        public virtual void Resume() {
            
        }

        public virtual void Pause() {

        }

        public virtual void Tweak() {

        }

        public virtual void Restart() {
            LoadScene();
        }

        public virtual void End() {
            LoadScene(BuiltScene.MainMenu);
        }
    }
}