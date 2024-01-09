﻿using DeathMustDieSaveEditor.Core.Logic;
using DeathMustDieSaveEditor.Core.Models;
using DeathMustDieSaveEditor.Core.Models.SaveStructure;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeathMustDieSaveEditor.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataManager DataManager = new DataManager();

        private bool AreComponentsLoaded = false;

        private string[] HeroNames = ["Avoron", "Kront", "Merris", "Nixi", "Skadi"];

        private string SelectedClass = "Knight";

        private bool IsHeroUnlocked = false;

        private List<Item> LoadedItems;

        public MainWindow()
        {
            InitializeComponent();
            foreach (var heroName in HeroNames)
            {
                this.HeroNameComboBox.Items.Add(heroName);
            }

            this.AreComponentsLoaded = true;
            LoadDataManager();
        }

        private void LoadDataManager()
        {
            string loadedFilePath = this.DataManager.TryLoadSaveAlone();
            if (string.IsNullOrEmpty(loadedFilePath))
                return;

            var fileName = System.IO.Path.GetFileName(loadedFilePath);
            this.LoadedFileNameLabel.Content = fileName;
            this.LoadInitialValues();
        }

        private void LoadCustomSaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Save Files|*.sav";

            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string filePath = openFileDialog.FileName;
            this.DataManager.LoadSave(filePath);

            var fileName = System.IO.Path.GetFileName(filePath);
            this.LoadedFileNameLabel.Content = fileName;

            this.LoadInitialValues();
        }

        private void LoadInitialValues()
        {
            this.GoldTextBox.Text = this.DataManager.GetGold().ToString();
            this.HeroNameComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DataManager.SaveChanges();
            System.Windows.Forms.MessageBox.Show("Changes are saved. Have Fun!", "Saved");
        }

        private void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GoldTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!AreComponentsLoaded)
                return;

            string stringValue = this.GoldTextBox.Text;
            if (string.IsNullOrEmpty(stringValue))
            {
                stringValue = "0";
                this.GoldTextBox.Text = stringValue;
            }

            var newGoldValue = int.Parse(stringValue);
            this.DataManager.SetGold(newGoldValue);
        }

        private void HeroNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = HeroNameComboBox.SelectedIndex;
            switch (index)
            {
                case 0:
                    this.SelectedClass = "Knight";
                    this.HeroAvatarImg.Source = new BitmapImage(new Uri(@"/Avoron.png", UriKind.Relative));
                    break;
                case 1:
                    this.SelectedClass = "Barbarian";
                    this.HeroAvatarImg.Source = new BitmapImage(new Uri(@"/Kront.png", UriKind.Relative));
                    break;
                case 2:
                    this.SelectedClass = "Sorceress";
                    this.HeroAvatarImg.Source = new BitmapImage(new Uri(@"/Witch.png", UriKind.Relative));
                    break;
                case 3:
                    this.SelectedClass = "Assassin";
                    this.HeroAvatarImg.Source = new BitmapImage(new Uri(@"/Nixi.png", UriKind.Relative));
                    break;
                case 4:
                    this.SelectedClass = "Warrior";
                    this.HeroAvatarImg.Source = new BitmapImage(new Uri(@"/Skadi.png", UriKind.Relative));
                    break;
                default:
                    break;
            }

            if (this.DataManager.IsHeroUnlocked(this.SelectedClass))
            {
                this.IsHeroUnlocked = true;
                this.LoadEquipment();
            }
            else
            {
                this.IsHeroUnlocked = false;
            }
        }

        private void LoadEquipment()
        {
            this.LoadedItems = this.DataManager.GetItems(this.SelectedClass)
                .ToList();
        }

        private void ItemSelectionImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Img was clicked");
        }

        private void ItemWasClicked(ItemType itemType)
        {
            throw new NotImplementedException();
        }
    }
}