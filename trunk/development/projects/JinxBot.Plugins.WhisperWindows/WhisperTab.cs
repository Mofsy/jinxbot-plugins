using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JinxBot.Controls.Docking;
using BNSharp;
using JinxBot.Controls;

namespace JinxBot.Plugins.WhisperWindows
{
    public partial class WhisperTab : DockableDocument
    {
        private enum Direction
        {
            Recv,
            Sent
        }

        private class Message
        {
            public ChatMessageEventArgs Args;
            public Direction Direction;
        }

        private Queue<Message> outstanding = new Queue<Message>();
        private bool ready = false;
        private IJinxBotClient client;

        public WhisperTab()
        {
            InitializeComponent();
        }

        public WhisperTab(IJinxBotClient client)
            : this()
        {
            this.client = client;
        }

        public string MyUsername
        {
            get;
            set;
        }

        public string OtherPerson
        {
            get;
            set;
        }

        public void AddReceived(ChatMessageEventArgs e)
        {
            if (ready)
            {
                this.chatBox1.AddChat(
                    new ChatNode(e.Username, Color.SteelBlue),
                    new ChatNode(": ", Color.SteelBlue),
                    new ChatNode(e.Text, Color.LightSteelBlue)
                    );
            }
            else
            {
                outstanding.Enqueue(new Message { Args = e, Direction = Direction.Recv });
            }
        }

        public void AddSent(ChatMessageEventArgs e)
        {
            if (ready)
            {
                this.chatBox1.AddChat(
                    new ChatNode(MyUsername + ": ", Color.Lime),
                    new ChatNode(e.Text, Color.LightSlateGray)
                    );
            }
            else
            {
                outstanding.Enqueue(new Message { Direction = Direction.Sent, Args = e });
            }
        }

        private void chatBox1_DisplayReady(object sender, EventArgs e)
        {
            ready = true;
            while (outstanding.Count > 0)
            {
                Message msg = outstanding.Dequeue();
                switch (msg.Direction)
                {
                    case Direction.Recv:
                        AddReceived(msg.Args);
                        break;
                    case Direction.Sent:
                        AddSent(msg.Args);
                        break;
                }
            }
        }

        private void chatBox1_MessageReady(object sender, MessageEventArgs e)
        {
            this.client.SendMessage(string.Concat("/w ", OtherPerson, " ", e.Message));
        }
    }
}
