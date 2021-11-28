/*
 * Date Created: Tuesday, November 23, 2021 6:42 AM
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

namespace Ph.CoDe_A.Lakbay.Core {
    public class Spawner : Controller {
        public bool randomizedLocation = true;
        public bool randomizedSpawn = true;
        [Min(0)]
        public int maxSpawnPerLocation = 1;
        public List<Transform> locations = new List<Transform>();
        public List<Spawn> spawns = new List<Spawn>();
        public List<float> chances = new List<float>();

        public override void Awake() {
            base.Awake();
        }

        public virtual void Build() => Build(spawns.ToArray());

        public virtual void Build(params Spawn[] spawns) {
            var locations = this.locations.ToArray();
            var originalLocations = locations;
            if(randomizedLocation) locations = locations.Shuffle().ToArray();

            var originalSpawns = spawns;
            if(randomizedSpawn) spawns = spawns.Shuffle().ToArray();

            foreach(var location in locations) {
                if(Application.isPlaying) location.DestroyChildren();
                else location.DestroyChildrenImmediately();

                foreach(var spawn in spawns) {
                    Spawn(originalLocations, location, originalSpawns, spawn);
                }
            }
        }

        public virtual Spawn Spawn(
            Transform[] locations, Transform location, Spawn[] spawns, Spawn spawn) {
            float chance = UnityEngine.Random.value;
            float spawnChance = chances.Count >= spawns.Length
                ? chances[Array.IndexOf(spawns, spawn)] : 1.0f;
            if(chance > spawnChance) return default;

            var currentSpawns = location.GetComponentsInChildren<Spawn>();
            if(currentSpawns.Length >= maxSpawnPerLocation) return default;
            if(spawn.OnSpawn(this, locations, location)) {
                return Instantiate(spawn, location);
            }
            return default;
        }
    }
}