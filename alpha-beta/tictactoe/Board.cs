namespace tictactoe
{
    public class Board
    {
        private Cell[,] cells = new Cell[20, 20];

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
            string count = new string('-', 43);
            Console.Write("  ");
            for(int k = 0; k < 20;k++)
            {
                Console.Write($"|{k%10}");
            }
            Console.WriteLine("|");

            for (int i = 0; i < 20; ++i)
            {
                if(i < 10)
                {
                    Console.Write($"{i} ");
                }
                else 
                {
                    Console.Write(i);
                }
                
                for (int j = 0; j < 20; ++j)
                {
                    Console.Write($"|{cells[i, j].GetValue()}");
                }
                Console.WriteLine("|");
                //Console.WriteLine(count);
            }
        }

        public bool IsValidMove(int x, int y)
        {
            return x >= 0 && x < 20 && y >= 0 && y < 20 && cells[y, x].GetValue() == ' ';
        }

        public void MakeMove(int x, int y, char player)
        {
            cells[y, x].SetValue(player);
        }

        private void DeleteMove(int x, int y)
        {
            cells[y, x].SetValue(' ');
        }

        public bool Is_win(char player, int lastX = 10, int lastY = 10, int window = 20)
        {
            for (int i = 0; i <= 19; i++)
            {
                for (int j = 0; j <= 19; j++)
                {
                    if (Connectivity(j, i, player).Item1 == 5)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Is_winm(char player,int x,int y)
        {
            return (Connectivity(x, y, player).Item1 == 5);
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

        private int CheckHorizontal(int x, int y, char player)
        {
            int count = 0;
            int maxCount = 0;
            for (int i = 0; i < 20; ++i)
            {
                if (cells[y, i].GetValue() == player)
                {
                    ++count;
                    if (count > maxCount)
                    {
                        maxCount = count;
                        if ((i + 1 >= 20) || (i - 1 < 0) || (cells[y, i + 1].GetValue() != player && cells[y, i + 1].GetValue() != ' '))
                        {
                            maxCount -= 1;
                        }
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return maxCount * maxCount;
        }

        private int CheckVertical(int x, int y, char player)
        {
            int count = 0;
            int maxCount = 0;
            for (int i = 0; i < 20; ++i)
            {
                if (cells[i, x].GetValue() == player)
                {
                    ++count;
                    if (count > maxCount)
                    {
                        maxCount = count;
                        if ((i + 1 >= 20) || (i - 1 < 0) || (cells[i + 1, x].GetValue() != player && cells[i + 1, x].GetValue() != ' '))
                        {
                            maxCount -= 1;
                        }
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return maxCount * maxCount;
        }

        private int CheckDiagonalLeftToRight(int x, int y, char player)
        {
            int count = 0;
            int maxCount = 0;

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
                    if (count > maxCount)
                    {
                        maxCount = count;
                        if ((curr_y - 1 < 0) || (curr_y + 1 >= 20) || (curr_x + 1 >= 20) || (curr_x - 1 < 0) || (cells[curr_y - 1, curr_x + 1].GetValue() != player && cells[curr_y - 1, curr_x + 1].GetValue() != ' '))
                        {
                            maxCount -= 1;
                        }
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return maxCount * maxCount;
        }

        private int CheckDiagonalRightToLeft(int x, int y, char player)
        {
            int count = 0;
            int maxCount = 0;

            int b = -1 * (-1 * y + 1 * x);
            int curr_x = -1;
            int curr_y = b - 1;
            for (int i = 0; i < 20; i++)
            {
                curr_y += 1;
                curr_x += 1;
                if (curr_x < 20 && curr_y < 20 && curr_y >= 0 && cells[curr_y, curr_x].GetValue() == player)
                {
                    ++count;
                    if (count > maxCount)
                    {
                        maxCount = count;
                        if ((curr_y + 1 >= 20) || (curr_y - 1 < 0) || (curr_x + 1 >= 20) || (curr_x - 1 < 0) || (cells[curr_y + 1, curr_x + 1].GetValue() != player && cells[curr_y + 1, curr_x + 1].GetValue() != ' '))
                        {
                            maxCount -= 1;
                        }
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return maxCount * maxCount;
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

        public (int, int) Hint(char player, int depth, int prevX, int prevY)
        {
            int bestScore = int.MinValue;
            int bestX = -1;
            int bestY = -1;

            
            var possibleMoves = GetPossibleMovesInRadius(1);

            foreach (var (x, y) in possibleMoves)
            {
                if (IsValidMove(x, y))
                {
                    MakeMove(x, y, player);
                    int score = Minimax(depth - 1, false, int.MinValue, int.MaxValue, x, y, 1);
                    DeleteMove(x, y);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestX = x;
                        bestY = y;
                    }
                }
            }

            return (bestX, bestY);
        }

        private int Minimax(int depth, bool isMaximizingPlayer, int alpha, int beta, int prevX, int prevY, int radius)
        {
            if (Is_winm('X', prevX, prevY))
            {
                return -1000000;
            }
            if (Is_winm('O', prevX, prevY))
            {
                return 1000000;
            }
            if (depth == 0)
            {
                return Evaluate();
            }

            var possibleMoves = GetPossibleMovesInRadius(radius);

            if (isMaximizingPlayer)
            {
                int bestScore = int.MinValue;
                foreach (var (x, y) in possibleMoves)
                {
                    //if (IsValidMove(x, y))
                    {
                        MakeMove(x, y, 'O');
                        int score = Minimax(depth - 1, false, alpha, beta, x, y, radius);
                        DeleteMove(x, y);
                        bestScore = Math.Max(score, bestScore);
                        alpha = Math.Max(alpha, bestScore);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                foreach (var (x, y) in possibleMoves)
                {
                    //if (IsValidMove(x, y))
                    {
                        MakeMove(x, y, 'X');
                        int score = Minimax(depth - 1, true, alpha, beta, x, y, radius);
                        DeleteMove(x, y);
                        bestScore = Math.Min(score, bestScore);
                        beta = Math.Min(beta, bestScore);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
                return bestScore;
            }
        }

        private int Evaluate()
        {
            int xScore = 0;
            int oScore = 0;
            int horizontal = 0;
            int vertical = 0;
            int diagonalLeftToRight = 0;
            int diagonalRightToLeft = 0;

            // Оценка для игрока 'X'
            for (int i = 0; i < 20; i++)
            {
                horizontal += CheckHorizontal(0, i, 'X');
                vertical += CheckVertical(i, 0, 'X');
                diagonalLeftToRight += CheckDiagonalLeftToRight(i, 19, 'X');
                diagonalRightToLeft += CheckDiagonalRightToLeft(0, i, 'X');
            }
            for (int i = 0; i < 19; i++)
            {
                diagonalLeftToRight += CheckDiagonalLeftToRight(0, i, 'X');
                diagonalRightToLeft += CheckDiagonalRightToLeft(i + 1, 0, 'X');
            }
            xScore = /*(int)(1.1 * ((double)*/(horizontal + vertical + diagonalRightToLeft + diagonalLeftToRight);

            // Оценка для игрока 'O'
            horizontal = 0;
            vertical = 0;
            diagonalLeftToRight = 0;
            diagonalRightToLeft = 0;
            for (int i = 0; i < 20; i++)
            {
                horizontal += CheckHorizontal(0, i, 'O');
                vertical += CheckVertical(i, 0, 'O');
                diagonalLeftToRight += CheckDiagonalLeftToRight(i, 19, 'O');
                diagonalRightToLeft += CheckDiagonalRightToLeft(0, i, 'O');
            }
            for (int i = 0; i < 19; i++)
            {
                diagonalLeftToRight += CheckDiagonalLeftToRight(0, i, 'O');
                diagonalRightToLeft += CheckDiagonalRightToLeft(i + 1, 0, 'O');
            }
            oScore = (horizontal + vertical + diagonalRightToLeft + diagonalLeftToRight);
            return oScore - xScore;
        }

        private List<(int, int)> GetPossibleMovesInRadius(int radius)
        {
            var possibleMoves = new List<(int, int)>();

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (cells[i, j].GetValue() != ' ')
                    {
                        for (int y = Math.Max(0, i - radius); y <= Math.Min(19, i + radius); y++)
                        {
                            for (int x = Math.Max(0, j - radius); x <= Math.Min(19, j + radius); x++)
                            {
                                if (cells[y, x].GetValue() == ' ' && !possibleMoves.Contains((x, y)))
                                {
                                    possibleMoves.Add((x, y));
                                }
                            }
                        }
                    }
                }
            }

            return possibleMoves;
        }
    }
}