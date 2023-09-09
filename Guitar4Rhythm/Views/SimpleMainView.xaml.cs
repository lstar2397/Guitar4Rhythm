using System.Windows;

namespace Guitar4Rhythm.Views {
    /// <summary>
    /// SimpleMainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SimpleMainView : Window {
        public SimpleMainView() {
            InitializeComponent();

            Closing += ((ViewModels.MainViewModel)DataContext).OnClosing;
        }
    }
}
