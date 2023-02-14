using Server.Domain;

namespace Server.Services
{
    public interface IGameService
    {
        public GameMatrix RollDice(string userName);
        public void ResetGame();

        public string GetGameName();

        public void SetGameName(string gameName);

        public string GetGameRoomOwner();

        public void SetGameRoomOwner(string gameRoomOwner);

        public bool GetGameStatus();

        public void SetGameStatus(bool isGameOver);
    }

    public class GameMatrix
    {
        public int DiceValue { get; set; }

        public int Position { get; set; }

        public bool IsWon { get; set; }

        public bool IsSnakeBite { get; set; }

        public bool IsLadder { get; set; }

        /// <summary>
        /// 卡片颜色
        /// </summary>
        public CardColor CardColor { get; set; }
    }
}
