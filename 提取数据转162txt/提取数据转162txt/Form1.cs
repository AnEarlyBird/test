using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 提取数据转162txt
{
    public partial class Form1 : Form
    {
        public string FILE_NAME = "1000Hz";


        //public List<int> RevBuf;
        public int DataLen=27000;//在数据长度DataLen中寻找第一导。
        public Form1()
        {
            InitializeComponent();
            CheckIndex();
            for (int i = 1; i <=90;i++ )
                SaveChannelN(i);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void CircAdd(ref byte b)
        {
            if (b == 255)
                b= 0;
            else 
                b++;
        }
        public void CheckIndex()
        {
            
            if (File.Exists(FILE_NAME))
            {
                FileInfo fi = new FileInfo(FILE_NAME);
                long len = fi.Length;
                long lenindex = len;
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open);
                BinaryReader r = new BinaryReader(fs);
                //Byte bb[294];
                Byte[] bb0=r.ReadBytes(294);
                byte index=bb0[19];
                int sampi;

                while (lenindex > 294)//找到文件开头
                {
                    Byte[] bb=r.ReadBytes(294);
                    lenindex = lenindex - 294;
                    CircAdd(ref index);
                    if (bb[19] != index)
                    {
                        MessageBox.Show(index - 1 + "帧处不连续" + (len-lenindex)+" "+bb[19]);
                        index = bb[19];
                    }
                    

                        
                }
                 MessageBox.Show("帧连续");

                //    for (int i = 0; i < 270000 - sampi; i++)
                //    {
                //        byte bb1 = r.ReadByte();
                //        byte bb2 = r.ReadByte();
                //        byte bb3 = r.ReadByte();
                //        string FILE_NAME2; FILE_NAME2 = "DATA//" + i%90 + "dao.txt";
                //        FileStream fs2 = new FileStream(FILE_NAME2, FileMode.Append);
                //        StreamWriter w = new StreamWriter(fs2);
                //        //RevBuf.Add(bb1 + bb2 + bb3);
                //        w.Write(bb1 + bb2 + bb3);
                //        if (i < 270000 - sampi - 1)
                //            w.Write(" ");
                //        w.Close();
                //        fs2.Close();
                //}
                r.Close();
                fs.Close();
            }
        }

        public void SaveChannelN(int chn)//参数chn为包含状态字节的导数，以此类推，第一导脑电为chn=2
        {

            if (File.Exists(FILE_NAME))
            {
                FileInfo fi = new FileInfo(FILE_NAME);
                long len = fi.Length;
                long lenindex = len;
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open);
                BinaryReader r = new BinaryReader(fs);
                string FILE_NAME2; FILE_NAME2 = "DATA//" +chn+ ".dat";
                FileStream fs2 = new FileStream(FILE_NAME2, FileMode.Create);
                StreamWriter w = new StreamWriter(fs2);

                while (lenindex > 294)//
                {
                    Byte[] bb = r.ReadBytes(294);
                    lenindex = lenindex - 294;
                    w.Write(bb[21+chn*3] * 65536 + bb[21+chn*3+1] * 256 + bb[21+chn*3+2]);
                    w.Write(" ");
                }
                r.Close();
                fs.Close();
            }
        }
    }
}
