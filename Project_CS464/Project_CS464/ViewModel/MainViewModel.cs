using Project_CS464.View;
using System.Windows;
using System.Windows.Input;

namespace Project_CS464.ViewModel
{
    public class MainViewModel
    {

        public ICommand LogoutCommand { get; set; }

        public MainViewModel()
        {
 
            LogoutCommand = new RelayCommand<object>((p) =>
            {
                if (p == null) return;
                Window currentWindow = p as Window;
                if (currentWindow == null) return;
                Login loginWindow = new Login();
                loginWindow.Show();
                currentWindow.Close();
                
            },
            (p) => { return true; });
        }
    }
}