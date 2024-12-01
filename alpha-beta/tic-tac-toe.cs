using System;
using System.Collections.Generic;
using System.IO;

public class Board
{
    private Cell[,] cells = new Cell[20, 20];
    private (int, int) lastMove;

    public Board()
    {
        for (int i = 0; i < 20; ++i)
        {
            for (int j = 0; j < 20; ++j)
            {
                cells[i, j] = new Cell();
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < 20; ++i)
        {
            for (int j = 0; j < 20; ++j)
            {
                Console.Write(cells[i, j].GetValue());
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(int x, int y)
    {
        return x >= 0 && x < 20 && y >= 0 && y < 20 && cells[y, x].GetValue() == ' ';
    }

    public void MakeMove(int x, int y, char player)
    {
        cells[y, x].SetValue(player);
        lastMove = (x, y);
    }

    public (int, int) Connectivity(int x, int y, char player)
    {
        int count = 0;
        int count_max = 0;
        int direction = 0;

        // Проверка по горизонтали
        for (int i = 0; i < 20; ++i)
        {
            if (cells[y, i].GetValue() == player)
            {
                ++count;
                if (count_max < count)
                {
                    count_max = count;
                    direction = 0;
                }
            }
            else
            {
                count = 0;
            }
        }
        count = 0;

        // Проверка по вертикали
        for (int i = 0; i < 20; ++i)
        {
            if (cells[i, x].GetValue() == player)
            {
                ++count;
                if (count_max < count)
                {
                    count_max = count;
                    direction = 1;
                }
            }
            else
            {
                count = 0;
            }
        }
        count = 0;

        // Проверка по диагонали (слева сверху направо вниз)
        int b = -1 * (-1 * y - 1 * x);
        int curr_x = -1;
        int curr_y = b + 1;
        for (int i = 0; i < 20; i++)
        {
            curr_y -= 1;
            curr_x += 1;
            if (curr_x < 20 && curr_y < 20 && curr_y >= 0 && cells[curr_y, curr_x].GetValue() == player)
            {
                ++count;
                if (count_max < count)
                {
                    count_max = count;
                    direction = 2;
                }
            }
            else
            {
                count = 0;
            }
        }
        count = 0;

        // Проверка по диагонали (слева снизу направо вверх)
        b = -1 * (-1 * y + 1 * x);
        curr_x = -1;
        curr_y = b - 1;
        for (int i = 0; i < 20; i++)
        {
            curr_y += 1;
            curr_x += 1;
            if (curr_x < 20 && curr_y < 20 && curr_y >= 0 && cells[curr_y, curr_x].GetValue() == player)
            {
                ++count;
                if (count_max < count)
                {
                    count_max = count;
                    direction = 3;
                }
            }
            else
            {
                count = 0;
            }
        }

        if (count_max > 5)
        {
            count_max = 5;
        }

        return (count_max, direction);
    }

    public bool IsFull()
    {
        for (int i = 0; i < 20; ++i)
        {
            for (int j = 0; j < 20; ++j)
            {
                if (cells[i, j].GetValue() == ' ')
                {
                    return false;
                }
            }
        }
        return true;
    }

    public (int, int) GetLastMove()
    {
        return lastMove;
    }

    public (int, int) Hint(char player)
    {
        Random rand = new Random();
        while (true)
        {
            int x = rand.Next(20);
            int y = rand.Next(20);
            if (IsValidMove(x, y))
            {
                return (x, y);
            }
        }
    }
}

public class Cell
{
    private char value;

    public Cell()
    {
        value = ' ';
    }

    public void SetValue(char val)
    {
        value = val;
    }

    public char GetValue()
    {
        return value;
    }
}

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

    public (int, int) GetMove(Board board)
    {
        return board.Hint(player);
    }
}

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
                var hint = board.Hint(player);
                Console.WriteLine("Hint* X: " + hint.Item1 + " Y: " + hint.Item2);
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
                if (board.Connectivity(x, y, player).Item1 == 5)
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
                var move = computer.GetMove(board);
                board.MakeMove(move.Item1, move.Item2, player);
                history.Add(move);
                board.Display();
                if (board.Connectivity(move.Item1, move.Item2, player).Item1 == 5)
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
                move = computer_X.GetMove(board);
                board.MakeMove(move.Item1, move.Item2, player);
            }
            else
            {
                Console.WriteLine("Computer_O step:\n");
                move = computer_O.GetMove(board);
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