using Project_CS464.Model; // Đảm bảo đã using Model
using Project_CS464.View;  // Đảm bảo đã using View
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project_CS464.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {

        private static UserDBEntities _db;
        public static UserDBEntities db
        {
            get
            {
                if (_db == null) _db = new UserDBEntities();
                return _db;
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        public ICommand LoginCommand { get; set; }

        public UserViewModel()
        {
            LoginCommand = new RelayCommand<object>((p)=>
            {
                Login(p);
            }
            );
        }

        void Login(object p)
        {
            Window currentWindow = p as Window;
            if (p == null) return;

            var passwordBox = currentWindow.FindName("PasswordBoxInput") as PasswordBox;
            string password = passwordBox != null ? passwordBox.Password : "";

            var accCount = db.Users.Where(x => x.Username == Username && x.Password == password).Count();

            if (accCount > 0)
            {
                var user = db.Users.Where(x => x.Username == Username && x.Password == password).First();

                if (user.Role != null)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    currentWindow.Close();
                }
                else
                {
                    MessageBox.Show("Tài khoản của bạn không có quyền truy cập!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // implementation của INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}