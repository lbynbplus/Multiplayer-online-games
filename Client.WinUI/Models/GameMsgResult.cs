namespace Client.WinUI.Models
{
    public class GameMsgResult
    {
        public string GameName { get; set; } = string.Empty;
        public int PlayerCount { get; set; }
    }

    public class GameMatrix
    {
        public int DiceValue
        {
            get; set;
        }

        public int Position
        {
            get; set;
        }

        public bool IsWon
        {
            get; set;
        }

        public bool IsSnakeBite
        {
            get; set;
        }

        public bool IsLadder
        {
            get; set;
        }

        /// <summary>
        /// 卡片颜色
        /// </summary>
        public CardColor CardColor
        {
            get; set;
        }
    }

    public enum CardColor
    {
        None = 0,
        Green = 1,
        Blue = 2,
        Red = 3
    }
}
