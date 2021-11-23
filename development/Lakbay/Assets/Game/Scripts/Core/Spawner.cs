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
        public Transform root;
        public List<Transform> locations = new List<Transform>();
        public List<Spawn> spawns = new List<Spawn>();

        public override void Awake() {
            base.Awake();
            if(!root) root = transform;
        }

        public virtual void Build() => Build(spawns.ToArray());

        public virtual void Build(params Spawn[] spawns) {
            if(!root) return;
            if(Application.isPlaying) root.DestroyChildren();
            else root.DestroyChildrenImmediately();

            var locations = this.locations.ToArray();
            var originalLocations = locations;
            if(randomizedLocation) locations = locations.Shuffle().ToArray();

            var originalSpawns = spawns;
            if(randomizedSpawn) spawns = spawns.Shuffle().ToArray();

            foreach(var location in locations) {
                if(Application.isPlaying) location.DestroyChildren();
                else location.DestroyChildrenImmediately();

                foreach(var spawn in spawns) {
                    Spawn(originalLocations, location, spawns, spawn);
                }
            }
        }

        public virtual void Spawn(
            Transform[] locations, Transform location, Spawn[] spawns, Spawn spawn) {
            var currentSpawns = location.GetComponentsInChildren<Spawn>();
            if(currentSpawns.Length >= maxSpawnPerLocation) return;
            if(spawn.OnSpawn(this, locations, location)) {
                Instantiate(spawn, location);
            }
        }
    }
}