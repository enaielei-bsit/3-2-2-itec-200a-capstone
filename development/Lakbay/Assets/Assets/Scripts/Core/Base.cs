/*
 * Date Created: Thursday, October 7, 2021 3:08 AM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Utilities;

namespace Ph.CoDe_A.Lakbay.Core {
    public abstract class Base : MonoBehaviour {
        public float timeScale = 1.0f;

        public virtual void Awake() {}

        public virtual void Start() {}

        public virtual void Update() {}

        public virtual void FixedUpdate() {}

        public virtual void LateUpdate() {}

        public virtual void OnGUI() {}

        public virtual void OnDisable() {}

        public virtual void OnEnable() {}

        public virtual void OnBecameInvisible() {}
        
        public virtual void OnBecameVisible() {}
        
        public virtual void OnCollisionEnter() {}
        
        public virtual void OnCollisionExit() {}
        
        public virtual void OnCollisionStart() {}

        public virtual void OnInspectorUpdate() {}

        public virtual void print(params object[] objs) {
            MonoBehaviour.print(objs.Join(", "));
        }

        public virtual void printLog(params object[] objs) {
            if(objs.Length != 0) {
                objs[0] = $"[{name}.{GetType().Name}]: " + objs[0].ToString();
                print(objs);
            }
        }
    }
}