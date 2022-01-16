/*
 * Date Created: Monday, December 13, 2021 12:33 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.QuestionRunner
{
    using TMPro;
    using UnityEngine.Localization.Components;
    using Utilities;

    public class QRPostPlayUI : Core.PostPlayUI
    {
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

        [Header("Summary")]
        public Button summary;
        public Button overallSummary;
        public LocalizeStringEvent message;

        protected int _score = 0;
        public virtual int score => _score;

        protected int _maxScore = 0;
        public virtual int maxScore => _maxScore;

        public virtual void Show()
        {
            if (!gameObject.activeSelf)
            {
                summary?.gameObject.SetActive(false);
                overallSummary?.gameObject.SetActive(false);
                BuildSummary();
                gameObject.SetActive(true);
            }
        }

        public virtual void Hide()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void Build(params QRLevel[] levels)
        {
            if (ratings && _filledRating && _emptyRating)
            {
                if (Application.isPlaying) ratings.transform.DestroyChildren();
                else ratings.transform.DestroyChildrenImmediately();

                _score = levels.Sum((l) => l.score);
                _maxScore = levels.Sum((l) => l.maxScore);
                foreach (var l in levels) printLog(
                     l.score, l.maxScore, l.spawned.Count, l.questions.Count);
                float part = maxScore / (float)maxRating;
                for (int i = 0; i < maxRating; i++)
                {
                    if (score != 0 && score < (part * (i + 1)))
                    {
                        Instantiate(_emptyRating, ratings);
                    }
                    else Instantiate(_filledRating, ratings);
                }
            }

            if (reviews && _correctReview && player)
            {
                if (Application.isPlaying) reviews.transform.DestroyChildren();
                else reviews.transform.DestroyChildrenImmediately();

                var levels_ = levels.Select(
                    (l) => l.spawned.Select((s) => l.questions[s]));

                int count = 0;
                foreach (var level in levels_)
                {
                    foreach (var question in level)
                    {
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

            bool passed = levels.All((l) => l.passed);
            passedRemark?.gameObject.SetActive(passed);
            failedRemark?.gameObject.SetActive(!passed);

            proceed?.gameObject.SetActive(passed);
            retry?.gameObject.SetActive(!passed);
        }

        public virtual void BuildSummary()
        {
            Build(Session.qrLevel);
            summary?.gameObject.SetActive(
                Session.qrLevelIndex == Session.qrLevels.Count - 1);
            overallSummary?.gameObject.SetActive(false);
            message?.RefreshString();
        }

        public virtual void BuildOverallSummary()
        {
            Build(Session.qrLevels.ToArray());
            summary?.gameObject.SetActive(false);
            overallSummary?.gameObject.SetActive(true);
            message?.RefreshString();
        }

        public virtual void Restart()
        {
            player?.Restart(
                Session.qrLevelIndex != Session.qrLevels.Count() - 1);
        }
    }
}