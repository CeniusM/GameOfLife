namespace GameOfLife;

static class Program
{
    private static GameOfLife.Game? game;
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        Form1 myForm = new Form1();

        var gameThread = new Thread(() => StartGame(myForm));

        myForm.Shown += (s, e) => gameThread.Start();
        myForm.FormClosing += (s, e) =>
        {
            game?.Stop();
            gameThread.Join();
        };

        Application.Run(myForm);
    }
    private static void StartGame(Form1 myForm)
    {
        game = new GameOfLife.Game(myForm, 40, 40);
        game.Start();
    }
}