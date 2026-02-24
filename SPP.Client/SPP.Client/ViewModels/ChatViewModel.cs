using SPP.Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SPP.Client.Data.Helpers;
using SPP.Client.Data;
using SPP.Client.Models.SPP.Client.Models;


namespace SPP.Client.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged

    {

        private readonly ChatRepository _repo = new ChatRepository();
        private readonly OllamaService _ollama = new OllamaService();

        public ObservableCollection<ChatMessageModel> Messages { get; }
            = new ObservableCollection<ChatMessageModel>();

        public Guid CurrentSessionId { get; } = Guid.NewGuid();

        private string _userInput;
        public string UserInput
        {
            get => _userInput;
            set { _userInput = value; OnPropertyChanged(); }
        }

        public ICommand SendCommand => new RelayCommand(async _ => await SendAsync());

        private async Task SendAsync()
        {
            if (string.IsNullOrWhiteSpace(UserInput)) return;

            var userMessage = new ChatMessageModel
            {
                SessionId = CurrentSessionId,
                Role = "user",
                Content = UserInput,
                CreatedAt = DateTime.Now
            };

            Messages.Add(userMessage);
            await _repo.SaveMessageAsync(CurrentSessionId, "user", UserInput);

            var history = await _repo.GetMessagesAsync(CurrentSessionId);

            // Create a placeholder for the assistant's response
            var assistantMessage = new ChatMessageModel
            {
                SessionId = CurrentSessionId,
                Role = "assistant",
                Content = "",
                CreatedAt = DateTime.Now
            };
            Messages.Add(assistantMessage);

            var responseBuilder = new StringBuilder();

            // Stream the response token by token
            await _ollama.GenerateStreamAsync(history, token =>
            {
                responseBuilder.Append(token);

                // Update the message on the UI thread
                App.Current.Dispatcher.Invoke(() =>
                {
                    assistantMessage.Content = responseBuilder.ToString();
                });
            });

            // Save the complete message to the repository
            await _repo.SaveMessageAsync(CurrentSessionId, "assistant", responseBuilder.ToString());

            UserInput = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
