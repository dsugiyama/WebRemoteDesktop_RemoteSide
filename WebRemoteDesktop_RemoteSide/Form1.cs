using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;


namespace WebRemoteDesktop_RemoteSide
{
    public partial class Form1 : Form
    {
        public delegate void setLabelDelegate(String str);
        public setLabelDelegate setTextBoxID;
        public Form1()
        {
            InitializeComponent();


            //delegateの初期化
            setTextBoxID = new setLabelDelegate(TextBox1SetText);
        }
        private void TextBox1SetText(string str)
        {
            textBox1.Text = str;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            RemoteInputDevice rid = new RemoteInputDevice();

            
            /*
            rid.leftClick(100,100);
            Thread.Sleep(1000);
            rid.rightClick(200, 200);
            Thread.Sleep(1000);
            rid.middleClick(300, 300);
            Thread.Sleep(1000);
            //Thread.Sleep(5000);
            //rid.typing();*/
            /*
            rid.setMouseCursor(100, 100);//指定された場所にマウスカーソルを移動する。

            Thread.Sleep(5000);//5秒間処理を中断する。この間にメモ帳などを開いて入力場所にカーソルを合わせるとよい。
            rid.typing("T");
            rid.typing("E");
            rid.typing("S");
            rid.typing("T");*/


            sc = new ServerConnector(this);

            sc.createWebSocket();
            sc.sendMsg(ServerConnector.MessageType.CREATE); 

        }
        ServerConnector sc;


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sc.sendMsg(ServerConnector.MessageType.DISCONNECT);
        }

      
    }
}
