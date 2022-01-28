namespace GameOfLife
{
    class Game
    {
        private Form1 _form;
        private bool _isRunning = false;
        public Game(Form1 form)
        {
            _form = form;
        }
        public void Start()
        {
            _isRunning = true;
        }
        public void Stop() => _isRunning = false;
    }
}