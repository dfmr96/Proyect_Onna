using UnityEngine;

namespace Player
{
    public static class PlayerHelper
    {
        private static GameObject _playerGO;

        public static void SetPlayer(GameObject player)
        {
            _playerGO = player;
        }

        public static GameObject GetPlayer()
        {
            return _playerGO;
        }
        
        public static void ClearPlayer()
        {
            _playerGO = null;
        }
    }
}