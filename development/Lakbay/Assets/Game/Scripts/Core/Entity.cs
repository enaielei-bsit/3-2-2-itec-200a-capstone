/*
 * Date Created: Sunday, October 10, 2021 8:08 AM
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
    public abstract class Entity : MonoBehaviour {
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

        public virtual void OnCollisionEnter(Collision collision) {}

        public virtual void OnCollisionExit(Collision collision) {}

        public virtual void OnTriggerEnter(Collider collider) {}

        public virtual void OnTriggerExit(Collider collider) {}

        public virtual void OnValidate() {}

        public static void print(params object[] objs) {
            MonoBehaviour.print(string.Join(", ", objs));
        }

        public void printLog(params object[] objs) {
            if(objs.Length == 0) return;
            objs[0] = $"[{name}.{GetType().Name}]: "
                + (objs[0] != null ? objs[0].ToString() : "");
            print(objs);
        }
    }
}