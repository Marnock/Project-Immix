using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;


namespace Com.MyCompany.MyGame
{
    ///<summary>
    ///Player name input field. User can input their name and appear above them in game
    ///</summary>
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants


        //Store the PlayerPref Key to avoid typos
        const string playerNamePrefKey = "PlayerName";


        #endregion


        #region Monobehaviour Callbacks

         ///<summary>
         ///Monobehaviour method called on GameObject by Unity during initialization phase
         ///</summary>

        void Start()
        {
            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();
            
            if(_inputField != null)
            {
                if(PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion
        


        #region Public Methods

        ///<summary>
        ///Sets the name of the player and saves it in the PlayerPrefs for future use
        ///</summary>
        ///<param name = "value"> The na me of the Player</param>

        public void SetPlayerName(string value)
        {
            //#Important
            if(string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;


            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

        #endregion

    }
    
}