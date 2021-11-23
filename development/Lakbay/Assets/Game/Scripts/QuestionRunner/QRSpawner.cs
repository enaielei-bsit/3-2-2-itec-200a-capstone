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

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    public class QRSpawner : Core.Spawner {
        [SerializeField]
        protected List<float> _spawnChances = new List<float>();
        public virtual float[] spawnChances => GetChances(
            spawns.Count, _spawnChances.ToArray());

        public override void Build(params Core.Spawn[] spawns) {
            base.Build(spawns);
        }

        public override void Spawn(
            Transform[] locations, Transform location,
            Core.Spawn[] spawns, Core.Spawn spawn) {
            var chance = UnityEngine.Random.value;
            if(chance >= spawnChances[Array.IndexOf(spawns, spawn)]) return;
            base.Spawn(locations, location, spawns, spawn);
        }

        public static float[] GetChances(int count, params float[] chances) {
            var lchances = chances.ToList();
            while(lchances.Count < count) lchances.Add(1.0f);
            return lchances.ToArray();
        }
    }
}