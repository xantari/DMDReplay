
using DMDReplay.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        //if (args.Length == 0)
        //{
        //    Console.WriteLine("Parameter not passed in.");
        //}

        //var argsFromMain = args[0].Trim();
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .CreateLogger();
        try
        {
            // Log.Information("Starting DMD Replay Console Application. Command: " + argsFromMain);

            using IHost host = CreateHostBuilder(args).Build();

            var vpmService = host.Services.GetRequiredService<IVPinMameService>();

            vpmService.RunROM(new RomConfiguration()
            {
                ShowExternalDmd = true,
                ShowNativePinmameDmd = true,
                RomName = "dw_l2",
                SwitchInitialization = new List<SwitchConfig>() {
                 new SwitchConfig(22,true), //Close Coin Door
                 new SwitchConfig(24, true), //Always Closed
                 new SwitchConfig(82, true), //Playfield Glass Switch
                }
            });

            Thread.Sleep(20000); //Wait

            //Insert Coin
            for (int i = 0; i < 5; i++)
            {
                vpmService.GetController().Switch[1] = true;
                Thread.Sleep(40);
                vpmService.GetController().Switch[1] = false;
                Thread.Sleep(40);

                Thread.Sleep(1000);
            }

            Log.Information("passed insert coin");

            //Load ball through
            vpmService.GetController().Switch[28] = false; //Ball through entry
            Thread.Sleep(40);
            vpmService.GetController().Switch[25] = true;
            Thread.Sleep(40);
            vpmService.GetController().Switch[26] = true;
            Thread.Sleep(40);
            vpmService.GetController().Switch[27] = true;
            Thread.Sleep(1000);

            Log.Information("ball through finished");

            //Start Game
            for (int i = 0; i < 5; i++)
            {
                vpmService.GetController().Switch[13] = true;
                Thread.Sleep(40);
                vpmService.GetController().Switch[13] = false;
                Thread.Sleep(40);
                Thread.Sleep(1000);
            }

            Log.Information("start game");

            //Launch Ball
            for (int i = 0; i < 5; i++)
            {
                vpmService.GetController().Switch[34] = true;
                Thread.Sleep(40);
                vpmService.GetController().Switch[34] = false;
                Thread.Sleep(40);
                Thread.Sleep(1000);
            }

            Log.Information("launch ball");

            for (int i = 0; i < 10; i++) //Hit bumpers a bunch of times
            {
                vpmService.GetController().Switch[61] = true;
                Thread.Sleep(100);
                vpmService.GetController().Switch[61] = false;
                Thread.Sleep(100);
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    vpmService.GetController().Switch[5] = true;
            //    Thread.Sleep(40);
            //    vpmService.GetController().Switch[5] = false;
            //    Thread.Sleep(40);
            //}

            //switch 34 = plunger

            //Start Game
            //vpmService.GetController().Switch[1] = true;
            //Thread.Sleep(40);
            //vpmService.GetController().Switch[1] = false;
            //Thread.Sleep(40);

            //Log.Information("passed start game");

            ////Launch Plunger
            //vpmService.GetController().Switch[34] = true;
            //Thread.Sleep(40);
            //vpmService.GetController().Switch[34] = false;
            //Thread.Sleep(40);

            //Log.Information("passed launch plunger");

            //vpmService.GetController().Switch[48] = true;
            //Thread.Sleep(40);
            //vpmService.GetController().Switch[48] = false;
            //Thread.Sleep(40);

            //vpmService.GetController().Switch[2] = true;
            //Thread.Sleep(40);
            //vpmService.GetController().Switch[2] = false;
            //Thread.Sleep(40);

            //vpmService.GetController().Switch[3] = true;
            //Thread.Sleep(40);
            //vpmService.GetController().Switch[3] = false;
            //Thread.Sleep(40);


            //vpmService.GetController().Switch[13] = true;
            //Thread.Sleep(40);
            //vpmService.GetController().Switch[13] = false;

            //vpmService.GetController().Switch[10] = true;
            //Thread.Sleep(40);
            //vpmService.GetController().Switch[10] = false;

            //for (int i = 0; i < 10; i++)
            //{
            //    if (i == 22 || i == 24 || i == 82)
            //        continue;

            //    Log.Information("Key: " + i);
            //    vpmService.GetController().Switch[i] = true;
            //    Thread.Sleep(40);
            //    vpmService.GetController().Switch[i] = false;
            //    Thread.Sleep(40);
            //}
            //if (argsFromMain == "DownloadNetsuiteBillingData")
            //{
            //    var netsuiteService = host.Services.GetRequiredService<IDownloadNetsuiteBillingDataService>();
            //    await netsuiteService.RunProcess();
            //}

            //if (argsFromMain == "UpdateFeeExpenseData")
            //{
            //    var feeUpdateService = host.Services.GetRequiredService<IUpdateFeeExpenseDataService>();
            //    await feeUpdateService.RunProcess();
            //}

            Console.Read();

        }
        catch (Exception e)
        {
            Log.Fatal(e, "DMD Replay failed");
            Log.CloseAndFlush();
            throw;
        }
        finally
        {
            Log.Information("DMD Replay completed");
            Log.CloseAndFlush();
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .UseSerilog()
          .UseDefaultServiceProvider((context, options) => { options.ValidateScopes = false; })
          //.ConfigureAppConfiguration((hostingContext, builder) =>
          //{
          //    builder.Sources.Clear();
          //    builder.SetBasePath(AppContext.BaseDirectory);
          //    builder.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);
          //    builder.AddEnvironmentVariables();
          //    builder.AddCommandLine(args);
          //    builder.Build();
          //})
          .ConfigureServices((context, services) =>
          {
              services.AddOptions();

              services.AddTransient<IVPinMameService, VPinMameService>();
          });
}