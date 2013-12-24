using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Globalization;
using CSP.RSA.Cipher;
using System.ServiceProcess;
using System.Resources;
namespace PipeServer
{
    class Program : IDisposable
    {
        
        public void Dispose()
        {
            Dispose(true);
        }

        ~Program()
        {
            Dispose(false);
        }


        protected virtual void Dispose(bool dispossing)
        {
            if (dispossing)
            {
                GC.SuppressFinalize(this);
            }
        
        }
        static StreamReader sr;
        static StreamWriter sw;
    
        static List<string> keys;

        static string sLogFormat, sErrorTime;



        static bool running;
        static Thread runningThread;
        static EventWaitHandle terminateHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        public string PipeName { get; set; }

        static void ServerLoop()
        {

            while (running)
            {
                ProcessNextClient();
            }

            terminateHandle.Set();
        }

        public static  void Run()
        {
            TraceLog("Service have start");
           
            running = true;
           
            runningThread = new Thread(ServerLoop);
            runningThread.IsBackground = true;
            runningThread.Start();
        }

        public static void Stop()
        {
            TraceLog("Service have end");
            running = false;
            
        }

        public  virtual string ProcessRequest(string message)
        {
            return "";
        }

        public static void ProcessClientThread(object o)
        {
            NamedPipeServerStream pipeStream = (NamedPipeServerStream)o;
            PrivatePublicKey pp;
            pp = new PrivatePublicKey();
            sr = new StreamReader(pipeStream);
            sw = new StreamWriter(pipeStream);
             RSAChiper rsa;
                try
                {
                   
                    keys = pp.CreateKeyPairs();
                    rsa = new RSAChiper(keys[0]);
                    string test;
                    sw.WriteLine("Waiting");
                    sw.Flush();
                    
                    test = sr.ReadLine();
                    if (test == "Connected")
                    {
                        TraceLog("Client " + Thread.CurrentThread.ManagedThreadId + " connected");
                        
                        sw.WriteLine(keys[1]);
                        sw.Flush();
                        pipeStream.WaitForPipeDrain();

                        test = sr.ReadLine();
                        if (test != null)
                        {
                            TraceLog("Chiper Data: " + test);
                            TraceLog("Plain Text: " + rsa.RSADecrypt(test));
                        }
                    }



                }

                catch (Exception ex)
                {
                    TraceLog(ex.Message);
                    
                }

                finally
                {

                    if (pipeStream.IsConnected) { pipeStream.Disconnect();

                    TraceLog("Client " + Thread.CurrentThread.ManagedThreadId + " disconnected");
                      
                    }
                }
            

        }

        public static void ProcessNextClient()
        {
         
            try
            {
                
                NamedPipeServerStream pipeStream = new NamedPipeServerStream("testpipe", PipeDirection.InOut, 254);
                pipeStream.WaitForConnection();
                Thread t = new Thread(ProcessClientThread);
                t.Start(pipeStream);
            }
            catch (Exception)
            {
            
            }
        }
        static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
                using (var service = new PipeService())
                    ServiceBase.Run(service);
            else
            {
                Run();
            }
        }

        public static void TraceLog(string pTraceLog)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (!Directory.Exists(path+"/Trace"))
            {
                Directory.CreateDirectory(path + "/Trace");
            }
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = "_" + sYear + "_" + sMonth + "_" + sDay;

            sLogFormat = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.ffff") + " | ";

            StreamWriter sw = new StreamWriter(path+"/Trace /Trace" + sErrorTime + ".log", true);
            sw.WriteLine(sLogFormat + pTraceLog);
            sw.Flush();
            sw.Close();
        }


    }
    }

