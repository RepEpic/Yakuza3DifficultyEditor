using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using LibYakuza3Difficulty;
using MessageBox.Avalonia;
using System;
using System.IO;


namespace Yakuza3DifficultyEditor
{

    public partial class MainWindow : Window
    {
        private string selectedFile = "";
        private string selectedDifficulty = "";

        private TextBlock selectedFileName;
        private TextBlock currentDifficultyText;
        private ComboBox newDifficultyComboBox;

        public MainWindow()
        {

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            // Binds to UI elements
            var menuFileOpen = this.Find<MenuItem>("menuFileOpen");
            newDifficultyComboBox = this.Find<ComboBox>("newDifficultyComboBox");
            selectedFileName = this.Find<TextBlock>("selectedFileName");
            currentDifficultyText = this.Find<TextBlock>("currentDifficultyText");
            var saveButton = this.Find<Button>("saveButton");

            var menuFileOpenClicked = menuFileOpen.GetObservable(MenuItem.ClickEvent);
            menuFileOpenClicked.Subscribe(value => MenuFileOpen_Click());

            var newDifficultySelected = newDifficultyComboBox.GetObservable(ComboBox.SelectedItemProperty);
            newDifficultySelected.Subscribe(value => NewDifficultyComboBox_SelectionChanged());

            var saveButtonClicked = saveButton.GetObservable(Button.ClickEvent);
            saveButtonClicked.Subscribe(value => SaveButton_Click());


        }
        public async void MenuFileOpen_Click()
        {
            Console.WriteLine("Open Click");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filters.Add(new FileDialogFilter() { Name = "Yakuza 3 Save", Extensions = { "sav" } });
            ofd.Filters.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });
            ofd.Directory = Y3Save.GetPath();
            var result = await ofd.ShowAsync((Window)this.GetVisualRoot());
            if (result != null) try
                {
                    Y3Save.Check(result[0]);
                    selectedFile = result[0];
                    selectedFileName.Text = Path.GetFileName(result[0]);
                    currentDifficultyText.Text = Y3Save.ReadDifficulty(selectedFile);
                }
                catch (Exception ex)
                {

                    await MessageBoxManager.GetMessageBoxStandardWindow("Error", ex.Message, MessageBox.Avalonia.Enums.ButtonEnum.Ok).Show();
                }
        }
        public void SaveButton_Click()
        {
            Console.WriteLine("Save Clicked");
            Y3Save.ChangeDifficulty(selectedFile, selectedDifficulty);
            currentDifficultyText.Text = Y3Save.ReadDifficulty(selectedFile);
        }

        private void NewDifficultyComboBox_SelectionChanged()
        {
            ComboBoxItem NewDifficultyComboBox = (ComboBoxItem)newDifficultyComboBox.SelectedItem;
            selectedDifficulty = (string)NewDifficultyComboBox.Content;
            Console.WriteLine(NewDifficultyComboBox.Content);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
