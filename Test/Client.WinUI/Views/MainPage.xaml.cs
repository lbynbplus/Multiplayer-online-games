using System.Collections.ObjectModel;
using Client.WinUI.Models;
using Client.WinUI.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.UI.Xaml.Controls;

namespace Client.WinUI.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    private readonly HubConnection connection;

    ObservableCollection<string> logList = new ObservableCollection<string>();
    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        connection = App.GetService<HubConnection>();
        InitializeComponent();

        LogList.ItemsSource = logList;
    }

    private async void Connect_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        connection.On<string>("FirstPlayerJoin", (message) =>
        {
            DispatcherQueue.TryEnqueue(async () =>
            {
                var newMessage = $"{message}";

                LogStatus.Text = newMessage;

                ContentDialog dialog = new ContentDialog();

                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                dialog.XamlRoot = this.XamlRoot;
                dialog.Title = "创建游戏信息";
                //dialog.PrimaryButtonText = "保存";
                dialog.CloseButtonText = "取消";
                dialog.DefaultButton = ContentDialogButton.Close;
                dialog.Content = new CreateGameDataPage();

                var result = await dialog.ShowAsync();
            });
        });

        connection.On<string>("PlayerJoin", (message) =>
        {
            DispatcherQueue.TryEnqueue(async () =>
            {
                var newMessage = $"{message}";

                LogStatus.Text = newMessage;

                ContentDialog dialog = new ContentDialog();

                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                dialog.XamlRoot = this.XamlRoot;
                dialog.Title = "创建游戏信息";
                //dialog.PrimaryButtonText = "保存";
                dialog.CloseButtonText = "取消";
                dialog.DefaultButton = ContentDialogButton.Close;
                dialog.Content = new JoinGameDataPage();

                var result = await dialog.ShowAsync();

            });
        });


        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                var newMessage = $"{user}: {message}";
                logList.Add(newMessage);
                InfoMsg.Text = newMessage;
            });
        });

        connection.On<GameMatrix>("RollDiceResult", (gameMatrix) =>
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                DiceValue.Text = gameMatrix.DiceValue.ToString();
                Position.Text = gameMatrix.Position.ToString();
                CardColor.Text = gameMatrix.CardColor.ToString();

            });
        });

        try
        {
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
            logList.Add(ex.Message);
        }
    }

    private async void StartGameBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            await connection.InvokeAsync("StartGame");
        }
        catch (Exception ex)
        {
            logList.Add(ex.Message);
        }
    }

    private async void TouziBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            var gameData = App.GameStartData;
            await connection.InvokeAsync("RollDice",
                gameData.PlayerName, gameData.GameName);
        }
        catch (Exception ex)
        {
            logList.Add(ex.Message);
        }
    }
}
