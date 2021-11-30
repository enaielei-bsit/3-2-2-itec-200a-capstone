/*
 * Date Created: Wednesday, November 24, 2021 6:32 AM
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
    using Core;

    [CreateAssetMenu(
        fileName="QRLevel",
        menuName="Game/Question Runner/Level"
    )]
    public class QRLevel : ScriptableObject {
        public GameMode category = GameMode.NonPro;
        public TimeOfDay time;

        public RepeaterHandler repeaterHandler;

        [SerializeField]
        [ContextMenuItem("Load", "LoadQuestionsFile")]
        protected TextAsset _questionsFile;
        public List<Question> questions = new List<Question>();

        public virtual void LoadQuestionsFile() {
            if(_questionsFile) {
                this.questions.Clear();
                var questions = _questionsFile.text.DeserializeAsYaml<Question[]>();
                this.questions.AddRange(questions);
            }
        }
    }
}