using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Exports all the logs in the project in a single PDF.
    /// </summary>
    public class ExportCommand : CommandBase
    {
        public ExportCommand()
        {
        }

        public override void Execute(object parameter)
        {
            try
            {
                // Your implementation here
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occurred near ExportCommand.Execute(): {e.Message}");
            }
        }
    }

}
