using Lotlab.PluginCommon;
using System.Collections.ObjectModel;

namespace PluginTemplate
{
    class PluginControlViewModel : PropertyNotifier
    {
        ObservableCollection<LogItem> _logs { get; set; } = null;
        public ObservableCollection<LogItem> Logs { get => _logs; set { _logs = value; OnPropertyChanged(); } }

        public PluginControlViewModel()
        {

        }
    }
}
