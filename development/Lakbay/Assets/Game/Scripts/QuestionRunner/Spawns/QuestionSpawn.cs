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

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Spawns {
    public class QuestionSpawn : QRSpawn {
        public Question question;
        public bool triggered = false;
        protected readonly List<float> _timeScales = new List<float>();

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var player = collider.GetComponentInParent<Player>();
            
            if(player && !player.GetComponentInParent<Buff>()) {
                if(!triggered) {
                    triggered = true;
                    OnTrigger(player);
                }
            }
        }

        public virtual void OnTrigger(Player trigger) {
            if(question != null) Handle(trigger);
            gameObject.SetActive(false);
        }

        public virtual void Handle(Widgets.QuestionWidget widget, Player player) {
            if(widget) {
                widget.gameObject.SetActive(true);
                widget.Build(question);
                Tween.LocalScale(
                    widget.transform, Vector3.zero, Vector3.one,
                    0.25f, 0.0f
                );
                
                _Save(player);
                _Pause(player);
                widget.onAnswer?.RemoveAllListeners();
                widget.onAnswer?.AddListener((qw, c) => {
                    Tween.LocalScale(
                        qw.transform, Vector3.zero,
                        0.25f, 0.0f
                    );
                    _Restore(player);
                });

                widget.Run();
            }
        }

        public virtual void Handle(Player player) {
            Handle(FindObjectOfType<Widgets.QuestionWidget>(true), player);
        }

        protected virtual void _Save(Player player) {
            _timeScales.Clear();
            _timeScales.AddRange(new float[] {
                player.buffable.timeScale,
                player.travel.timeScale,
                player.slide.timeScale
            });
        }

        protected virtual void _Pause(Player player) {
            player.buffable.timeScale = 0.0f;
            player.travel.timeScale = 0.0f;
            player.slide.timeScale = 0.0f;
        }

        protected virtual void _Restore(Player player) {
            try {
                player.buffable.timeScale = _timeScales.Pop();
                player.travel.timeScale = _timeScales.Pop();
                player.slide.timeScale = _timeScales.Pop();
            } catch {
                player.buffable.timeScale = 1.0f;
                player.travel.timeScale = 1.0f;
                player.slide.timeScale = 1.0f;
            }
        }
    }
}