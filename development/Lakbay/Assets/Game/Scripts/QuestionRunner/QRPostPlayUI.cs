/*
 * Date Created: Monday, December 13, 2021 12:33 PM
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
    using TMPro;
    using Utilities;

    public class QRPostPlayUI : Core.PostPlayUI {
        [Space]
        public QRPlayer player;

        [Header("Rating")]
        public int maxRating = 5;
        [SerializeField]
        protected Image _filledRating;
        [SerializeField]
        protected Image _emptyRating;
        public RectTransform ratings;

        [Header("Review")]
        [SerializeField]
        protected Button _correctReview;
        [SerializeField]
        protected Button _wrongReview;
        public RectTransform reviews;

        [Header("Remarks")]
        public RectTransform passedRemark;
        public RectTransform failedRemark;

        public virtual void Show() {
            if(!gameObject.activeSelf) {
                gameObject.SetActive(true);
                Build();
            }
        }

        public virtual void Hide() {
            if(gameObject.activeSelf) {
                gameObject.SetActive(false);
            }
        }

        public virtual void Build() {
            if(ratings && _filledRating && _emptyRating) {
                if(Application.isPlaying) ratings.transform.DestroyChildren();
                else ratings.transform.DestroyChildrenImmediately();

                int score = player ? player.score : 0;
                int maxScore = player ? player.maxScore : 1;
                float part = maxScore / (float) maxRating;
                for(int i = 0; i < maxRating; i++) {
                    if(score != 0 && score < (part * (i + 1))) {
                        Instantiate(_emptyRating, ratings);
                    } else Instantiate(_filledRating, ratings);
                }
            }

            if(reviews && _correctReview && player) {
                if(Application.isPlaying) reviews.transform.DestroyChildren();
                else reviews.transform.DestroyChildrenImmediately();

                var levels = Session.qrLevels.Select(
                    (l) => l.spawned.Select((s) => l.questions[s]));
                
                int count = 0;
                foreach(var level in levels) {
                    foreach(var question in level) {
                        count++;
                        var _review = question.correct
                            ? _correctReview : _wrongReview;
                        var review = Instantiate(_review, reviews);
                        review.onClick.AddListener(
                            () => player.questionUI?.Show(
                                question, readOnly: true
                        ));

                        var text = review.GetComponentInChildren<TextMeshProUGUI>();
                        text?.SetText(count.ToString("00"));
                    }
                }
            }

            passedRemark?.gameObject.SetActive(Session.qrPassed);
            failedRemark?.gameObject.SetActive(!Session.qrPassed);

            proceed?.gameObject.SetActive(Session.qrPassed);
            retry?.gameObject.SetActive(!Session.qrPassed);
        }
    }
}