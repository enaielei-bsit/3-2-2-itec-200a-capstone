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

    [RequireComponent(typeof(Collider))]
    public class QuestionSpawn : QRSpawn {
        public float progressUpdateDuration = 0.5f;
        public Question question;
        public bool triggered = false;
        public bool handled = false;

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
            if(question != null) {
                Handle(trigger);
            }
            gameObject.SetActive(false);
        }

        public virtual void Handle(QuestionUI ui, QRPlayer player) {
            if(ui && !ui.gameObject.activeSelf) {
                handled = true;
                // Take note of the last count for playerStop
                Session.qrLevel.lastStop = player.repeaterHandler.repeated;

                player?.Pause();
                ui.Show(question, (qw, c) => {
                    player.qrInGameUI.SetProgress(
                        Session.qrLevel.progress,
                        progressUpdateDuration
                    );
                    qw.Hide();
                    player?.Resume();
                });

                // ui.Run(); // Done in the Show method instead...
            }
        }

        public virtual void Handle(QRPlayer player) {
            Handle(player.questionUI, player);
        }

        public override bool OnSpawnCheck(
            Spawner spawner, Transform[] locations, Transform location) {
            if(base.OnSpawnCheck(spawner, locations, location)) {
                return Session.qrLevel.free != null;
            }

            return false;
        }

        public override void OnSpawn(Spawner spawner) {
            base.OnSpawn(spawner);
            var question = Session.qrLevel.free;
            if(question != null) {
                var questions = Session.qrLevel.questions;
                Session.qrLevel.spawned.Add(questions.IndexOf(question));
                this.question = question;
            }
        }

        public override void OnDestroy() {
            base.OnDestroy();
            if(!handled) {
                if(!Session.qrLevel || !Session.qrLevel.questions.Contains(question)) return;
                Session.qrLevel.spawned.Remove(
                    Session.qrLevel.questions.IndexOf(question)
                );
            }
        }
    }
}