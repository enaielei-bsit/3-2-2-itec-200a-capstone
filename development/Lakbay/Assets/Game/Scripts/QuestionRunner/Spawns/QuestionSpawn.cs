/*
 * Date Created: Friday, October 15, 2021 10:25 AM
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

using Pixelplacement;

using Utilities;
using Ph.CoDe_A.Lakbay.Core;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Spawns {
    using Core;

    public class QuestionSpawn : QRSpawn {
        public Question question;
        public bool triggered = false;
        public virtual Widgets.QuestionWidget questionWidget =>
            FindObjectOfType<Widgets.QuestionWidget>(true);

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var player = collider.GetComponentInParent<QRPlayer>();
            
            if(player && collider.GetTrigger<SpawnTrigger>()) { 
                if(!triggered) {
                    triggered = true;
                    OnTrigger(player);
                }
            }
        }

        public virtual void OnTrigger(QRPlayer trigger) {
            if(question != null) Handle(trigger);
            gameObject.SetActive(false);
        }

        public virtual void Handle(Widgets.QuestionWidget widget, QRPlayer player) {
            if(widget) {
                // widget.gameObject.SetActive(true);
                widget.Show();
                widget.Build(question);
                // Tween.LocalScale(
                //     widget.transform, Vector3.zero, Vector3.one,
                //     0.25f, 0.0f
                // );
                
                player?.Pause();
                widget.onAnswer?.RemoveAllListeners();
                widget.onAnswer?.AddListener((qw, c) => {
                    // Tween.LocalScale(
                    //     qw.transform, Vector3.zero,
                    //     0.25f, 0.0f
                    // );
                    qw.Hide();
                    player?.Resume();
                });

                widget.Run();
            }
        }

        public virtual void Handle(QRPlayer player) {
            Handle(questionWidget, player);
        }

        public override bool OnSpawn(
            Spawner spawner, Transform[] locations, Transform location) {
            return base.OnSpawn(spawner, locations, location);
        }
    }
}