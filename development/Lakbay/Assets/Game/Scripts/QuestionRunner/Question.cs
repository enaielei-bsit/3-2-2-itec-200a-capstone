/*
 * Date Created: Tuesday, November 2, 2021 4:09 PM
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

using YamlDotNet.Serialization;

using Utilities;

namespace Ph.CoDe_A.Lakbay.QuestionRunner {
    using CommandLine;
    using Core;
    using YamlDotNet.Serialization;

    [Serializable]
    public class Choice {
        public bool correct;
        public Content content = new Content();

        public Choice() {}

        public Choice(Content content, bool correct=false) {
            this.content = content;
            this.correct = correct;
        }
    }

    [Serializable]
    public class Question {
        public Content content = new Content();
        public List<Choice> choices = new List<Choice>();
        protected List<Choice> _answers = new List<Choice>();
        [YamlIgnore]
        public virtual Choice[] answers => _answers.ToArray();
        [YamlIgnore]
        public virtual bool correct {
            get {
                if(_answers.Count == 0) return false;
                return _answers.All((a) => choices.Contains(a) && a.correct);
            }
        }
        [YamlIgnore]
        public virtual Choice[] solution =>
            choices.Where((c) => c.correct).ToArray();

        public Question() {}

        public Question(Content content, params Choice[] choices) {
            this.content = content;
            this.choices.AddRange(choices);
        }

        public virtual bool Check(int index, ref Choice choice) {
            if(choices.Count == 0) return false;
            choice = choices.Find(
                (c) => choices.IndexOf(c) == index && c.correct);
            return choice != null;
        }

        public virtual bool Check(int index) {
            Choice choice = default;
            return Check(index, ref choice);
        }

        public virtual void Answer(int index) => Answer(new int[] {index});

        public virtual void Answer(IEnumerable<int> indices) {
            Answer(indices.Where((i) => i.Within(0, choices.Count - 1))
                .Select((i) => choices[i]).ToArray());
        }

        public virtual void Answer(Choice choice) =>
            Answer(new Choice[] {choice});

        public virtual void Answer(IEnumerable<Choice> choices) {
            _answers.Clear();
            AddAnswer(choices);
        }

        public virtual void AddAnswer(Choice choice) => AddAnswer(
            new Choice[] {choice}
        );
        
        public virtual void AddAnswer(int index) =>
            AddAnswer(new int[] {index});
        
        public virtual void AddAnswer(IEnumerable<int> indices) {
            AddAnswer(indices.Where((i) => i.Within(0, choices.Count - 1))
                .Select((i) => choices[i]).ToArray());
        }

        public virtual void AddAnswer(IEnumerable<Choice> choices) {
            foreach(var choice in choices) {
                if(choices.Contains(choice)) _answers.Add(choice);
            }
        }
        
        public virtual void RemoveAnswer(int index) =>
            RemoveAnswer(new int[] {index});
        
        public virtual void RemoveAnswer(IEnumerable<int> indices) {
            RemoveAnswer(indices.Where((i) => i.Within(0, choices.Count - 1))
                .Select((i) => choices[i]).ToArray());
        }

        public virtual void RemoveAnswer(Choice choice) => RemoveAnswer(
            new Choice[] {choice}
        );
        
        public virtual void RemoveAnswer(IEnumerable<Choice> choices) {
            foreach(var choice in choices) {
                while(_answers.Contains(choice)) _answers.Remove(choice);
            }
        }
    }
}