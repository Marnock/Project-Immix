using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        public static GameManager Instance;

        [Tooltip("The prefab to use for the player")]
        public GameObject playerPrefab;

        #endregion

        void Start()
        {
            Instance = this;

            if(playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                //Only instantiate if PlayerManager does not have a reference to an existing insance of localPlayer
                if(PlayerManager.LocalPlayerInstance == null)
                {
                      Debug.LogFormat("We are instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
                      //we're in a room so spawn a character for the local player which gets synced with PhotonNetwork.Instantiate
                     PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);  
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); //This is not visible if you are the player connecting

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); //called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); //This is visible when other disconnects

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); //called before OnPlayerLeftRoom

                LoadArena();
            }
        }


        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion


        #region Private Methods

        ///<summary>
        ///Method loads the appropraite room based on the PlayerCount property of the room 
        void LoadArena()
        {
            if(!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork: Trying to load a level but is not the master client");
            }
            Debug.LogFormat("PhotonNetwork: Loading Level: {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            //#Critical PhotonNetwork.LoadLevel() should only be called if you are the master cleint
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion


    }
}