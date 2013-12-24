using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace PipeServer
{
    partial class PipeService : ServiceBase
    {
        public PipeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Program.Run();
        }

        protected override void OnStop()
        {
            Program.Stop();
            base.OnStop();

        }
    }
}
