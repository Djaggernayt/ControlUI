using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ControlUI
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private UserModel _selectedUser;
        private bool _isLoading;

        public ObservableCollection<UserModel> Users { get; set; }

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand GenerateUsersCommand { get; }

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            LoadTestData();

            SaveCommand = new RelayCommand(_ => Save(), _ => SelectedUser != null);
            LoadCommand = new RelayCommand(_ => LoadTestData());
            GenerateUsersCommand = new RelayCommand(_ => GenerateUsers(1000));
        }

        private async void LoadTestData()
        {
            IsLoading = true;

            await Task.Delay(800); 

            Users.Clear();
            Users.Add(new UserModel { Id = 1, Name = "Иван", Email = "ivan@test.com", IsActive = true, Role = "Admin" });
            Users.Add(new UserModel { Id = 2, Name = "Мария", Email = "maria@test.com", IsActive = false, Role = "User" });
            Users.Add(new UserModel { Id = 3, Name = "Алексей", Email = "alex@test.com", IsActive = true, Role = "User" });

            SelectedUser = Users.FirstOrDefault();
            IsLoading = false;
        }

        private void Save()
        {
            
            MessageBox.Show($"Сохранено: {SelectedUser?.Name}");
        }

        private void GenerateUsers(int count)
        {
            Users.Clear();
            var rnd = new Random();

            for (int i = 1; i <= count; i++)
            {
                Users.Add(new UserModel
                {
                    Id = i,
                    Name = $"User {i}",
                    Email = $"user{i}@test.com",
                    IsActive = rnd.NextDouble() > 0.3,
                    Role = rnd.NextDouble() > 0.7 ? "Admin" : "User"
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
