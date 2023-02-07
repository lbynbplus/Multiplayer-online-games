namespace Server.Domain
{
    public interface ICard
    {
        /// <summary>
        /// 获取坐标对应的卡片颜色
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public CardColor GetCardColor(int position);
    }

    public class Card : ICard
    {
        private List<CardItem> _cardColorPosition = new List<CardItem>()
        {
              new CardItem()
            {
                CardColor =  CardColor.Red,
                Position = 1,
            },
            new CardItem()
            {
              CardColor =  CardColor.Green,
                Position = 2,
            },
            new CardItem()
            {
               CardColor =  CardColor.Blue,
                Position = 3
            },
            new CardItem()
            {
                CardColor =  CardColor.Red,
                Position = 4
            },
            new CardItem()
            {
              CardColor =  CardColor.Green,
                Position = 5
            },
            new CardItem()
            {
               CardColor =  CardColor.Blue,
                Position = 6
            },
            new CardItem()
            {
                CardColor =  CardColor.Red,
                Position = 10,
            },
            new CardItem()
            {
              CardColor =  CardColor.Green,
                Position = 11
            },
            new CardItem()
            {
               CardColor =  CardColor.Blue,
                Position = 12
            },
            new CardItem()
            {
                CardColor =  CardColor.Red,
                Position = 13,
            },
            new CardItem()
            {
              CardColor =  CardColor.Green,
                Position = 14
            },
            new CardItem()
            {
               CardColor =  CardColor.Blue,
                Position = 15
            },
             new CardItem()
            {
                CardColor =  CardColor.Red,
                Position = 33
            },
            new CardItem()
            {
              CardColor =  CardColor.Green,
                Position = 45
            },
            new CardItem()
            {
               CardColor =  CardColor.Blue,
                Position = 56
            },
        };
        public CardColor GetCardColor(int position)
        {
            var item = _cardColorPosition.Where(c => c.Position == position).FirstOrDefault();
            if (item is null)
            {
                return CardColor.None;
            }
            return item.CardColor;
        }
    }

    public class CardItem
    {
        public CardColor CardColor { get; set; }
        public int Position { get; set; }
    }
}
