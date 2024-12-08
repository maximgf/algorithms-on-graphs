namespace tictactoe
{
    public class Game
    {
        private Board board = new Board();
        private List<(int, int)> history = new List<(int, int)>();

        public void Play()
        {
            char player = 'X';
            ComputerPlayer computer = new ComputerPlayer('O');

            while (true)
            {
                if (board.IsFull())
                {
                    Console.WriteLine("DRAW");
                    SaveHistory();
                    break;
                }
                if (player == 'X')
                {
                    int x, y;
                    //var hint = board.Hint(player, 3);
                    //Console.WriteLine("Hint* X: " + hint.Item1 + " Y: " + hint.Item2);
                    Console.Write("Enter your move <x y>: ");
                    string[] input = Console.ReadLine().Split();
                    x = int.Parse(input[0]);
                    y = int.Parse(input[1]);

                    if (!board.IsValidMove(x, y))
                    {
                        Console.WriteLine("Invalid move. Try again.");
                        continue;
                    }
                    Console.Clear();
                    Console.WriteLine("Player step:\n");
                    board.MakeMove(x, y, player);
                    history.Add((x, y));
                    board.Display();
                    if (board.Is_win('X'))//board.Connectivity(x, y, player).Item1 == 5)
                    {
                        Console.WriteLine("You win!");
                        SaveHistory();
                        break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Computer step:\n");
                    var move = computer.GetMove(board, history.LastOrDefault().Item1, history.LastOrDefault().Item2);
                    board.MakeMove(move.Item1, move.Item2, player);
                    history.Add(move);
                    board.Display();
                    if (board.Is_win('O'))//board.Connectivity(move.Item1, move.Item2, player).Item1 == 5)
                    {
                        Console.WriteLine("Computer wins!");
                        SaveHistory();
                        break;
                    }
                }
                player = (player == 'X') ? 'O' : 'X';
            }
        }

        public void SaveHistory()
        {
            using (StreamWriter file = new StreamWriter("history.txt"))
            {
                file.WriteLine("GAME");
                for (int i = 0; i < history.Count; i++)
                {
                    string play = "O: ";
                    if (i % 2 == 0)
                    {
                        play = "X: ";
                    }
                    Console.WriteLine(play + history[i].Item1 + " " + history[i].Item2);
                    file.WriteLine(play + history[i].Item1 + " " + history[i].Item2);
                }
                Console.WriteLine("DATA is history.txt");
            }
        }

        public void GameDemonstration()
        {
            ComputerPlayer computer_X = new ComputerPlayer('X');
            ComputerPlayer computer_O = new ComputerPlayer('O');
            char player = 'X';
            (int, int) move;
            while (true)
            {
                if (board.IsFull())
                {
                    Console.WriteLine("DRAW");
                    SaveHistory();
                    break;
                }
                if (player == 'X')
                {
                    Console.WriteLine("Computer_X step:\n");
                    move = computer_X.GetMove(board, history.LastOrDefault().Item1, history.LastOrDefault().Item2);
                    board.MakeMove(move.Item1, move.Item2, player);
                }
                else
                {
                    Console.WriteLine("Computer_O step:\n");
                    move = computer_O.GetMove(board, history.LastOrDefault().Item1, history.LastOrDefault().Item2);
                    board.MakeMove(move.Item1, move.Item2, player);
                }

                history.Add(move);
                Console.Clear();
                board.Display();
                if (board.Connectivity(move.Item1, move.Item2, player).Item1 == 5)
                {
                    Console.WriteLine("Computer " + player + " wins!");
                    SaveHistory();
                    break;
                }
                player = (player == 'X') ? 'O' : 'X';
                //System.Threading.Thread.Sleep(1000);
            }
        }
    }
}