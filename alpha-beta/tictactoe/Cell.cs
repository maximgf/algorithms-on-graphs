namespace tictactoe
{
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
}