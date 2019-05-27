﻿using AutumnBox.Basic.ManagedAdb;
using AutumnBox.GUI.MVVM;
using AutumnBox.GUI.Util.Bus;
using AutumnBox.GUI.Util.Net;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace AutumnBox.GUI.ViewModel
{
    class VMMainMenu : ViewModelBase
    {
        public ICommand Exit { get; }
        public ICommand OpenLoggingWindow { get; }
        public ICommand OpenUpdateLogs { get; }
        public ICommand OpenSettings { get; }
        public ICommand OpenShell { get; }
        public ICommand UpdateCheck { get; }
        public ICommand OpenOSInformation { get; }
        public VMMainMenu()
        {
            Exit = new MVVMCommand(p => { App.Current.Shutdown(0); });
            OpenLoggingWindow = new MVVMCommand(p => WinM.X("Log"));
            OpenUpdateLogs = new MVVMCommand(p => WinM.X("UpdateLogs"));
            OpenSettings = new MVVMCommand(p => WinM.X("Settings"));
            UpdateCheck = new MVVMCommand(P => Updater.Do());
            OpenOSInformation = new MVVMCommand(p => WinM.X("OpenSource"));
            OpenShell = new MVVMCommand(p => OpenShellMethod(p?.ToString()));
        }
        private static void OpenShellMethod(string fileName)
        {
            ProcessStartInfo info = new ProcessStartInfo
            {
                WorkingDirectory = Adb.AdbToolsDir.FullName,
                FileName = fileName ?? "cmd.exe",
                UseShellExecute = false,
                Verb = "runas",
            };
            info.EnvironmentVariables["ANDROID_ADB_SERVER_PORT"] = Adb.Server.Port.ToString();
            if (Properties.Settings.Default.EnvVarCmdWindow)
            {
                var pathEnv = info.EnvironmentVariables["path"];
                info.EnvironmentVariables["path"] = $"{Adb.AdbToolsDir.FullName};" + pathEnv;
            }
            if (Properties.Settings.Default.StartCmdAtDesktop)
            {
                info.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            Process.Start(info);
        }
    }
}
