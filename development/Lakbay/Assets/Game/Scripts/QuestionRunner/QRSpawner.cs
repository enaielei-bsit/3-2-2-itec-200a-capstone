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
                    if(!intervals[index].Any((i) => repeater.handler.repeated % i == 0))
                        return false;
                }
            }

            var questionSpawn = spawn as Spawns.QuestionSpawn;
            if(questionSpawn) {
                if(Session.qrLevel.questions.Count == 0) return false;
                var question = Session.qrLevel.questions.Shuffle()
                    .FirstOrDefault((q) => !q.answered);
                if(question == null) return false;
            }

            return true;
        }

        public override void OnSpawnInstantiate(Spawn spawn) {
            base.OnSpawnInstantiate(spawn);
            var questionSpawn = spawn as Spawns.QuestionSpawn;
            if(questionSpawn) {
                var question = Session.qrLevel.questions.Shuffle()
                    .FirstOrDefault((q) => !q.answered);
                questionSpawn.question = question;
            }
        }
    }
}