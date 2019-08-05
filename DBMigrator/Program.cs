using DbUp;
using System;
using System.Linq;
using System.Reflection;


namespace DBMigrator
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString =
         args.FirstOrDefault()
         ?? "Data Source=(local);Database=DemoDB;Trusted_Connection=True;";

            var engine =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .LogScriptOutput()
                    .Build();

            ScriptingUpgrader upgradScriptingEngine = new ScriptingUpgrader(connectionString, engine);
            var result = upgradScriptingEngine.Run(args);
            
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
