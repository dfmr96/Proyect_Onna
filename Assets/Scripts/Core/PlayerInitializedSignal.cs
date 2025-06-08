using Player;

namespace Core
{
    public class PlayerInitializedSignal
    {
        public PlayerModel Model;

        public PlayerInitializedSignal(PlayerModel model)
        {
            Model = model;
        }
    }
}