/*
 * Date Created: Thursday, November 4, 2021 6:22 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.QuestionRunner.Widgets
{
    using Core;

    [RequireComponent(typeof(Button))]
    public class ChoiceWidget : Controller
    {
        protected string _oldValue;
        protected string _value => choice.text;

        public bool automatic = true;
        public bool readOnly = false;
        public virtual Button button => GetComponentInChildren<Button>();
        public Image background;
        public TextMeshProUGUI text;
        public Color backgroundIdle;
        public Color backgroundCorrect;
        public Color textIdle;
        public Color textChosen;
        public Choice choice;
        protected Question _question;

        public override void Update()
        {
            base.Update();
            if (automatic)
            {
                if (_oldValue != _value)
                {
                    _oldValue = _value;
                    Build(choice.text);
                }
            }

            if (background && choice != null)
            {
                background.color = backgroundIdle;
                if (readOnly && choice.correct)
                {
                    background.color = backgroundCorrect;
                }
            }

            if (_question != null)
            {
                int index = _question.choices.IndexOf(choice);
                bool answered = _question.answers.Contains(index);
                if (text)
                {
                    if (answered) text.color = textChosen;
                    else text.color = textIdle;
                }
            }
        }

        public virtual void Build(
            QuestionUI questionInterface, Choice choice, bool readOnly = false)
        {
            if (choice == null) return;
            _question = questionInterface.question;
            this.choice = choice;
            this.readOnly = readOnly;
            Build(choice.text);

            if (button && !readOnly)
            {
                button.onClick.RemoveAllListeners();
                if (Application.isPlaying) button.onClick.AddListener(
                     () => questionInterface.Answer(choice)
                 );
            }

            if (readOnly) button.interactable = false;
        }

        public virtual void Build(string text)
        {
            if (this.text) this.text.SetText(text);
        }
    }
}