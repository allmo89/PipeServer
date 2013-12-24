using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace PipeServer
{
    [RunInstaller(true)]
    public partial class PipeInstaller : System.Configuration.Install.Installer
    {
        public PipeInstaller()
        {
            InitializeComponent();
            ServiceProcessInstaller processinstaller = new ServiceProcessInstaller();
            ServiceInstaller installer = new ServiceInstaller();
            processinstaller.Account = ServiceAccount.LocalSystem;
            processinstaller.Username = null;
            processinstaller.Password = null;


            installer.DisplayName = "C# PipeServer";
            installer.StartType = ServiceStartMode.Automatic;
            

            installer.ServiceName = "PipeService";

            this.Installers.Add(processinstaller);
            this.Installers.Add(installer);
        }
    }
}
