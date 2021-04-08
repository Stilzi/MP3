using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MyMp3Player
{
    public partial class Form1 : Form
    {
        private Mp3Player mp3Player = new Mp3Player();
        public Form1()
        {
            InitializeComponent();
            movePanel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Mp3 Files|*.mp3";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    mp3Player.open(ofd.FileName);
                }
            } // Используется для открытия диалогового окна выбора файла для дальнейшего воспроизведения
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mp3Player.play();
        } // Кнопка запуска проигрывателя

        private void button4_Click(object sender, EventArgs e)
        {
            mp3Player.stop();
        } // Кнопка для остановки воспроизведения

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        } // Закрывает программу целиком

        void movePanel()
        {
            int move = 0, moveX = 0, moveY = 0;
            panel1.MouseDown += (s, e) => { move = 1; moveX = e.X; moveY = e.Y; };
            panel1.MouseMove += (s, e) => { if (move == 1) SetDesktopLocation(MousePosition.X - moveX, MousePosition.Y - moveY); };
            panel1.MouseUp += (s, e) => { move = 0; };
        } // Специальный   метод для захвата и движения программы по экрану



        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            uint CurrVol = ushort.MaxValue / 2;
            NativeMethods.waveOutGetVolume(IntPtr.Zero, out CurrVol);
            ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
            int NewVolume = ((ushort.MaxValue / 100) * trackBar1.Value);
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            NativeMethods.waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
            label1.Text = Convert.ToString("Volume: " + trackBar1.Value + "%");
        } // Трекбар регулирующий уровень громкости аудио
    }

    public static class NativeMethods
    {
    
        [DllImport("winmm.dll")]
        internal static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);
        [DllImport("winmm.dll")]
        internal static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
    } // Специальный класс для работы прогарммы с аудио
}
