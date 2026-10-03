using Core.Models;
using System.Windows;

namespace Data_Logger_1._3.Services
{
    internal class MessageBoxService
    {
        internal static void Show(Result result, string caption)
        {
            switch (result.PublicMessageType)
            {
                case Result.MessageType.Info:
                    {
                        MessageBox.Show(result.PublicMessage, caption, MessageBoxButton.OK, MessageBoxImage.Information);

                        break;
                    }
                case Result.MessageType.Warning:
                    {
                        MessageBox.Show(result.PublicMessage, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

                        break;
                    }
                case Result.MessageType.Error:
                    {
                        MessageBox.Show(result.PublicMessage, caption, MessageBoxButton.OK, MessageBoxImage.Error);

                        break;
                    }
            }
        }
    }
}
