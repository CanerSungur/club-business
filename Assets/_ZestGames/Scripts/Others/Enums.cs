namespace ZestGames
{
    public class Enums
    {
        public enum GameState { WaitingToStart, Started, PlatrofmEnded, GameEnded }
        public enum GameEnd { None, Success, Fail }
        public enum PoolStamp { Something, MoneyCollect2D, MoneySpend2D, Customer, Beer }
        public enum AudioType { Testing_PlayerMove, Button_Click, UpgradeMenu, CollectMoney, SpendMoney }
        public enum AiStateType { Idle, Wander, GetIntoClub, Dance, BuyDrink, GoToToilet, GetIntoToiletQueue, Attack, Defend, Leaving }
        public enum AiLocation { Outside, Inside }
        public enum AiGender { Man, Woman }
        public enum AiMood { VeryHappy, Happy, Neutral, Frustrated, Angry, Furious }
        public enum QueueType { Gate, Bar, Toilet }
    }
}
