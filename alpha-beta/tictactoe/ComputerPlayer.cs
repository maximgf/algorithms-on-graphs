namespace tictactoe
{
    public class ComputerPlayer
    {
        private char player;

        public ComputerPlayer(char player)
        {
            this.player = player;
        }

        public char GetPlayer()
        {
            return player;
        }

        public (int, int) GetMove(Board board, int prevX, int prevY)
        {
            return board.Hint(player,4,prevX,prevY);
        }
    }
}