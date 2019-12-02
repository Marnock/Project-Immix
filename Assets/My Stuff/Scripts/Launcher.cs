using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class Launcher: MonoBehaviourPunCallbacks
    {
        #region  Private Serializable Fields

        ///<summary>
        ///The maximum number of players per room
        ///</summary>
        [Tooltip("The maximum number of players per room. If a room is full, a new one will be created.")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion


        #region  Private Fields

        ///<summary>
        ///This client's version number.!-- Users are separated from each other by gameVersion
        ///</summary>
        string gameVersion = "1";


        ///<summary>
        ///Keep track of the current process. Connection is asynchronous and uses several callbacks from Photon
        ///Meaning that we must keep track of this so we can adjust the behaviour when we recieve a callback by Photon.
        ///This is typically used for OnConnectedToMaster() callback.
        ///</summary>
        bool isConnecting;

        #endregion

        



        #region  Monobehaviour Callbacks

        ///<summary>
        ///Monobehaviour method called on gameObject by unity during early initialization phase
        ///</summary>

        void Awake()
        {
            //#Critical
            //this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all cleints in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }


        ///<summary>
        ///Monobehaviour method called on gameObject by unity during early initialization phase
        ///</summary>
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }


        #endregion


        #region  Public Methods


        ///<summary>
        ///Start the connection process
        ///- If already connected, attempt joining a random room
        ///- If not connected, connect this application instance to photon cloud network
        ///</summary>

        public void Connect()
        {
            isConnecting = true;
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            //check if we are connected or notm join if we are, else initiate the connection to the server
            if(PhotonNetwork.IsConnected)
            {
                //#Critical we need at this point to attempt to join a random room. If it fails, notification will come in OnJoinRandomFailed() and a room will be created
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                //#Critical connect to photon online server
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion



        #region MonobehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");

            if(isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
            }
            
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() was called by PUN. No random room is available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            //#Critical we failed to join a room, so we create a new room
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() was called by PUN. This client is now in a room");

            //#Critical Only load if we are the first player, else rely on PhotonNetwork.AutomaticallySyncScene to sync the instance scene
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Load the 'Room for 1' ");

                //#Critical Load the Room level
                PhotonNetwork.LoadLevel("Room for 1");
            }
        }

        #endregion
    }
}