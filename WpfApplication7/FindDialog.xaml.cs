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
    public delegate void Find(string text, bool? cs);
    public partial class FindDialog : Window
    {
        public event Find findNext;
        public event Find findPrev;
        public FindDialog()
        {
            InitializeComponent();
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FindNextBTN_Click(object sender, RoutedEventArgs e)
        {
            findNext.Invoke(FindEDT.Text, matchCaseCB.IsChecked);
        }

        private void FindPrevBTN_Click(object sender, RoutedEventArgs e)
        {
            findPrev.Invoke(FindEDT.Text, matchCaseCB.IsChecked);
        }
    }
}
