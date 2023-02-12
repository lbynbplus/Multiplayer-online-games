// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Client.WinUI.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Client.WinUI.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class JoinGameDataPage : Page
{

    private readonly HubConnection connection;

    public JoinGameDataPage()
    {
        this.InitializeComponent();
        connection = App.GetService<HubConnection>();
    }



    private async void JoinBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var gameData = new GameStartData
            {
                PlayerName = PlayerName.Text,
            };
            App.GameStartData = gameData;
            await connection.InvokeAsync("AddUser", gameData);
        }
        catch (Exception ex)
        {
            //messagesList.Items.Add(ex.Message);
        }
    }
}
