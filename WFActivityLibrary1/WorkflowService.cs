using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Threading;

namespace WFActivityLibrary1
{
    /// <summary>
    /// code
    /// http://msdn.microsoft.com/en-us/library/dd560894.aspx
    /// video
    /// http://code.msdn.microsoft.com/Windows-Workflow-164557c3
    /// </summary>
    public class WorkflowService
    {
        public bool Live;

        AutoResetEvent syncEvent = new AutoResetEvent(false);

        WorkflowApplication wfApp = new WorkflowApplication(new BugActivity());

        public void Start()
        {
            // Handle the desired lifecycle events.
            wfApp.Completed = delegate(WorkflowApplicationCompletedEventArgs e)
            {
                Console.WriteLine("\tWorkflow {0} Completed.", e.InstanceId);
                Live = false;
                syncEvent.Set();
            };

            wfApp.Aborted = delegate(WorkflowApplicationAbortedEventArgs e)
            {
                // Display the exception that caused the workflow 
                // to abort.
                Console.WriteLine("Workflow {0} Aborted.", e.InstanceId);
                Console.WriteLine("Exception: {0}\n{1}",
                    e.Reason.GetType().FullName,
                    e.Reason.Message);
            };

            wfApp.Idle = delegate(WorkflowApplicationIdleEventArgs e)
            {
                Console.WriteLine("\tWorkflow {0} Idle.", e.InstanceId);
                syncEvent.Set();
            };
            Live = true;
            // Start the workflow.
            wfApp.Run();
            syncEvent.WaitOne();
        }

        public void ListNextActions()
        {
            var bookmarks = wfApp.GetBookmarks();

            Console.Write(">>>> List of available actions:");

            foreach (var bookmark in bookmarks)
            {
                Console.Write(" " + bookmark.BookmarkName);
            }

            Console.Write(Environment.NewLine);
        }

        public void MoveNext(string action)
        {
            if (!wfApp.GetBookmarks().Any(b => b.BookmarkName == action))
            {
                return;
            }

            wfApp.ResumeBookmark(action, "1");

            syncEvent.WaitOne();
        }
    }
}
