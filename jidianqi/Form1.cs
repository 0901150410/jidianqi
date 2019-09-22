using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace jidianqi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //我们在form载入时，扫描识别存在的串口。
            Scan_serialport(serialPort, comb_port);  //调用扫描串口函数
            serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);//必须手动添加事件处理程序
            comb_baud.Text = "115200";
            button11.BackColor = Color.Red;
            button11.Text = "串口已关闭";
        }

        //串口接收函数，需要定义数据接收的格式
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                    button1.Text = "打开串口";
                    button4.Enabled = true;
                    button11.BackColor = Color.Red;
                    button11.Text = "串口已关闭";
                }
                catch
                {
                    MessageBox.Show("关闭串口失败","错误");
                }
            }
            else
            {
                try
                {
                    serialPort.PortName = comb_port.Text;
                    serialPort.BaudRate = Convert.ToInt32(comb_baud.Text, 10);//设置波特率
                    serialPort.Open();
                    button1.Text = "关闭串口";
                    button4.Enabled = false;
                    button11.BackColor = Color.Green;
                    button11.Text = "串口已打开";
                }
                catch
                {
                    MessageBox.Show("打开串口失败", "错误");
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Scan_serialport(serialPort, comb_port);  //调用扫描串口函数
        }

        //实现串口扫描函数
        private void Scan_serialport(SerialPort myport,ComboBox myport_combobox)
        {
            string buffer;
            string[] com_string = new string[15];
            int count = 0;
            myport_combobox.Items.Clear();
            for (int i = 1; i < 15; i++)
            {
                try
                {
                    buffer = "COM" + i.ToString();
                    myport.PortName = buffer;
                    myport.Open();
                    com_string[count] = buffer;
                    myport_combobox.Items.Add(buffer);
                    myport.Close();
                }
                catch{}
            }
            myport_combobox.Text = com_string[0];
        }

        //实现数据（控制指令）的发送函数
        //参数列表：数据头，地址，功能码，四字节数据，校验和
        private void write_command_to_serialport(SerialPort myport,byte data)
        {
            byte[] command = new byte[8];//需重新定义命令的格式
            if(myport.IsOpen)
            {
                try
                {
                    myport.Write(command, 0, 8);
                }
                catch
                {
                    MessageBox.Show("命令发送失败，请检查", "错误");
                }
            }
        }
    }
}
