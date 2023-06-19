// // ----------------------------------------------------------------------------
// // <copyright file="BasicMain.cs" company="Kinetix SAS">
// // Kinetix Unity SDK - Copyright (C) 2022 Kinetix SAS
// // </copyright>
// // ----------------------------------------------------------------------------

using UnityEngine;
using Kinetix;
using Kinetix.UI;
using Kinetix.UI.EmoteWheel;

namespace Kinetix.Sample
{
    public class KinetixInitScript : MonoBehaviour
    {
        //[SerializeField] private string _virtualWorldKey; // key is provided in singleton class KinetixRestApiClient
        [SerializeField] private Animator _localPlayerAnimator;
        public string _userId = "Ant0nin";

        private void Awake()
        {
            KinetixCore.OnInitialized += OnKinetixInitialized;
            KinetixCore.Initialize(new KinetixCoreConfiguration()
            {
                VirtualWorldKey = KinetixRestApiClient.Instance.apiKey,
                PlayAutomaticallyAnimationOnAnimators = true,
                ShowLogs                              = true,
                EnableAnalytics                       = true
            });
        }

        private void OnDestroy()
        {
            KinetixCore.OnInitialized -= OnKinetixInitialized;

            //KinetixCore.Account.DisconnectAccount();
        }

        private void OnKinetixInitialized()
        {
            KinetixUIEmoteWheel.Initialize(new KinetixUIEmoteWheelConfiguration()
            {
                enabledCategories = new []
                {
                    EKinetixUICategory.INVENTORY,
                    EKinetixUICategory.EMOTE_SELECTOR
                }
            });

            KinetixCore.Animation.RegisterLocalPlayerAnimator(_localPlayerAnimator);

            //KinetixCore.Account.ConnectAccount("sdk-sample-user-id", OnAccountConnected);
            KinetixCore.Account.ConnectAccount(_userId, OnAccountConnected);
        }

        private void OnAccountConnected()
        {
            KinetixCore.Account.AssociateEmotesToUser("d228a057-6409-4560-afd0-19c804b30b84");
            KinetixCore.Account.AssociateEmotesToUser("bd6749e5-ac29-46e4-aae2-bb1496d04cbb");
            KinetixCore.Account.AssociateEmotesToUser("7a6d483e-ebdc-4efd-badb-12a2e210e618");
        }
    }
}
