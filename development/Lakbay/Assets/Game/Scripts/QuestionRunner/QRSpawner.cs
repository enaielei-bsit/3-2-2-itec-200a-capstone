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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    public class QRSpawner : Core.Spawner {
        [SerializeField]
        protected List<string> _steps = new List<string>();
        public int[][] steps => _steps.Select((s) => s.Trim())
            .Select((s) => s.Split(" ").Select((n) => int.Parse(n))
                .ToArray()).ToArray();

        public override Spawn Spawn(
            Transform[] locations, Transform location,
            Spawn[] spawns, Spawn spawn) {
            var newSpawn = base.Spawn(locations, location, spawns, spawn);
            if(!newSpawn) return default;
            var questionSpawn = spawn as Spawns.QuestionSpawn;
            if(questionSpawn) {
                if(Session.qrLevel.questions.Count == 0) return default;
                var question = Session.qrLevel.questions.Find((q) => !q.answered);
                if(question == null) return default;
                questionSpawn.question = question;
            }

            return spawn;
        }
    }
}