namespace Meadow
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        static MeadowApp _app;

        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--exitOnDebug") return;

            // instantiate and run new meadow app
            _app = new MeadowApp();
            _app.RunAndBlock();
        }
    }
}
