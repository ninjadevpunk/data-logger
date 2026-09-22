
using MVVMEssentials.Commands;
using System.Diagnostics;

/// <summary>
/// Command for forgot password page
/// </summary>
public class ForgotPasswordCommand : CommandBase
{
    public ForgotPasswordCommand()
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
            Debug.WriteLine($"Exception occurred near ForgotPasswordCommand.Execute(): {e.Message}");
        }
    }
}
