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
namespace Consumable
{
    public partial class Form1 : Form
    {
        string filePath;
        string[] lines;
        string[] GUI_x = new string[7];

        //리스트로 상수화 후 뽑아서 쓸 수 있도록 형식을 만든거지만 사용성 떨어짐.
        string[] IO = { "\tAI", "\tMELSEC1", "\t-", "\t-", "\t.01f", "\t24", "\t2", "\t-", "\tpoll", "\t0", "\t9999999", "\t-", "\t-" };
        
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    lines = File.ReadAllLines(filePath);
                }
            }
            
            if(filePath == null)
            {
                MessageBox.Show("파일 경로 지정이 안되어있음.");
            }
            else
            {
                if (lines.Length == 0)
                {
                    MessageBox.Show("빈 파일입니다.");

                }
                else
                {
                    string[] header = new string[lines.Length];
                    string[] IO_name = new string[lines.Length];
                    string[] IO_addr = new string[lines.Length];
                    string[] IO_addr_set = new string[lines.Length];
                    string[] IO_addr_reset = new string[lines.Length];
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf(".") != -1)
                        {
                                header[i] = lines[i].Substring(0, lines[i].IndexOf("."));
                                IO_name[i] = lines[i].Split('\t')[0];
                                IO_addr[i] = lines[i].Split('\t')[1];
                                IO_addr_set[i] = lines[i].Split('\t')[2];
                                IO_addr_reset[i] = lines[i].Split('\t')[3];
                        }
                        else
                        {
                            IO_name[i] = "null";
                        }
                    }
                    //IO
                    string[] IO_list = new string[IO_name.Length];
                    string[] SET_list = new string[IO_name.Length];
                    string[] RESET_list = new string[IO_name.Length];
                    string[] Pro_list = new string[IO_name.Length];
                    string[] Date_list = new string[IO_name.Length];
                    string Melsec = "";
                    
                 
                    // GUI
                    string[] text_name = new string[IO_name.Length];
                    string[] text_time = new string[IO_name.Length];
                    string[] text_SET = new string[IO_name.Length];
                    string[] text_IO = new string[IO_name.Length];
                    string[] text_Pro = new string[IO_name.Length];
                    string[] text_DATE = new string[IO_name.Length];
                    string[] text_SEQ = new string[IO_name.Length];
                    int start_y = 489;
                    //Sequence
                    string[] seq_name = new string[IO_name.Length];
                    string[] seq_Pro = new string[IO_name.Length];
                    string[] seq_SET = new string[IO_name.Length];
                    int cnt = 1;
                    for (int i = 0; i < IO_name.Length; i++)
                    {
                        // IO_name : 인덱스 값으로 해당 주소값을 매핑하여 사용.
                        //IO_name 리스트에서 분류된 IO Name을 이용하여 IO, SET, RESET, Progress, DATE 등으로 분리하여 리스트 담는 부분.
                        if (IO_name[i] != "null")
                        {
                            if (header[i] == "Prm3")
                            {
                                Melsec = "\tMELSEC3";
                            }
                            if (header[i] == "Prm4")
                            {
                                Melsec = "\tMELSEC4";
                            }
                            if (header[i] == "Prm5")
                            {
                                Melsec = "\tMELSEC5";
                            }
                            if (header[i] == "Prm6")
                            {
                                Melsec = "\tMELSEC6";
                            }
                            IO_list[i] = IO_name[i] + "\t\t" + IO[0] + IO[1] + "\t" + IO[2] + IO[3] + IO[4] + IO[5] + "\t0x" + IO_addr[i] + IO[6] + IO[7] + IO[8] + IO[9] + IO[10] + IO[11] + IO[12];
                            SET_list[i] = IO_name[i] + "Set" + "\t\tAO" + "\tMELSEC1" + "\t\t-\t-\t.f\t24\t0x" + IO_addr_set[i] + "\t2\t-\tpoll\t0\t9999999\t-\t-";
                            RESET_list[i] = IO_name[i] + "Reset" + "\t\tDO" + Melsec + "\t\teOFFON\t-\td\t4\t" + IO_addr_reset[i] + "\t1\t-\tpoll";
                            Pro_list[i] = IO_name[i] + "Progress" + "\t\tAO\t-\t\t-\t%\t.1f\t24\t-\t1\t-\tpoll\t0\t100\t-\t-";
                            Date_list[i] = IO_name[i] + "_DATE\t\tSIO\t\"-\"";
                            text_name[i] = "-\t\t\t\t\t-\t\tSTATIC_B_L_F20( " + IO_name[i].Split('_')[1].ToString() + " )\t\t\"\"\t\t" + GUI_x[0] + "\t" + start_y.ToString() + "\t340\t26\t-\t\t\"\"\tFALSE";
                            text_time[i] = "-\t\t\t\t\t-\t\tSTATIC_B_L_F18(TIMES)\t\t\t\"\"\t\t" + GUI_x[1] + "\t" + start_y.ToString() + "\t80\t26\t-\t\t\"\"\tFALSE";
                            text_SET[i] = IO_name[i] + "Set" + "\t\tP_AO\t\tTEXT_B_F18(12)\t\t\t\t\"\"\t\t" + GUI_x[2] + "\t" + start_y.ToString() + "\t67\t26\tIO_POPUP()\t\"\"\tFALSE";
                            text_IO[i] = IO_name[i] + "\t\t\tAIO\t\tTEXT_B_F18(12)\t\t\t\t\"\"\t\t" + GUI_x[3] + "\t" + start_y.ToString() + "\t67\t26\t-\t\t\"\"\tFALSE";
                            text_Pro[i] = IO_name[i] + "Progress\t\tAIO\t\tProgressBar(#lightblue,#crimson,#black)\t\"\"\t\t" + GUI_x[4] + "\t" + start_y.ToString() + "\t143\t26\t-\t\t\"\"\tFALSE";
                            text_DATE[i] = IO_name[i] + "_DATE\t\tSIO\t\tTEXT_B_F18(12)\t\t\t\t\"\"\t\t" + GUI_x[5] + "\t" + start_y.ToString() + "\t78\t26\t-\t\t\"\"\tFALSE";
                            text_SEQ[i] = "fnSignalTower\t\t\t\tSEQ\t\tImgExeBtn_2(Reset, 138)\t\t\t\"\"\t\t" + GUI_x[6] + "\t" + start_y.ToString() + "\t67\t28\tSEQ_EVENT_N( " + IO_name[i] + "\tRESET\t" + IO_name[i] + "Reset )\t\t\"\"\tFALSE";
                            seq_name[i] = "CIAO\t\t" + IO_name[i] + "\t\t\t( _TEXT(\"" + IO_name[i] + "\")\t);";
                            seq_Pro[i] = "CIAO\t\t" + IO_name[i] + "Progress\t\t( _TEXT(\"" + IO_name[i] + "Progress\")\t);";
                            seq_SET[i] = "CIAO\t\t" + IO_name[i] + "Set\t\t( _TEXT(\"" + IO_name[i] + "Set\")\t);";
                            if(cnt == 1)
                            {
                                start_y += 29;
                                cnt = 0;
                            }
                            else
                            {
                                start_y += 28;
                                cnt = 1;
                            }
                        }
                        else
                        {
                            IO_list[i] = "";
                            SET_list[i] = "";
                            RESET_list[i] = "";
                            Pro_list[i] = "";
                            Date_list[i] = "";
                            text_name[i] = "";
                            text_time[i] = "";
                            text_SET[i] = "";
                            text_IO[i] = "";
                            text_Pro[i] = "";
                            text_DATE[i] = "";
                            text_SEQ[i] = "";
                            seq_name[i] = "";
                            seq_Pro[i] = "";
                            seq_SET[i] = "";
                        }
                    }

                    filePath = null;
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.InitialDirectory = "c:\\";
                        openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                        openFileDialog.FilterIndex = 2;
                        openFileDialog.RestoreDirectory = true;
                        
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            //Get the path of specified file
                            filePath = openFileDialog.FileName;
                        }
                    }
                    if (filePath == null) {
                        MessageBox.Show("쓰기 파일 미선택");
                    }
                    else
                    {
                        StreamWriter sw = new StreamWriter(filePath, false);

                        for (int i = 0; i < IO_list.Length; i++)
                        {
                            sw.WriteLine(IO_list[i].ToString());
                        }
                        for (int i = 0; i < SET_list.Length; i++)
                        {
                            sw.WriteLine(SET_list[i].ToString());
                        }
                        for (int i = 0; i < RESET_list.Length; i++)
                        {
                            sw.WriteLine(RESET_list[i].ToString());
                        }
                        for (int i = 0; i < Pro_list.Length; i++)
                        {
                            sw.WriteLine(Pro_list[i].ToString());
                        }
                        for (int i = 0; i < Date_list.Length; i++)
                        {
                            sw.WriteLine(Date_list[i].ToString());
                        }
                        for(int i = 0; i < IO_name.Length; i++)
                        {
                            sw.WriteLine(text_name[i]);
                            sw.WriteLine(text_time[i]);
                            sw.WriteLine(text_SET[i]);
                            sw.WriteLine(text_IO[i]);
                            sw.WriteLine(text_Pro[i]);
                            sw.WriteLine(text_DATE[i]);
                            sw.WriteLine(text_SEQ[i]);
                            sw.WriteLine("//");
                            sw.WriteLine("//");
                        }
                        for(int i = 0; i < IO_name.Length; i++)
                        {
                            sw.WriteLine(seq_name[i]);
                            sw.WriteLine(seq_Pro[i]);
                            sw.WriteLine(seq_SET[i]);
                        }
                        
                        sw.Close();
                        MessageBox.Show("쓰기 완료");
                    }
                }
            }

            
        }

        private void R_CheckedChanged(object sender, EventArgs e)
        {
            if (R.Checked)
            {
                GUI_x[0] = "922";
                GUI_x[1] = "1280";
                GUI_x[2] = "1342";
                GUI_x[3] = "1410";
                GUI_x[4] = "1479";
                GUI_x[5] = "1633";
                GUI_x[6] = "1709";
            }
        }

        private void L_CheckedChanged(object sender, EventArgs e)
        {
            if (L.Checked)
            {
                GUI_x[0] = "54";
                GUI_x[1] = "411";
                GUI_x[2] = "473";
                GUI_x[3] = "542";
                GUI_x[4] = "611";
                GUI_x[5] = "765";
                GUI_x[6] = "840";
            }
        }
    }
}
