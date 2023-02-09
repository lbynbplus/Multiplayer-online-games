namespace Server.Domain
{
    public class GameFactory : IGameFactory
    {
        ILadder _ladder;
        ISnake _snake;
        ICard _card;
        public GameFactory(ILadder ladder, ISnake snake, ICard card)
        {
            _ladder = ladder;
            _snake = snake;
            _card = card;
        }

        public int ApplyLadder(int postion)
        {
            return (_ladder.GetPosition(postion));
        }

        public CardColor GetCardColor(int postion)
        {
            return _card.GetCardColor(postion);
        }

        public int SnakeBite(int postion)
        {
            return (_snake.GetPosition(postion));
        }
    }
}
