using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using system_task2.Command;
using System.IO;
using System.Threading;
using system_task2.Helper;

namespace system_task2.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand FromBtn { get; set; }
        public RelayCommand ToBtn { get; set; }
        public RelayCommand CopyBtn { get; set; }
        public RelayCommand ResumeBtn { get; set; }
        public RelayCommand PauseBtn { get; set; }
        public string Encrypt { get; set; }

        string filetext = string.Empty;

        string filePathTo = string.Empty;

        string fileContentFrom = string.Empty;

        string filePathFrom = string.Empty;
        public int Length { get; set; }
        Thread thread;
        public MainViewModel(MainWindow mainWindow)
        {
            FromBtn = new RelayCommand((sender) =>
            {
                using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;



                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePathFrom = openFileDialog.FileName;



                        var fileStream = openFileDialog.OpenFile();



                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            fileContentFrom = reader.ReadToEnd();
                        }
                    }
                    mainWindow.fromTxt.Text = filePathFrom;
                }
                string text = File.ReadAllText(filePathFrom);
                Encrypt = EncryptAndDecrypt.Encrypt(text);
                Length = Encrypt.Length / 100;
            });

            ToBtn = new RelayCommand((sender) =>
            {
                using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;



                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePathTo = openFileDialog.FileName;



                        var fileStream = openFileDialog.OpenFile();



                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            filetext= reader.ReadToEnd();
                        }
                    }
                    mainWindow.toTxt.Text = filePathTo;
                }
            });

            CopyBtn = new RelayCommand((sender) =>
            {
                thread = new Thread(() => { File.AppendAllText(filePathTo, Encrypt); });
                thread.Start();
                mainWindow.progressBar.Value = 100;
                MessageBox.Show("copy was ended");
            });

            ResumeBtn = new RelayCommand((sender) =>
            {
                try
                {
                    thread.Resume();
                }
                catch (Exception)
                {
                }
            });

            PauseBtn = new RelayCommand((sender) =>
            {
                try
                {
                    thread.Suspend();
                }
                catch (Exception)
                {
                }
            });
        }
    }
}
