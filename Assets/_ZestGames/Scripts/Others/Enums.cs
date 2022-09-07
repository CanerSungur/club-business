namespace ZestGames
{
    public class Enums
    {
        public enum GameState { WaitingToStart, Started, PlatrofmEnded, GameEnded }
        public enum GameEnd { None, Success, Fail }
        public enum PoolStamp { Something, MoneyCollect2D, MoneySpend2D, Customer }
        public enum AudioType { Testing_PlayerMove, Button_Click, UpgradeMenu }
        public enum AiStateType { Idle, Wander, GetIntoClub }
        public enum AiLocation { Outside, Inside }
        public enum AiGender { Man, Woman }
    }
}
