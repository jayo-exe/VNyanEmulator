using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VNyanInterface;

namespace VNyanEmulator
{
    public class VNyanAvatarHarness : MonoBehaviour
    {
        public GameObject AvatarObject;
        private GameObject lastAvatarObject;

        private void OnEnable()
        {
            if (AvatarObject == null && transform.childCount > 0)
            {
                AvatarObject = transform.GetChild(0).gameObject;
            }

            VNyanTestAvatar.SetAvatar(AvatarObject);
        }
        private void OnDisable()
        {
            VNyanTestAvatar.SetAvatar(null);
        }

        private void Update()
        {
            if (AvatarObject == lastAvatarObject) return;
            VNyanTestAvatar.SetAvatar(null);
            VNyanTestAvatar.SetAvatar(AvatarObject);
            lastAvatarObject = AvatarObject;
        }
    }
}
