using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lots_o__level_types
{
    internal class LightControllerLever : MonoBehaviour, IButtonReceiver
    {
        [SerializeField]
        public RoomController Room;




        public void ButtonPressed(bool val)
        {
            Room.SetPower(val);
        }

        public void ConnectButton(GameButtonBase button)
        {
            button.Set(false);
            Room.SetPower(false);
        }

            }
}
