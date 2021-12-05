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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    using Widgets;

    public class QRUserInterface : Core.Controller {
        public Player player;
        public Transform skillsRoot;
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
            Build();
        }

        [ContextMenu("Build")]
        public virtual void Build() {
            BuildSkills();
        }

        public virtual void BuildSkills() {
            if(!skillsRoot || !player) return;
            if(Application.isPlaying) skillsRoot.DestroyChildren();
            else skillsRoot.DestroyChildrenImmediately();
            foreach(var skill in player.caster.skills) {
                var skillWidget = Instantiate(_skill, skillsRoot.transform);
                skillWidget.Set(player.caster, player.buffable, skill);
            }
        }
    }
}