using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ClassLibrary1;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private V5MainCollection MC = new V5MainCollection();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MC;
        }

        private void Add_FromFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if ((bool)dlg.ShowDialog())
                    MC.AddFromFile(dlg.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add From File error: " + ex.Message);
            }
            finally
            {
                ErrorMsg();
            }
        }

        private void AddDefault_V5DataCollection_Click(object sender, RoutedEventArgs e)
        {
            MC.AddDefaultDataCollection();
            ErrorMsg();
        }

        private void AddDefaults_Click(object sender, RoutedEventArgs e)
        {
            MC.AddDefaults();
            DataContext = MC;
            ErrorMsg();
        }

        private void AddDefault_V5DataOnGrid_Click(object sender, RoutedEventArgs e)
        {
            MC.AddDefaultDataOnGrid();
            ErrorMsg();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (MC.IsChanged)
            {
                UnsavedChanges();
            } 
            MC = new V5MainCollection();
            DataContext = MC;
            ErrorMsg();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MC.IsChanged)
                {
                    UnsavedChanges();
                }
                Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
                if ((bool)fd.ShowDialog())
                {
                    MC = new V5MainCollection();
                    MC.Load(fd.FileName);
                    DataContext = MC;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loading Error: " + ex.Message);
            }
            finally
            {
                ErrorMsg();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                if ((bool)dialog.ShowDialog())
                    MC.Save(dialog.FileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Saving Error: " + ex.Message);
            }
            finally
            {
                ErrorMsg();
            }
        }

        private bool UnsavedChanges()
        {
            MessageBoxResult msg = MessageBox.Show("Save Changes?", "Save", MessageBoxButton.YesNoCancel);
            if (msg == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                if ((bool)dialog.ShowDialog())
                    MC.Save(dialog.FileName);
            }
            else if (msg == MessageBoxResult.Cancel)
            {
                return true;
            }
            return false;
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MC.IsChanged)
            {
                e.Cancel = UnsavedChanges();
            }
            ErrorMsg();
        }

        public void ErrorMsg()
        {
            if (MC.ErrorMessage != null)
            {
                MessageBox.Show(MC.ErrorMessage, "Error");
                MC.ErrorMessage = null;
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var selectedLB = LB_Main.SelectedItems;
            List<V5Data> Items = new List<V5Data>();
            Items.AddRange(selectedLB.Cast<V5Data>());
            foreach (V5Data item in Items)
            {
                MC.Remove(item.InfoData, item.Time);
            }
            ErrorMsg();
        }

        private void FilterDataCollection(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V5DataCollection)) args.Accepted = true;
                else args.Accepted = false;
            }
        }

        private void FilterDataOnGrid(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V5DataOnGrid)) args.Accepted = true;
                else args.Accepted = false;
            }
        }
    }
}
