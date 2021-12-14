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
    using Pixelplacement;
    using Pixelplacement.TweenSystem;
    using Widgets;

    public class QRInGameUI : Core.Controller {
        protected TweenBase _goalSet;

        public QRPlayer player;

        [Header("Skills")]
        public RectTransform skills;
        [SerializeField]
        protected SkillWidget _skill;

        [Header("Indicators")]
        public float lifeMinColorValueOffset = 0.10f;
        public RectTransform lives;
        [SerializeField]
        protected Image _life;
        public Slider progress;
        
        [Space()]
        public Button pause;

        public override void Awake() {
            base.Awake();
            if(!player) player = FindObjectOfType<QRPlayer>();
        }

        public override void Start() {
            base.Start();
            // Build();
        }

        [ContextMenu("Build")]
        public virtual void Build() {
            BuildSkills();
            BuildLives();
            SetProgress(Session.qrLevel.progress);
        }

        public virtual void BuildSkills() {
            if(!skills || !player) return;
            if(Application.isPlaying) skills.DestroyChildren();
            else skills.DestroyChildrenImmediately();
            foreach(var skill in player.caster.skills) {
                var skillWidget = Instantiate(_skill, skills.transform);
                skillWidget.Set(player.caster, player.buffable, skill);
            }
        }

        public virtual void BuildLives() {
            if(!lives || !player) return;
            if(Application.isPlaying) lives.DestroyChildren();
            else lives.DestroyChildrenImmediately();

            float flives = player.lives;
            foreach(int i in Enumerable.Range(0, player.maxLives)) {
                var life = Instantiate(_life, lives.transform);
                Color.RGBToHSV(life.color, out float h, out float s, out float v);
                float offset = lifeMinColorValueOffset;
                float min = v - offset;
                var color =
                    Color.HSVToRGB(h, s, (offset * (i / (flives - 1))) + min);
                life.color = color;
            }
        }

        public virtual void UpdateLives() {
            if(!lives || !player) return;
            foreach(var img in lives.GetComponentsInChildren<Image>().Enumerate()) {
                img.Value.enabled = img.Key <= player.lives;
            }
        }

        public virtual void SetProgress(float value, float duration=0.20f, float delay=0.0f) {
            if(progress) {
                value = Mathf.Clamp(value, 0.0f, 1.0f);
                if(_goalSet != null) _goalSet.Cancel();
                _goalSet = Tween.Value(
                    progress.normalizedValue, value,
                    (v) => progress.normalizedValue = v,
                    duration, delay);
            }
        }

        public override void Update() {
            base.Update();
            UpdateLives();
        }
    }
}