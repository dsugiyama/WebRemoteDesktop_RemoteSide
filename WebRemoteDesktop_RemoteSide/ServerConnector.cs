using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using WebSocket4Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Reflection;
 

namespace WebRemoteDesktop_RemoteSide
{
    class ServerConnector
    {

        string hostID;
        public void sendMsgPack()
        {
            
       
            /*
            var targetObject = new GetHostID
            {
                type = "create-hostid",
            };*/
            /*

            var stream = new MemoryStream();

            var serializer = MessagePackSerializer.Get<RemoteMock>();
            var deserializer = MessagePackSerializer.Get<GetHostID>();
            serializer.Pack(stream, targetObject);
            stream.Position = 0;
            //var deserializedObject = serializer.Unpack(stream);
                  
            var ws = new WebSocket("ws://40.74.115.93:8080");
            ws.DataReceived += ws_DataReceived;

            /// サーバ接続開始
            ws.Open();

            var str = targetObject;
            /// 送受信ループ
            while (true)
            {
                //var str = Console.ReadLine();
                //if (str == "END") break;

                if (ws.State == WebSocketState.Open)
                {
                    byte[] sendByte = stream.ToArray();
                    ws.Send(sendByte,0,sendByte.Length);
                  

                    
                   // deserializer = serializer.Unpack();
                    Thread.Sleep(100);
                   
                    
                }
                else
                {
                    Console.WriteLine("{0}:wait...", DateTime.Now.ToString());
                }
            }

            /// ソケットを閉じる
            ws.Close();*/
        }

        Form1 paretnForm;
        public ServerConnector(Form1 f1)
        {
            paretnForm = f1;
        }

        WebSocket ws;
        public void createWebSocket()
        {
            //WebSocketを作成し、DataReveivedのイベントハンドラを取り付ける。
            ws = new WebSocket("ws://rcpc00.japanwest.cloudapp.azure.com:8080");//
            ws.MessageReceived += ws_MessageReceived;
        }

        System.Diagnostics.Process ffmpegProcess = new System.Diagnostics.Process();//動画プロセス
        void ws_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            JObject json = JObject.Parse(e.Message);
            string type = json.GetValue("type").ToString();
            //{ type: ‘mouse-click’ (string), x: (int), y: (int) } //"disconnect-guest"
            switch (type)
            {
                case "create-hostid":
                    type_CreateHostID(json);
                    break;
                
                case "connect-guest"://意図　これを受信したら動画の転送を開始する。
                    //起動パスを求める
                    Assembly myAssembly = Assembly.GetEntryAssembly();
                    string startPath = myAssembly.Location.Substring(0, myAssembly.Location.LastIndexOf("\\") + 1);
                    //ffmpegを立ち上げる
                    string ffmplegPath = startPath + "ffmpeg-20160119-git-cc83177-win64-static\\bin\\";
                    //Processオブジェクトを作成する
                    ffmpegProcess = new System.Diagnostics.Process();
                    //起動する実行ファイルのパスを設定する
                    ffmpegProcess.StartInfo.FileName = ffmplegPath + "ffmpeg.exe";
                    ffmpegProcess.StartInfo.Arguments = "-f gdigrab -draw_mouse 1 -show_region 1 -framerate 30 -video_size 1366x768 -i desktop -f mpeg1video -b 1800k http://rcpc00.japanwest.cloudapp.azure.com:8082/" + hostID + "/";
                    //起動する。プロセスが起動した時はTrueを返す。
                    bool result = ffmpegProcess.Start();
                    break;

                case "disconnect-guest":
                    ffmpegProcess.Kill();
                    break;

                case "mouse-click":
                    type_mouseClick(json);                    
                    break;

                case "mouse-down":
                    type_mouseDown(json);
                    break;

                case "mouse-up":
                    type_mouseUp(json);
                    break;


                case "mouse-move":
                    type_mouseMove(json);
                    break;

                case "key-down":
                    //{ type: ‘key-down’, key: ‘A’ or ‘B’ or … (string) }
                    type_keyDown(json);
                    break;

                //case



                default:
                    break;
            }


