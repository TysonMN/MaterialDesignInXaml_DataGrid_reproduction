using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace WpfApp1 {
  public class MyItem {
    public string MyString { get; set; }
  }

  public class ViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    public string DataBindingSourceLevel => PresentationTraceSources.DataBindingSource.Switch.Level.ToString();

    public void NotifyDataBindingSourceLevelChanged() =>
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataBindingSourceLevel)));

    public List<MyItem> MyItems { get; } = new List<MyItem> {
      new MyItem { MyString = "A" },
      new MyItem { MyString = "B" }
    };

    public static ViewModel DesignTimeViewModel { get; } = new ViewModel();
  }

  public class PopUpTraceListener : DefaultTraceListener {
    public override void Write(string messagePart) => MessageBox.Show(messagePart);
    public override void WriteLine(string messagePart) => MessageBox.Show(messagePart);
  }

  public partial class MainWindow : Window {
    public MainWindow() {
      _ = PresentationTraceSources.DataBindingSource.Listeners.Add(new PopUpTraceListener());
      DataContext = new ViewModel();
      InitializeComponent();
    }

    private void OnClick(object _1, RoutedEventArgs _2) {
      PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
      ((ViewModel)DataContext).NotifyDataBindingSourceLevelChanged();
    }
  }
}