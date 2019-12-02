﻿using UnityEngine;
using UnityEngine.UI;


using System.Collections;


namespace Com.MyCompany.MyGame
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields


        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text playerNameText;


        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField]
        private Slider playerHealthSlider;

        private PlayerManager target;

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 targetPosition;


        #endregion

        #region Public Fields

        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        #endregion

        #region MonoBehaviour Callbacks

        void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            _canvasGroup = this.GetComponent<CanvasGroup>();
        }

        void Update()
        {
            //Reflect the Player's health
            if(playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health;
            }

            //Destroy itself if the target is null
            if(target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }


        void LateUpdate()
        {
            if(targetRenderer != null)
            {
                this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            //#Critical Follow the GameObject on screen
            if(targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }

        #endregion


        #region Public Methods

        public void SetTarget(PlayerManager _target)
        {
            if(_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            //Cache references for efficiency
            target = _target;
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
            CharacterController characterController = _target.GetComponent<CharacterController>();
            //Get data from the player that wont change during the lifetime of this component
            if(characterController != null)
            {
                characterControllerHeight = characterController.height;
            }
            if(playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
        }


        #endregion


    }
}