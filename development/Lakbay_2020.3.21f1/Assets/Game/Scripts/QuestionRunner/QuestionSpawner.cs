/*
 * Date Created: Thursday, November 4, 2021 5:53 PM
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

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    using Spawns;

    [CreateAssetMenu(
        fileName="QuestionSpawner",
        menuName="Game/QuestionRunner/QuestionSpawner"
    )]
    public class QuestionSpawner : MatrixCellHandler {
        public readonly List<QuestionSpawn> spawns = new List<QuestionSpawn>();
        public bool shuffledQuestions = true;
        [SerializeField]
        protected TextAsset _file;
        public QuestionSpawn spawn;
        public List<Question> questions = new List<Question>();
        protected readonly List<Question> _questions = new List<Question>();

        public override void OnBuild(
            Matrix matrix, GameObject cell, Vector2Int index, float chance) {
            if(this.spawn && spawn.OnSpawn(matrix, cell, index, chance)) {
                if(_questions.Count != 0) {
                    var spawn = Instantiate(this.spawn, cell.transform);
                    spawn.question = _questions.PopRandomly();
                    spawns.Add(spawn);
                }
            }
        }

        public override void OnPreBuild(Matrix matrix) {
            spawns.Clear();
            _questions.Clear();
            if(shuffledQuestions) _questions.AddRange(questions.Shuffle());
            else _questions.AddRange(questions);
        }

        [ContextMenu("Load from File")]
        protected virtual void LoadFromFile() {
            if(_file) {
                questions.Clear();
                questions.AddRange(
                    _file.text.DeserializeAsYaml<List<Question>>()
                );
            }
        }
    }
}