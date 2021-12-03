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
    using UnityEngine.Localization;

    [CreateAssetMenu(
        fileName="QRLevel",
        menuName="Game/Question Runner/Level"
    )]
    public class QRLevel : Asset, ILocalizable {
        protected LocalizeTextAssetEvent _questionsFileEvent;

        public GameMode category = GameMode.NonPro;
        public TimeOfDay time;

        public RepeaterHandler repeaterHandler;

        [SerializeField]
        // [ContextMenuItem("Load", "LoadQuestionsFile")]
        // protected TextAsset _questionsFile;
        public LocalizedAsset<TextAsset> questionsFile;
        public List<Question> questions = new List<Question>();

        public virtual void LoadQuestions(bool update=true) =>
            LoadQuestions(questionsFile.LoadAsset(), update);

        public virtual void LoadQuestions(TextAsset asset) =>
            LoadQuestions(asset, true);

        public virtual void LoadQuestions(TextAsset asset, bool update) {
            var questions = asset.text.DeserializeAsYaml<List<Question>>();

            if(!update) {
                this.questions.Clear();
                this.questions.AddRange(questions);
            } else {
                if(this.questions.Count != questions.Count) {
                    LoadQuestions(asset, false);
                    return;
                } else {
                    foreach(var nquestion in questions.Enumerate()) {
                        var oquestion = this.questions[nquestion.Key];

                        oquestion.content = nquestion.Value.content;
                        foreach(var nchoice in nquestion.Value.choices.Enumerate()) {
                            oquestion.choices[nchoice.Key].text = nchoice.Value.text;
                        }
                    }
                }
            }
        }

        public virtual void Localize(Localizer localizer) {
            localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                questionsFile, LoadQuestions);
        }
    }
}