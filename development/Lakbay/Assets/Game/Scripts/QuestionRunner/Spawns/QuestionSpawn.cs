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

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Spawns {
    public class QuestionSpawn : Spawn {
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

        public override bool OnSpawn(
            Matrix matrix, GameObject cell, Vector2Int index, float chance) {
            bool can = base.OnSpawn(matrix, cell, index, chance);
            if(can) {
                var spawns = cell.GetComponentsInChildren<Spawn>();
                if(Application.isPlaying) cell.DestroyChildren();
                else cell.DestroyChildrenImmediately();

                // if(can) {
                //     var sp = cell.transform.parent
                //         .GetComponentInChildren(GetType());
                //     can = sp == null;
                // }
            }

            return can;
        }

        public virtual void OnTrigger(Player player) {
            if(question != null) {
                var qw = FindObjectOfType<Widgets.QuestionWidget>(true);
                if(qw) {
                    qw.gameObject.SetActive(true);
                    qw.Build(question);
                    
                    _Save(player);
                    _Pause(player);
                    qw.onAnswer?.RemoveAllListeners();
                    qw.onAnswer?.AddListener((qw, c) => _Restore(player));

                    qw.Run();
                }
            }

            gameObject.SetActive(false);
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