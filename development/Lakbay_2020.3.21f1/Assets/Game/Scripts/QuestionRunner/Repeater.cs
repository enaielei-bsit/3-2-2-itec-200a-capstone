/*
 * Date Created: Tuesday, October 19, 2021 7:58 AM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    public class Repeater : Core.Entity {
        public GameObject root;

        [SerializeField]
        protected List<Repeatable> _repeatables = new List<Repeatable>();
        public virtual Repeatable[] repeatables {
            get => GetComponentsInChildren<Repeatable>();
        }

        // [ContextMenu("Build")]
        public virtual void Build(int count) {}

        public override void Awake() {
            base.Awake();
            if(!root) root = gameObject;
        }
    }
}