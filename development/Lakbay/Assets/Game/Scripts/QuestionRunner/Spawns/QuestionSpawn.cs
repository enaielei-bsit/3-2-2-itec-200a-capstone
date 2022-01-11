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
    using UnityEngine.Localization;

    [RequireComponent(typeof(Collider))]
    public class QuestionSpawn : QRSpawn {
        public float progressUpdateDuration = 0.5f;
        public float delayBeforeResuming = 2f;
        public Question question;
        public bool triggered = false;
        public bool handled = false;
        public AudioSource _sound;
        [Space]
        public AudioSource correctSound;
        public AudioSource wrongSound;
        public LocalizedString correctMessage;
        public LocalizedString wrongMessage;

        [Space]
        public UnityEvent onTrigger = new UnityEvent();

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
                onTrigger?.Invoke();
                if(_sound) {
                    var sound = Instantiate(_sound, transform.parent);
                    sound.Play();
                }
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
                    bool correct = qw.question.correct;
                    float dismiss = delayBeforeResuming * 0.98f;
                    player?.notification?.ShowAutoDismiss(
                        correct ? correctMessage : wrongMessage,
                        autoDismiss: dismiss
                    );
                    AudioSource sound = null;
                    if(correct) {
                        sound = Instantiate(correctSound, player.transform);
                    } else sound = Instantiate(wrongSound, player.transform);
                    sound.Play();
                    Destroy(sound.gameObject, dismiss);

                    player?.Invoke("Resume", delayBeforeResuming);
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