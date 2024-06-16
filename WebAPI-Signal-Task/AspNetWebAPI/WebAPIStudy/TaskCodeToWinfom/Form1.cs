using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskCodeToWinfom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //主线程卡死，但是gif图还是在执行的。 最后导致看到了缺失的情况
        //    Thread.Sleep(3000);
        //    MessageBox.Show("ok");
        //}
        private async void button1_Click(object sender, EventArgs e)
        {
            //异步方式
            //test1
            //await Task.Delay(3000);
            //MessageBox.Show("ok");

            //HttpClient client = new HttpClient();

            //test2
            //创建了一个任务，这个任务没有真正的执行，只有.Result才执行，不支持直接.Result这么做 会有漏洞，导致获取空的result
            //此做法 会由主线程做处理。 同步处理
            //var task = client.GetAsync("https://github.com/");
            //等待任务执行 由主线程进行等待
            //task.Wait();
            //var html = task.Result;

            //test3
            //异步调用
            //var hrm = await client.GetAsync("https://github.com/");
            //MessageBox.Show("ok");
            

        }
    }
}
