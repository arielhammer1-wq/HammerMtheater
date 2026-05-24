using Model;
using MoviesInterface;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HammerMtheater.Pages
{
    public partial class EditProfilePage : Page
    {
        private readonly MoviesFunctions _api = new MoviesFunctions();

        public EditProfilePage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (App.CurrentUser == null)
            {
                MessageBox.Show("No user is currently logged in.", "Error");
                NavigationService.GoBack();
                return;
            }

            TxtId.Text = App.CurrentUser.Id.ToString();
            TxtUsername.Text = App.CurrentUser.Username;
            TxtEmail.Text = App.CurrentUser.Email;
            TxtPassword.Password = App.CurrentUser.Pass;

           
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null)
            {
                MessageBox.Show("No user is currently logged in.", "Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtUsername.Text))
            {
                MessageBox.Show("Username cannot be empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                MessageBox.Show("Email cannot be empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtPassword.Password))
            {
                MessageBox.Show("Password cannot be empty.");
                return;
            }

            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                User updatedUser = App.CurrentUser;

                updatedUser.Username = TxtUsername.Text.Trim();
                updatedUser.Email = TxtEmail.Text.Trim();
                updatedUser.Pass = TxtPassword.Password;

                await _api.UpdateUser(updatedUser);

                App.CurrentUser = updatedUser;

                MessageBox.Show("Profile updated successfully!");

                NavigationService.Navigate(new HomePage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update profile: " + ex.Message);
            }
            finally
            {
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}