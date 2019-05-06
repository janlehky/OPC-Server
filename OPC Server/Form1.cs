using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Opc.Da;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;

namespace OPC_Server
{
    public partial class Form1 : Form
    {

        // variable definition for OPC communication
        private OpcCom.Factory fact = new OpcCom.Factory();
        private Server server;

        private SubscriptionState groupState;

        private Subscription group;

        private List<Item> itemList = new List<Item>();

        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string recieve;
        private Socket listener;

        // Thread signal.  
        public ManualResetEvent allDone = new ManualResetEvent(false);

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Form1()
        {
            InitializeComponent();

            string localIP;
            using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                serverSocket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = serverSocket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            ipAddress_form.Text = localIP;
            port_form.Text = "13000";
            opcServerAddress.Text = "opcda://localhost/RSLinx OPC Server";

            // open socket for communication
            serverWorker.RunWorkerAsync();

            // connect to OPC server
            ConnectToOpcServer();
        }

        private void ConnectToOpcServer()
        {
            try
            {
                // define communication to server
                this.server = new Server(fact, null);
                this.server.Url = new Opc.URL(opcServerAddress.Text);

                // open communication to server
                this.server.Connect();
                
                // create subscription group with set refresh rate
                groupState = new SubscriptionState();
                groupState.Name = "Group";
                groupState.UpdateRate = 1000;
                groupState.Active = true;
                this.group = (Opc.Da.Subscription)server.CreateSubscription(groupState);
                log.Info("Connected to OPC server");
            }
            catch (Exception x)
            {
                log.Fatal(x.Message);
            }
        }

        private void writeValue(Int32 value, string tagName)
        {
            try
            {
                List<Item> list = this.group.Items.ToList();
                int index = list.FindIndex(x => x.ItemName == tagName); // x.ItemName.StartsWith(tagName)

                if (index == -1)
                {
                    // add PLC item to listv
                    Item[] items = new Item[1];
                    items[0] = new Item();
                    items[0].ItemName = tagName;
                    items = this.group.AddItems(items);
                }

                stsLabel.Text = this.group.Items.Length.ToString();
                // write items
                ItemValue[] writeValues = new ItemValue[1];
                writeValues[0] = new ItemValue();
                if (index == -1)
                {
                    writeValues[0].ServerHandle = this.group.Items.Last().ServerHandle;
                }
                else
                {
                    writeValues[0].ServerHandle = this.group.Items[index].ServerHandle;
                }
                writeValues[0].Value = value;
                Opc.IRequest req;
                this.group.Write(writeValues, 321, new WriteCompleteEventHandler(WriteCompleteCallback), out req);
            }
            catch (Exception x)
            {
                log.Fatal(x.Message);
            }
        }

        private void writeValue_Float(float value, string tagName)
        {
            try
            {
                List<Item> list = this.group.Items.ToList();
                int index = list.FindIndex(x => x.ItemName == tagName); // x.ItemName.StartsWith(tagName)

                if (index == -1)
                {
                    // add PLC item to listv
                    Item[] items = new Item[1];
                    items[0] = new Item();
                    items[0].ItemName = tagName;
                    items = this.group.AddItems(items);
                }

                stsLabel.Text = this.group.Items.Length.ToString();
                // write items
                ItemValue[] writeValues = new ItemValue[1];
                writeValues[0] = new ItemValue();
                if (index == -1)
                {
                    writeValues[0].ServerHandle = this.group.Items.Last().ServerHandle;
                }
                else
                {
                    writeValues[0].ServerHandle = this.group.Items[index].ServerHandle;
                }
                writeValues[0].Value = value;
                Opc.IRequest req;
                this.group.Write(writeValues, 321, new WriteCompleteEventHandler(WriteCompleteCallback), out req);
            }
            catch (Exception x)
            {
                log.Fatal(x.Message);
            }
        }

        private void WriteCompleteCallback(object clientHandle, Opc.IdentifiedResult[] results)
        {
            Console.WriteLine("Write completed");
            foreach (Opc.IdentifiedResult writeResult in results)
            {
                // Console.WriteLine("\t{0} write result: {1}", writeResult.ItemName, writeResult.ResultID);
                log.Info(writeResult.ResultID.ToString());
            }
            Console.WriteLine();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.server.IsConnected)
                {
                    this.server.Disconnect();
                }

                ConnectToOpcServer();
            }
            catch (Exception x)
            {
                log.Fatal(x.Message);
            }
        }

        private void btnServerStart_Click(object sender, EventArgs e)
        {
            if (serverWorker.IsBusy)
            {
                serverWorker.CancelAsync();
                serverWorker.RunWorkerAsync();
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            try
            {
                // Read data from the client socket.   
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    content = Encoding.ASCII.GetString(state.buffer, 0, bytesRead).ToString();
                    string[] data = content.Split(new[] { " " }, StringSplitOptions.None);

                    log.Info(content);

                    if (data[0] == "w")
                    {
                        Int32 int_value;
                        if (Int32.TryParse(data[2], out int_value)){
                            writeValue(int_value, data[1]);
                        }
                        else
                        {
                            writeValue_Float(float.Parse(data[2]), data[1]);
                        }
                    }

                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
            catch(Exception x)
            {
                log.Fatal(x.Message);
            }
        }

        // State object for reading client data asynchronously  
        public class StateObject
        {
            // Client  socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 1024;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Int32 port = Int32.Parse(port_form.Text);
            
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = IPAddress.Parse(ipAddress_form.Text);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            SocketPermission permission = new SocketPermission(NetworkAccess.Accept,
                   TransportType.Tcp, "", SocketPermission.AllPorts);
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            log.Info("Socket worker started");

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception x)
            {
                log.Fatal(x.Message);
            }
        }
    }
}
