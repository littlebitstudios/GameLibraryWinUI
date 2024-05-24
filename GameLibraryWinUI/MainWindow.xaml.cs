using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using YamlDotNet.Serialization;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GameLibraryWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            Window window = this;
            window.ExtendsContentIntoTitleBar = true;  // enable custom titlebar
            window.SetTitleBar(AppTitleBar);      // set user ui element as titlebar

            ReloadGameList();
        }

        private void ReloadGameList()
        {
            string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            GameList.Items.Clear();
            if (Directory.Exists($"{documentsFolder}\\LittleBitGameLibrary\\games"))
            {
                string[] files = System.IO.Directory.GetFiles($"{documentsFolder}\\LittleBitGameLibrary\\games");
                foreach (string file in files)
                {
                    if (file.EndsWith(".yaml"))
                    {
                        var deserializer = new Deserializer();
                        var yamlstring = File.ReadAllText(file);
                        var game = deserializer.Deserialize<Game>(yamlstring);
                        game.filename = file;
                        GameList.Items.Add(game);
                    }
                }
            }
            else
            {
                Directory.CreateDirectory($"{documentsFolder}\\LittleBitGameLibrary\\games");
            }
        }

        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var editdialog = new EditDialog();

            ContentDialog dialog = new()
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot=this.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "New Game",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = editdialog
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary){
                string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                Game game = new Game(editdialog.NameInput.Text, editdialog.DevInput.Text,editdialog.DescInput.Text,editdialog.GenreInput.Text,editdialog.FileNameInput.Text);
                Serializer serializer = new Serializer();
                if (game.filename.EndsWith(".yaml") == false){
                    game.filename = game.filename + ".yaml";
                }

                if (game.filename.StartsWith($"{documentsFolder}\\LittleBitGameLibrary\\Games") == false)
                {
                    game.filename = $"{documentsFolder}\\LittleBitGameLibrary\\Games\\" + game.filename;
                }

                System.IO.File.WriteAllText(game.filename, serializer.Serialize(game));
                ReloadGameList();
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadGameList();
        }

        private void DeleteGameButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedgame = GameList.SelectedItem as Game;
            if (selectedgame != null) { 
                System.IO.File.Delete(selectedgame.filename);
                ReloadGameList();
            }
        }

        private async void GameEditButton_Click(object sender, RoutedEventArgs e)
        {
            var clickedGame = GameList.SelectedItem as Game;
            var editdialog = new EditDialog();

            ContentDialog dialog = new()
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = this.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "New Game",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = editdialog
            };

            editdialog.GenreInput.Text = clickedGame.genre;
            editdialog.DescInput.Text = clickedGame.description;
            editdialog.NameInput.Text = clickedGame.name;
            editdialog.FileNameInput.Text = clickedGame.filename;
            editdialog.DevInput.Text = clickedGame.developer;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                Game game = new Game(editdialog.NameInput.Text, editdialog.DevInput.Text, editdialog.DescInput.Text, editdialog.GenreInput.Text, editdialog.FileNameInput.Text);
                Serializer serializer = new Serializer();
                if (game.filename.EndsWith(".yaml") == false)
                {
                    game.filename = game.filename + ".yaml";
                }

                if (game.filename.StartsWith($"{documentsFolder}\\LittleBitGameLibrary\\Games") == false)
                {
                    game.filename = $"{documentsFolder}\\LittleBitGameLibrary\\Games\\" + game.filename;
                }

                System.IO.File.WriteAllText(game.filename, serializer.Serialize(game));
                ReloadGameList();
            }
        }

        private void GameList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            // Set the selected item of the GameList to the item that was right-clicked on
            GameList.SelectedItem = (e.OriginalSource as FrameworkElement)?.DataContext;
        }
    }
}
