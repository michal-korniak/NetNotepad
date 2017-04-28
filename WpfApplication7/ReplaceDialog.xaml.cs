using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication7
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class ReplaceDialog : Window
    {
        #region delegats
        public delegate void Find(string text, bool? cs);
        public delegate void Replace(string newValue);
        public delegate void ReplaceAll(string oldValue, string newValue, bool? cs);
        #endregion
        #region events
        public event Find FindEvent;
        public event Replace ReplaceEvent;
        public event ReplaceAll ReplaceAllEvent;
        #endregion
        public ReplaceDialog()
        {
            InitializeComponent();
        }

        private void FindBTN_Click(object sender, RoutedEventArgs e)
        {
            FindEvent.Invoke(FindEDT.Text,MatchCaseCHB.IsChecked);
        }

        private void ReplaceBTN_Click(object sender, RoutedEventArgs e)
        {
            ReplaceEvent.Invoke(ReplaceEDT.Text);
        }

        private void ReplaceAllBTN_Click(object sender, RoutedEventArgs e)
        {
            ReplaceAllEvent.Invoke(FindEDT.Text, ReplaceEDT.Text, MatchCaseCHB.IsChecked);
        }
    }
}
