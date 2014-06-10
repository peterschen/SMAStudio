﻿using SMAStudio.Settings;
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

namespace SMAStudio
{
    /// <summary>
    /// Interaction logic for WelcomeDialog.xaml
    /// </summary>
    public partial class WelcomeDialog : Window
    {
        public WelcomeDialog()
        {
            InitializeComponent();

            txtSMAUrl.Focus();
            txtSMAUrl.Select(txtSMAUrl.Text.Length, 0);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!txtSMAUrl.Text.StartsWith("https://"))
            {
                MessageBox.Show("Invalid URL specified for the SMA Web Service. You are required to use HTTP SSL.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SettingsManager.Current.Settings.SmaWebServiceUrl = txtSMAUrl.Text;

            DialogResult = true;
            Close();
        }
    }
}