           // throw new NotImplementedException();
        }

        private void type_CreateHostID(JObject json)
        {
            hostID = json.GetValue("hostid").ToString();
            paretnForm.Invoke(paretnForm.setTextBoxID, hostID);
        }

        private void type_mouseClick(JObject json)
        {
            RemoteInputDevice rid = new RemoteInputDevice();

            string mouseButton = json.GetValue("button").ToString();
            switch (mouseButton)
            {
                case "right":
                    rid.rightClick(int.Parse(json.GetValue("x").ToString()), int.Parse(json.GetValue("y").ToString()));
                    break;

                case "left":
                    rid.leftClick(int.Parse(json.GetValue("x").ToString()), int.Parse(json.GetValue("y").ToString()));

                    break;

                default:
                    break;
            }            
        }


        private void type_mouseDown(JObject json)
        {
            RemoteInputDevice rid = new RemoteInputDevice();

            string mouseButton = json.GetValue("button").ToString();
            switch (mouseButton)
            {
                case "right":
                    rid.rightDown(int.Parse(json.GetValue("x").ToString()), int.Parse(json.GetValue("y").ToString()));
                    break;

                case "left":
                    rid.leftDown(int.Parse(json.GetValue("x").ToString()), int.Parse(json.GetValue("y").ToString()));

                    break;

                default:
                    break;
            }
        }

        private void type_mouseUp(JObject json)
        {
            RemoteInputDevice rid = new RemoteInputDevice();

            string mouseButton = json.GetValue("button").ToString();
            switch (mouseButton)
            {
                case "right":
                    rid.rightUp(int.Parse(json.GetValue("x").ToString()), int.Parse(json.GetValue("y").ToString()));
                    break;

                case "left":
                    rid.leftUp(int.Parse(json.GetValue("x").ToString()), int.Parse(json.GetValue("y").ToString()));

                    break;

                default:
                    break;
            }
        }

        private void type_mouseMove(JObject json)
        {
            RemoteInputDevice rid = new RemoteInputDevice();
            rid.setMouseCursor(int.Parse(json.GetValue("x").ToString()), int.Parse(json.GetValue("y").ToString()));
        }


        private void type_keyDown(JObject json)
        {
            RemoteInputDevice rid = new RemoteInputDevice();
            rid.typing(json.GetValue("key").ToString());
            
           /* //バグあり
            //Modifiedキーがあるかどうかで分ける。
            bool modFlagShift =(bool)json.GetValue("shift");
            //bool modFlagShift = (bool)json.GetValue("ctrl");
            //bool modFlagShift = (bool)json.GetValue("alt");
            RemoteInputDevice rid = new RemoteInputDevice();
            if (modFlagShift == false)
            {               
                rid.typing(json.GetValue("key").ToString());
            }
            else
            {
                rid.typingModifireKey(json.GetValue("key").ToString(), "");
            }*/
        }

     



        public void sendMsg(MessageType mt)
        {
            //処理によって送るJsonを変える
            string json = "";
            if (mt == MessageType.CREATE)
            {
                json = "{\"type\":\"connect-host\",\"screenWidth\":1366,\"screenHeight\":768}\n";
            }
            else if (mt == MessageType.DISCONNECT)
            {
                json = "{\"type\":\"disconnect-host\",\"hostid\":"+hostID+"}\n";
            }

            //string json = "{\"type\":\"connect-host\",\"screenWidth\":600,\"screenHeight\":600}\n";
            //{ type: ‘disconnect-host’, hostid: (string) }
            //"{\"type\":\"disconnect-host\",\"
            if (ws.State != WebSocketState.Open)
            {
                ws.Open();
            }
            while (ws.State != WebSocketState.Open)//受信されるまで待機
            {
                
            }

            if (ws.State == WebSocketState.Open)
            {
                ws.Send(json);

                //Thread.Sleep(100);

            }

           // ws.Close();
        }
        public void disconectSerer()
        {
            string json = "{\"type\":\"disconnect-host\",\"hostid\":" + hostID + "}\n";            
        }

        public enum MessageType
        {
            CREATE,DISCONNECT
        };





            



    }

}
