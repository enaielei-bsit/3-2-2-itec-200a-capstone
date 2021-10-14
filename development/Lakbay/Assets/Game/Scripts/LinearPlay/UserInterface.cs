/*
 * Date Created: Thursday, October 14, 2021 11:33 AM
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

using Utilities;

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    using Widgets;

    public class UserInterface : Core.Widget {
        public Player player;
        public GameObject skillsRoot;
        [SerializeField]
        protected SkillWidget _skill;
        public virtual SkillWidget[] skills =>
            skillsRoot.GetComponentsInChildren<SkillWidget>();

        public override void Awake() {
            base.Awake();
            if(!player) player = FindObjectOfType<Player>();
        }

        public override void Start() {
            base.Start();
            Populate();
        }

        [ContextMenu("Populate")]
        public virtual void Populate() {
            PopulateSkills();
        }

        public virtual void PopulateSkills() {
            if(!skillsRoot || !player) return;
            if(Application.isPlaying) skillsRoot.DestroyChildren();
            else skillsRoot.DestroyChildrenImmediately();
            foreach(var skill in player.skills) {
                var skillWidget = Instantiate(_skill, skillsRoot.transform);
                skillWidget.Set(player.buffable, skill);
            }
        }
    }
}