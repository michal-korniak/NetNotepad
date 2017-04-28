using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

namespace WpfApplication7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string prev = "";
        private FileInfo fileDir;
        private int FindCarretIndex;    //helpful to findNext(), thanks to it selected() and findNext() works good together
        bool FindPrevRunnedBefore=false;

        public MainWindow()
        {
            InitializeComponent();
        }
        private bool okToContinue()
        {
            if (prev == textBox.Text)
                return true;
            MessageBoxResult result = MessageBox.Show("Do you want to save changes?", ".NetNotepad", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                save();
                return true;
            }
            else if (result == MessageBoxResult.No)
                return true;
            else
                return false;
        }
        private void save()
        {
            if (prev == "")
                saveFileAs();
            else
            {
                using (StreamWriter strWrite = new StreamWriter(fileDir.FullName))
                    strWrite.Write(textBox.Text);
            }

        }
        private void saveFileAs()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Text document (.txt) |*.txt";
            dlg.FileName = "untitled";
            dlg.DefaultExt = "txt";
            bool? feedback = dlg.ShowDialog();
            if (feedback != true)
                return;
            fileDir = new FileInfo(dlg.FileName);
            using (StreamWriter strWrite = new StreamWriter(fileDir.FullName))
                strWrite.Write(textBox.Text);
            prev = textBox.Text;
            FileName.Content = fileDir.Name;

        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (!okToContinue())
                return;
            textBox.Clear();
            prev = "";
            FileName.Content = " ";
        }

        private void Open_Clicked(object sender, RoutedEventArgs e)
        {
            if (!okToContinue())
                return;
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Text document (.txt) |*.txt";
            bool? feedback = dlg.ShowDialog();
            if (feedback != true)
                return;
            fileDir = new FileInfo(dlg.FileName);
            using (StreamReader strRead = new StreamReader(dlg.FileName))
                textBox.Text = strRead.ReadToEnd();
            prev = textBox.Text;
            FileName.Content = fileDir.Name;


        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            textBox.Undo();
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            textBox.Cut();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            textBox.Copy();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            textBox.Paste();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            textBox.SelectedText = "";
        }

        private void Font_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FontDialog fontDlg = new System.Windows.Forms.FontDialog();
            //fontDlg.ShowColor = true; ///TODO
            fontDlg.ShowEffects = false;
            fontDlg.Font = new System.Drawing.Font(textBox.FontFamily.ToString(), (float)textBox.FontSize,
                (textBox.FontStyle == FontStyles.Italic ? System.Drawing.FontStyle.Italic : System.Drawing.FontStyle.Regular) |
                (textBox.FontWeight == FontWeights.Bold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular));

            if (fontDlg.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                string fontFamily = fontDlg.Font.FontFamily.ToString();
                fontFamily = fontFamily.Substring(18, fontFamily.Length - 19);  ///because in the begging string is "[Font Family: Name=FONT]"

                textBox.FontFamily = new FontFamily(fontFamily);
                textBox.FontSize = fontDlg.Font.Size;

                textBox.FontWeight = fontDlg.Font.Bold ? (FontWeights.Bold) : (FontWeights.Normal);
                textBox.FontStyle = fontDlg.Font.Italic ? (FontStyles.Italic) : (FontStyles.Normal);
            }

        }

        private void Word_Wrap_Click(object sender, RoutedEventArgs e)
        {
            textBox.TextWrapping = Word_Wrap.IsChecked ? TextWrapping.WrapWithOverflow : TextWrapping.NoWrap;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SaveFileAs_Click(object sender, RoutedEventArgs e)
        {
            saveFileAs();
        }

        private void Text_Changed(object sender, TextChangedEventArgs e)
        {   
            Stats.Content = string.Format("Lengths : {0},  Lines : {1}", textBox.Text.Length, textBox.Text.Split('\n').Length);
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            FindDialog findDlg = new FindDialog();
            findDlg.Owner = this;
            findDlg.findNext += findNext;
            findDlg.findPrev += findPrev;
            findDlg.Show();
            FindPrevRunnedBefore = false;
        }
        private void findNext(string text, bool? cs)
        {
            if (FindCarretIndex != textBox.CaretIndex + text.Length)
                FindCarretIndex = textBox.CaretIndex;
            if (FindPrevRunnedBefore == true)
            {
                FindCarretIndex += text.Length;
                FindPrevRunnedBefore = false;
            }
            int index=textBox.Text.IndexOf(text,FindCarretIndex);
            if (index == -1)
            {
                MessageBox.Show(String.Format("Cannot find \"{0}\"", text));
                return;
            }
            textBox.Select(index, text.Length);
            FindCarretIndex = index + text.Length;
            textBox.Focus();
           
        }
        private void findPrev(string text, bool? cs)
        {

            int index = textBox.Text.LastIndexOf(text, textBox.CaretIndex);
            if (index == -1)
            {
                MessageBox.Show(String.Format("Cannot find \"{0}\"", text));
                return;
            }
            textBox.Select(index, text.Length);
            textBox.Focus();
            FindPrevRunnedBefore = true;

        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            ReplaceDialog replaceDlg = new ReplaceDialog();
            replaceDlg.Owner = this;
            replaceDlg.FindEvent += findNext;
            replaceDlg.ReplaceEvent += replace;
            replaceDlg.ReplaceAllEvent += replaceAll;
            replaceDlg.Show();
        }
        private void replace(string newValue)
        {
            if (textBox.SelectedText.Length < 2)
                return;
            textBox.SelectedText = newValue;
            textBox.CaretIndex = textBox.Text.Length;
        }
        private void replaceAll(string oldValue, string newValue, bool? cs)
        {
            textBox.Text = textBox.Text.Replace(oldValue, newValue);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("I'm your motherfucker and you're fucking bastard!\n\t\txD");
        }
    }
}