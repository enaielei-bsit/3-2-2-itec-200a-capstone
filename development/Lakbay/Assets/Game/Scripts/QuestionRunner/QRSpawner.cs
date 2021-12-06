/*
 * Date Created: Tuesday, November 23, 2021 7:33 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ph.CoDe_A.Lakbay.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    public class QRSpawner : Core.Spawner {
        public Repeater repeater;
        [SerializeField]
        protected List<string> _steps = new List<string>();
        [SerializeField]
        protected List<string> _intervals = new List<string>();
        public int[][] steps => _steps.Select((s) => s.Trim())
            .Select((s) => s.Split(" ").Select((n) => Mathf.Max(int.Parse(n), 0))
                .ToArray()).ToArray();
        public int[][] intervals => _intervals.Select((s) => s.Trim())
            .Select((s) => s.Split(" ").Select((n) => Mathf.Max(int.Parse(n), 1))
                .ToArray()).ToArray();

        public override bool CanSpawn(
            Transform[] locations, Transform location,
            Spawn[] spawns, Spawn spawn) {
            bool can = base.CanSpawn(locations, location, spawns, spawn);
            if(!can) return false;

            if(repeater && repeater.handler) {
                int index = Array.IndexOf(spawns, spawn);

                var steps = this.steps;
                if(index.Within(0, steps.Length - 1)) {
                    if(!steps[index].Contains(repeater.handler.repeated))
                        return false;
                }

                var intervals = this.intervals;
                if(index.Within(0, intervals.Length - 1)) {
                    if(intervals[index].All((i) => (repeater.handler.repeated.Mod(i)) != 0))
                        return false;
                }
            }

            var questionSpawn = spawn as Spawns.QuestionSpawn;
            if(questionSpawn) {
                if(Session.spawnedQuestionIndices.Count
                    == Session.qrLevel.questions.Count) {
                    return false;
                }
            }

            var stop = spawn as Spawns.PlayerStopSpawn;
            if(stop) {
                bool existing = Array.Find(locations,
                    (l) => l.GetComponentInChildren<Spawns.PlayerStopSpawn>()
                );
                int current = repeater.handler.repeated;
                int offset = 5;
                if(existing || !Session.qrLevel.questions.All((q) => q.answered)
                    || current != (Session.stopStart + offset)) {
                    printLog("triggered1");
                    return false;
                }
            }

            if(Session.stopStart != -1) {
                if(spawn as SkillSpawn || spawn as Spawns.ObstacleSpawn) {
                    printLog("triggered2");
                    return false;
                }
            }

            return true;
        }

        public override void OnSpawnInstantiate(Spawn spawn) {
            base.OnSpawnInstantiate(spawn);
            var questionSpawn = spawn as Spawns.QuestionSpawn;
            if(questionSpawn) {
                var spawned = Session.spawnedQuestions;
                var question = Session.qrLevel.questions.Shuffle()
                    .FirstOrDefault((q) => !spawned.Contains(q));
                Session.spawnedQuestionIndices.Add(
                    Session.qrLevel.questions.IndexOf(question));
                questionSpawn.question = question;
            }
        }
    }
}