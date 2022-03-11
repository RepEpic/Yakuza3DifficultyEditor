using System;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using LibYakuza3Difficulty;

namespace Yakuza3DifficultyEditor.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedFile = "";
        private string selectedDifficulty = "";
        public MainWindow()
        {
            InitializeComponent();
        }
        void MenuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Open Click");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Yakuza 3 Save Files (*.sav)|*.sav|All files (*.*)|*.*";
            ofd.InitialDirectory = Y3Save.GetPath();
            if (ofd.ShowDialog() == true) try
                {
                    Y3Save.Check(ofd.FileName);
                    selectedFile = ofd.FileName;
                    selectedFileName.Text = ofd.SafeFileName;
                    currentDifficultyText.Text = Y3Save.ReadDifficulty(selectedFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK);
                }
        }

        void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Save Clicked");
            Y3Save.ChangeDifficulty(selectedFile, selectedDifficulty);
            currentDifficultyText.Text = Y3Save.ReadDifficulty(selectedFile);
        }

        private void NewDifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem NewDifficultyComboBox = (ComboBoxItem)newDifficultyComboBox.SelectedItem;
            selectedDifficulty = (string)NewDifficultyComboBox.Content;
            Console.WriteLine(NewDifficultyComboBox.Content);
        }
    }
}
