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
    using Content = List<Core.Entry>;

    [Serializable]
    public class Choice {
        public bool correct = false;
        public string text = "";

        public Choice() {}

        public Choice(string text, bool correct=false) {
            this.text = text;
            this.correct = correct;
        }

        public override string ToString() {
            return text.ToString();
        }
    }

    [Serializable]
    public class Question {
        protected float _elapsedTime = 0.0f;
        public virtual float elapsedTime {
            get => _elapsedTime;
            set => _elapsedTime = Mathf.Clamp(value, 0.0f, time);
        }
        [YamlIgnore]
        public virtual float time => Helper.GetExpectedReadTime(ToString())
            + 10.0f;
        public Content content = new Content();
        public List<Choice> choices = new List<Choice>();
        [NonSerialized]
        protected List<int> _answers = new List<int>();
        [YamlIgnore]
        public virtual int[] answers => _answers.ToArray();
        [YamlIgnore]
        public virtual bool correct {
            get {
                if(_answers.Count == 0) return false;
                return _answers.All(
                    (a) => a.Within(0, choices.Count - 1) && choices[a].correct);
            }
        }
        [YamlIgnore]
        public virtual Choice[] solution =>
            choices.Where((c) => c.correct).ToArray();
        [YamlIgnore]
        public virtual bool answered => _answers.Count != 0;

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
                if(choices.Contains(choice)) _answers.Add(
                    this.choices.IndexOf(choice));
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
                int index = this.choices.IndexOf(choice);
                while(_answers.Contains(index))
                    _answers.Remove(index);
            }
        }

        public virtual void ClearAnswers() => RemoveAnswer(answers);

        public override string ToString() {
            return new string[] {content.ToString(), choices.Join("\n")}
                .Join("\n");
        }
    }
}