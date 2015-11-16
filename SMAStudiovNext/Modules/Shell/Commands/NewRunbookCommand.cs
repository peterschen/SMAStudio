﻿using Caliburn.Micro;
using Gemini.Framework.Services;
using SMAStudiovNext.Models;
using SMAStudiovNext.Modules.Credential.ViewModels;
using SMAStudiovNext.Modules.EnvironmentExplorer.ViewModels;
using SMAStudiovNext.Modules.Runbook.ViewModels;
using SMAStudiovNext.Windows;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace SMAStudiovNext.Modules.Shell.Commands
{
    public class NewRunbookCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var shell = IoC.Get<IShell>();
            
            var dialog = new UIAddNewItemDialog();
            dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            if ((bool)dialog.ShowDialog())
            {
                var reader = new StreamReader(dialog.SelectedTemplate.Path);
                reader.ReadLine(); // Skip the first line since that contains the DESCRIPTION
                string runbookContent = reader.ReadToEnd();

                reader.Close();
                
                var context = IoC.Get<EnvironmentExplorerViewModel>().GetCurrentContext();
                var viewModel = default(RunbookViewModel);

                switch (context.ContextType)
                {
                    case Core.ContextType.SMA:
                        var runbook = new SMA.Runbook();
                        runbook.RunbookName = dialog.CreatedName;

                        viewModel = new RunbookViewModel(new RunbookModelProxy(runbook, context));
                        viewModel.AddSnippet(runbookContent);
                        break;
                    case Core.ContextType.Azure:
                        var azureRunbook = new Vendor.Azure.Runbook();
                        azureRunbook.RunbookName = dialog.CreatedName;

                        viewModel = new RunbookViewModel(new RunbookModelProxy(azureRunbook, context));
                        viewModel.AddSnippet(runbookContent);
                        break;
                }

                if (viewModel != null)
                    shell.OpenDocument(viewModel);
            }
        }
    }
}
