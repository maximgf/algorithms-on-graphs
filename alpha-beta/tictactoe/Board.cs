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
 
        }

        private void DeleteMove(int x, int y)
        {
            cells[y, x].SetValue(' ');
        }

        public bool Is_win(char player)
        {
            for(int i = 0; i < 20;i++)
            {
                if(Connectivity(0,i,player).Item1 == 5 || Connectivity(i, 0, player).Item1 == 5)
                {
                    return true;
                }
            }
            return false;
        }
        
        public (int, int) Connectivity(int x, int y, char player)
        {
            int count = 0;
            int count_max = 0;
            int direction = 0;

            // Ïðîâåðêà ïî ãîðèçîíòàëè
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

            // Ïðîâåðêà ïî âåðòèêàëè
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

            // Ïðîâåðêà ïî äèàãîíàëè (ñëåâà ñâåðõó íàïðàâî âíèç)
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

            // Ïðîâåðêà ïî äèàãîíàëè (ñëåâà ñíèçó íàïðàâî ââåðõ)
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



        public (int, int) Hint(char player, int depth, int prevX, int prevY)
        {
            int bestScore = player == 'X' ? int.MinValue : int.MaxValue;
            int bestX = -1;
            int bestY = -1;

            int startX = Math.Max(0, prevX - 4);
            int endX = Math.Min(19, prevX + 4);
            int startY = Math.Max(0, prevY - 4);
            int endY = Math.Min(19, prevY + 4);

            for (int i = startY; i <= endY; i++)
            {
                for (int j = startX; j <= endX; j++)
                {
                    if (IsValidMove(j, i))
                    {
                        MakeMove(j, i, player);
                        int score = Minimax(depth - 1, player == 'O', int.MinValue, int.MaxValue, prevX, prevY);
                        DeleteMove(j, i);

                        if (player == 'X' && score > bestScore)
                        {
                            bestScore = score;
                            bestX = j;
                            bestY = i;
                        }
                        else if (player == 'O' && score < bestScore)
                        {
                            bestScore = score;
                            bestX = j;
                            bestY = i;
                        }
                    }
                }
            }

            return (bestX, bestY);
        }

        private int Minimax(int depth, bool isMaximizingPlayer, int alpha, int beta, int prevX, int prevY)
        {
            if (depth == 0 || Is_win('X') || Is_win('O'))
            {
                return Evaluate();
            }

            int startX = Math.Max(0, prevX - 4);
            int endX = Math.Min(19, prevX + 4);
            int startY = Math.Max(0, prevY - 4);
            int endY = Math.Min(19, prevY + 4);

            if (isMaximizingPlayer)
            {
                int bestScore = int.MinValue;
                for (int i = startY; i <= endY; i++)
                {
                    for (int j = startX; j <= endX; j++)
                    {
                        if (IsValidMove(j, i))
                        {
                            MakeMove(j, i, 'X');
                            int score = Minimax(depth - 1, false, alpha, beta, prevX, prevY);
                            DeleteMove(j, i);
                            bestScore = Math.Max(score, bestScore);
                            alpha = Math.Max(alpha, bestScore);
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int i = startY; i <= endY; i++)
                {
                    for (int j = startX; j <= endX; j++)
                    {
                        if (IsValidMove(j, i))
                        {
                            MakeMove(j, i, 'O');
                            int score = Minimax(depth - 1, true, alpha, beta, prevX, prevY);
                            DeleteMove(j, i);
                            bestScore = Math.Min(score, bestScore);
                            beta = Math.Min(beta, bestScore);
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                return bestScore;
            }
        }

        private int Evaluate()
        {
            if (Is_win('X'))
            {
                return 1;
            }
            else if (Is_win('O'))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}