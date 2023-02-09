using Server.Services;

namespace Server.Models
{
    public class Player
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int ID { get; set; }
        public int Position { get; set; }
        public int CardY { get; set; }
        public int CardG { get; set; }
        public int CardB { get; set; }
        public string ConnectionId { get; set; } = string.Empty;
        public bool IsOffLine { get; set; }
        public GameMatrix GameMatrix { get; set; } = new GameMatrix();
    }
}
