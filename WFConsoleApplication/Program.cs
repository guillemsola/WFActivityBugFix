using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFActivityLibrary1;

namespace WFConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new WorkflowService();

            Console.WriteLine(">> Start with WF...");
            app.Start();
            while (app.Live)
            {
                app.ListNextActions();
                Console.WriteLine(">> Say your action: ");
                var action = Console.ReadLine();
                app.MoveNext(action);
            }
            
            Console.WriteLine("End of WF!");
            Console.ReadKey();
        }
    }
}
