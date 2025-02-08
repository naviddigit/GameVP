namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        int count = 0;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            try
            {
                new Tasks.AutoLimit().DeleteSessionDublicate();

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  \ber-1.:" + ex.Message + "\n");
                Console.ForegroundColor = ConsoleColor.White;

                new Tasks.AutoLimit().DeleteSessionDublicate();


            }




            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    if (count == 0)
            //    {
            //        //_logger.LogInformation("START at: {time}", DateTimeOffset.Now);
            //        Console.WriteLine("START at: " + DateTimeOffset.Now);
            //        Console.Beep();
            //        new Tasks.AutoLimit().DeleteSessionDublicate();
            //    }
            //    var countDilay = 60;
            //    if (count <= countDilay)
            //    {
            //        if (count != countDilay)
            //        {
            //            Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
            //            Console.Write("Netx Time: " + count + "   ");
            //        }
            //        count++;
            //    }
            //    else
            //    {
            //        count = 1;
            //        Console.Write("...");
            //        new Tasks.AutoLimit().DeleteSessionDublicate();
            //    }
            //    await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}