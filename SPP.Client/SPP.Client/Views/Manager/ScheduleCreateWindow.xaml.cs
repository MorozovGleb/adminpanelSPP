using SPP.Client.Models.SPP.Client.Models;
using SPP.Client.Services;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SPP.Client.Views.Manager
{
    public partial class ScheduleCreateWindow : Window
    {
        private readonly OllamaService _ollamaService = new OllamaService();
        private readonly List<ChatMessageModel> _messages = new List<ChatMessageModel>();

        public ScheduleCreateWindow()
        {
            InitializeComponent();
            MessagesListBox.ItemsSource = _messages;
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userText = UserInputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(userText))
                return;

            var userMessage = new ChatMessageModel
            {
                Role = "user",
                Content = userText
            };

            _messages.Add(userMessage);
            MessagesListBox.Items.Refresh();
            UserInputTextBox.Text = "";

            // создаем пустое сообщение ассистента
            var assistantMessage = new ChatMessageModel
            {
                Role = "assistant",
                Content = ""
            };

            _messages.Add(assistantMessage);
            MessagesListBox.Items.Refresh();

            await _ollamaService.GenerateStreamAsync(_messages, token =>
            {
                Dispatcher.Invoke(() =>
                {
                    assistantMessage.Content += token;
                    MessagesListBox.Items.Refresh();
                    MessagesListBox.ScrollIntoView(assistantMessage);
                });
            });
        }
    }
}