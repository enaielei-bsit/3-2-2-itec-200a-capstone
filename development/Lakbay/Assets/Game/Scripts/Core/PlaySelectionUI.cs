/*
 * Date Created: Friday, December 10, 2021 2:50 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core
{
    public class PlaySelectionUI : Controller
    {
        public Player player;
        public MessageBoxUI message;
        public ToggleGroup group;

        [Space]
        public LocalizedString useCheckpoint;

        [Space]
        public LocalizedString qrDetailed;

        [Space]
        public LocalizedString qrLevel1DetailedNonPro;
        public LocalizedString qrLevel1DetailedPro;

        [Space]
        public LocalizedString qrLevel2DetailedNonPro;
        public LocalizedString qrLevel2DetailedPro;

        [Space]
        public LocalizedString qrLevel3DetailedNonPro;
        public LocalizedString qrLevel3DetailedPro;

        [Space]
        public LocalizedString saDetailed;
        
        [Space]
        public LocalizedString saBlowbagetsDetailed;
        public LocalizedString saParallelParkingDetailed;
        public LocalizedString saPerpendicularParkingDetailed;
        public LocalizedString saBackInAngleParkingDetailed;
        public LocalizedString saThreePointTurnDetailed;
        public LocalizedString saTailgatingDetailed;
        public LocalizedString saRightOfWayDetailed;
        public LocalizedString saTrafficSignalRulesDetailed;

        public new virtual IEnumerator Start()
        {
            base.Start();
            yield return new WaitUntil(() => Initialization.finished);
            group?.GetFirstActiveToggle().onValueChanged.Invoke(true);
        }

        public virtual void Play()
        {
            if (Session.checkpointController.IsCheckpointValid())
            {
                message?.ShowConfirmation(
                    useCheckpoint,
                    onYes: () => Session.checkpointController.LoadCheckpoint(
                        player
                    ),
                    onNo: () => player?.Play()
                );
            }
            else
            {
                player?.Play();
            }
        }

        public virtual void ShowMessage(LocalizedString str) {
            message?.ShowMessage(str);
        }

        public virtual void ShowLevel1Detailed(int mode) {
            GameMode mode_ = (GameMode) mode;
            if(mode_ == GameMode.NonPro) ShowMessage(qrLevel1DetailedNonPro);
            else ShowMessage(qrLevel1DetailedPro);
        }

        public virtual void ShowLevel2Detailed(int mode) {
            GameMode mode_ = (GameMode) mode;
            if(mode_ == GameMode.NonPro) ShowMessage(qrLevel2DetailedNonPro);
            else ShowMessage(qrLevel2DetailedPro);
        }

        public virtual void ShowLevel3Detailed(int mode) {
            GameMode mode_ = (GameMode) mode;
            if(mode_ == GameMode.NonPro) ShowMessage(qrLevel3DetailedNonPro);
            else ShowMessage(qrLevel3DetailedPro);
        }

        public virtual void ShowBlowbagetsDetailed() {
            ShowMessage(saBlowbagetsDetailed);
        }

        public virtual void ShowParallelParkingDetailed() {
            ShowMessage(saParallelParkingDetailed);
        }

        public virtual void ShowPerpendicularParkingDetailed() {
            ShowMessage(saPerpendicularParkingDetailed);
        }

        public virtual void ShowBackInAngleParkingDetailed() {
            ShowMessage(saBackInAngleParkingDetailed);
        }

        public virtual void ShowThreePointTurnDetailed() {
            ShowMessage(saThreePointTurnDetailed);

        }

        public virtual void ShowTailgatingDetailed() {
            ShowMessage(saTailgatingDetailed);
        }

        public virtual void ShowRightOfWayDetailed() {
            ShowMessage(saRightOfWayDetailed);
        }

        public virtual void ShowTrafficSignalRulesDetailed() {
            ShowMessage(saTrafficSignalRulesDetailed);
        }

        public virtual void ShowQRDetailed() {
            ShowMessage(qrDetailed);
        }

        public virtual void ShowSADetailed() {
            ShowMessage(saDetailed);
        }
    }
}