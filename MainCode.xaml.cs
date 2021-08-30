using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using WK.Libraries.BetterFolderBrowserNS;

namespace Play_Key_Voice
{
    public partial class MainCode : Window
    {
        //レイアウトたちのリスト
        readonly List<int> Sound_Streams = new List<int>();
        readonly List<int> IsKeysDown = new List<int>();
        readonly List<float> Sound_Freqs = new List<float>();
        readonly List<TextBlock> Key_Names = new List<TextBlock>();
        readonly List<TextBox> Sound_Names = new List<TextBox>();
        readonly List<Button> Sound_Select = new List<Button>();
        readonly List<string> Sound_Full_Files = new List<string>();
        string Load_File_Path = "";
        bool IsClosing = false;
        bool IsMessageShowing = false;
        //キーの状態を取得する関数
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        public MainCode()
        {
            InitializeComponent();
            //Bass Audio Libraryの初期化
            Bass.BASS_Init(-1, 48000, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            Opacity = 0;
            //レイアウト(キー名やテキストボックスなど)を配置
            for (int Number = 0; Number < 10; Number++)
                Set_Layout_Number(Number);
            string[] Q_To_P = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
            for (int Number = 10; Number < 20; Number++)
                Set_Leyout_Q_To_P(Number, Q_To_P[Number - 10]);
            string[] A_To_L = { "A", "S", "D", "F", "G", "H", "J", "K", "L" };
            for (int Number = 20; Number < 29; Number++)
                Set_Leyout_A_To_L(Number, A_To_L[Number - 20]);
            string[] Z_To_M = { "Z", "X", "C", "V", "B", "N", "M" };
            for (int Number = 29; Number < 36; Number++)
                Set_Leyout_Z_To_M(Number, Z_To_M[Number - 29]);
            //初期値
            Volume_S.Value = 50;
            Speed_S.Value = 50;
            //フォルダにアクセスできなかったら強制終了
            string Dir = Directory.GetCurrentDirectory();
            if (!Sub_Code.CanDirectoryAccess(Dir))
            {
                MessageBox.Show("フォルダ\"" + Dir + "\"にアクセスできませんでした。位置を変更してください。");
                Application.Current.Shutdown();
            }
            //前回開いていたセーブファイルが存在すればロードする
            if (File.Exists(Dir + "\\Resources\\Load_Path.dat"))
            {
                Sub_Code.File_Decrypt(Dir + "\\Resources\\Load_Path.dat", Dir + "\\Resources\\Load_Path.dat.tmp", "SRTTbacon_Play_Key_Voice_Load_Path", false);
                StreamReader str = new StreamReader(Dir + "\\Resources\\Load_Path.dat.tmp");
                string PST_File = str.ReadLine();
                str.Close();
                str.Dispose();
                File.Delete(Dir + "\\Resources\\Load_Path.dat_tmp");
                if (File.Exists(PST_File))
                    Load_Preset(PST_File);
                else
                    File.Delete(Dir + "\\Resources\\Load_Path.dat");
            }
            Window_Show();
        }
        //メッセージを表示してフェードアウト
        private async void Message_Feed_Out(string Message)
        {
            if (IsMessageShowing)
            {
                IsMessageShowing = false;
                await Task.Delay(1000 / 59);
            }
            Message_T.Text = Message;
            IsMessageShowing = true;
            Message_T.Opacity = 1;
            int Number = 0;
            while (Message_T.Opacity > 0 && IsMessageShowing)
            {
                Number++;
                if (Number >= 120)
                    Message_T.Opacity -= 0.025;
                await Task.Delay(1000 / 60);
            }
            if (IsMessageShowing)
            {
                IsMessageShowing = false;
                Message_T.Text = "";
            }
            Message_T.Opacity = 1;
        }
        //画面を表示
        private async void Window_Show()
        {
            while (Opacity < 1 && !IsClosing)
            {
                Opacity += 0.04;
                await Task.Delay(1000 / 60);
            }
            Loop();
        }
        //ループ処理
        private async void Loop()
        {
            //60FPS
            double nextFrame = Environment.TickCount;
            float period = 1000f / 60f;
            while (Visibility == Visibility.Visible)
            {
                //FPSを上回っていたらスキップ
                double tickCount = Environment.TickCount;
                if (tickCount < nextFrame)
                {
                    if (nextFrame - tickCount > 1)
                        await Task.Delay((int)(nextFrame - tickCount));
                    System.Windows.Forms.Application.DoEvents();
                    continue;
                }
                //常にキーの状態を確認し、キーが押されたら指定されているサウンドを再生
                for (int Number = 0; Number < Sound_Names.Count; Number++)
                {
                    if (Number < 10)
                    {
                        Key Key_Name = (Key)Enum.Parse(typeof(Key), "D" + Number, true);
                        var vKey = KeyInterop.VirtualKeyFromKey(Key_Name);
                        short KeyState = GetAsyncKeyState(vKey);
                        if (KeyState != 0)
                        {
                            IsKeysDown[Number]++;
                            if (IsKeysDown[Number] == 1)
                            {
                                Bass.BASS_ChannelStop(Sound_Streams[Number]);
                                Bass.BASS_ChannelPlay(Sound_Streams[Number], false);
                            }
                        }
                        else
                            IsKeysDown[Number] = 0;
                    }
                    else
                    {
                        Key Key_Name = (Key)Enum.Parse(typeof(Key), Key_Names[Number].Text, true);
                        var vKey = KeyInterop.VirtualKeyFromKey(Key_Name);
                        short KeyState = GetAsyncKeyState(vKey);
                        if (KeyState != 0)
                        {
                            IsKeysDown[Number]++;
                            if (IsKeysDown[Number] == 1)
                            {
                                Bass.BASS_ChannelStop(Sound_Streams[Number]);
                                Bass.BASS_ChannelPlay(Sound_Streams[Number], false);
                            }
                        }
                        else
                            IsKeysDown[Number] = 0;
                    }
                }
                //次のフレーム時間を計算
                if (Environment.TickCount >= nextFrame + period)
                {
                    nextFrame += period;
                    continue;
                }
                nextFrame += period;
            }
        }
        //終了
        private async void Exit_B_Click(object sender, RoutedEventArgs e)
        {
            if (!IsClosing)
            {
                IsClosing = true;
                while (Opacity > 0)
                {
                    Opacity -= 0.05;
                    await Task.Delay(1000 / 60);
                }
                Application.Current.Shutdown();
            }
        }
        //画面サイズを整える
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Drawing.Size MaxSize = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size;
            MaxWidth = MaxSize.Width;
            MaxHeight = MaxSize.Height;
            Left = 0;
            Top = 0;
        }
        //サウンド追加
        private void Sound_Select_B_Click(int Number, string Key_Name)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog()
            {
                Title = "サウンドファイルを選択してください。",
                Filter = "サウンドファイル(*.wav; *.mp3)|*.wav;*.mp3",
                Multiselect = false
            };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Load_Sound(Number, Key_Name, ofd.FileName);
            ofd.Dispose();
        }
        //サウンドをロード
        private void Load_Sound(int Number, string Key_Name, string Sound_File)
        {
            int Key_Number = (int)(Key)Enum.Parse(typeof(Key), Key_Name, true);
            Bass.BASS_ChannelStop(Sound_Streams[Number]);
            Bass.BASS_StreamFree(Sound_Streams[Number]);
            int Stream = Bass.BASS_StreamCreateFile(Sound_File, 0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);
            Sound_Streams[Number] = BassFx.BASS_FX_TempoCreate(Stream, BASSFlag.BASS_FX_FREESOURCE);
            Sound_Full_Files[Number] = Sound_File;
            Sound_Names[Number].Text = Path.GetFileName(Sound_File);
            Bass.BASS_ChannelSetAttribute(Sound_Streams[Number], BASSAttribute.BASS_ATTRIB_VOL, (float)(Volume_S.Value / 100));
            float Freq = 44100f;
            Bass.BASS_ChannelGetAttribute(Sound_Streams[Number], BASSAttribute.BASS_ATTRIB_TEMPO_FREQ, ref Freq);
            Sound_Freqs[Number] = Freq;
            Bass.BASS_ChannelSetAttribute(Sound_Streams[Number], BASSAttribute.BASS_ATTRIB_TEMPO_FREQ, Sound_Freqs[Number] * (float)(Speed_S.Value / 50));
        }
        //プリセットを保存
        private void Save_Preset_B_Click(object sender, RoutedEventArgs e)
        {
            bool IsClear = true;
            for (int Number = 0; Number < Sound_Full_Files.Count; Number++)
            {
                if (Sound_Full_Files[Number] != "")
                {
                    IsClear = false;
                    break;
                }
            }
            if (IsClear)
            {
                Message_Feed_Out("保存する内容がありません。");
                return;
            }
            string Dir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Dir + "\\Preset"))
                Directory.CreateDirectory(Dir + "\\Preset");
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog()
            {
                Title = "プリセットの保存先を指定してください。",
                Filter = "プリセット(*.pst)|*.pst",
                InitialDirectory = Dir + "\\Preset"
            };
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter stw = File.CreateText(sfd.FileName + ".tmp");
                stw.WriteLine(Volume_S.Value);
                stw.WriteLine(Speed_S.Value);
                foreach (string File_Now in Sound_Full_Files)
                    stw.WriteLine(File_Now);
                stw.Close();
                Sub_Code.File_Encrypt(sfd.FileName + ".tmp", sfd.FileName, "Play_Voice_Preset_Data_By_SRTTbacon", true);
                Message_Feed_Out("プリセットを保存しました。");
            }
            sfd.Dispose();
        }
        //保存されたプリセットを読み込む
        private void Load_Preset_B_Click(object sender, RoutedEventArgs e)
        {
            string Dir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Dir + "\\Preset"))
                Directory.CreateDirectory(Dir + "\\Preset");
            System.Windows.Forms.OpenFileDialog sfd = new System.Windows.Forms.OpenFileDialog()
            {
                Title = "プリセットを指定してください。",
                Filter = "プリセット(*.pst)|*.pst",
                InitialDirectory = Dir + "\\Preset",
                Multiselect = false
            };
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Load_Preset(sfd.FileName);
            sfd.Dispose();
        }
        //保存されたプリセットを読み込む
        private void Load_Preset(string PST_File)
        {
            for (int Number = 0; Number < Sound_Names.Count; Number++)
            {
                Sound_Names[Number].Text = "";
                Sound_Freqs[Number] = 44100f;
                Sound_Full_Files[Number] = "";
            }
            for (int Number = 0; Number < Sound_Streams.Count; Number++)
            {
                Bass.BASS_ChannelStop(Sound_Streams[Number]);
                Bass.BASS_StreamFree(Sound_Streams[Number]);
            }
            Sub_Code.File_Decrypt(PST_File, PST_File + ".tmp", "Play_Voice_Preset_Data_By_SRTTbacon", false);
            string[] Lines = File.ReadAllLines(PST_File + ".tmp");
            File.Delete(PST_File + ".tmp");
            Volume_S.Value = double.Parse(Lines[0]);
            Speed_S.Value = double.Parse(Lines[1]);
            for (int Number = 2; Number < Lines.Length; Number++)
            {
                int Real_Number = Number - 2;
                if (File.Exists(Lines[Number]))
                {
                    Sound_Names[Real_Number].Text = Path.GetFileName(Lines[Number]);
                    Sound_Full_Files[Real_Number] = Lines[Number];
                    if (Real_Number < 10)
                        Load_Sound(Real_Number, "D" + Real_Number, Lines[Number]);
                    else
                        Load_Sound(Real_Number, Key_Names[Real_Number].Text, Lines[Number]);
                }
            }
            Load_File_Path = PST_File;
            Message_Feed_Out("プリセットをロードしました。");
        }
        //音量の変化
        private void Volume_S_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Volume_T.Text = "音量:" + (int)Volume_S.Value;
            for (int Number = 0; Number < Sound_Streams.Count; Number++)
                if (Sound_Streams[Number] != 0)
                    Bass.BASS_ChannelSetAttribute(Sound_Streams[Number], BASSAttribute.BASS_ATTRIB_VOL, (float)(Volume_S.Value / 100));
        }
        //名前が変更されたサウンドが入っているフォルダを選択
        private void Load_Dir_B_Click(object sender, RoutedEventArgs e)
        {
            BetterFolderBrowser bfb = new BetterFolderBrowser()
            {
                Title = "サウンドファイルが存在するフォルダを選択してください。",
                Multiselect = false,
                RootFolder = Sub_Code.Get_OpenDirectory_Path()
            };
            if (bfb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Sub_Code.Set_Directory_Path(bfb.SelectedFolder);
                foreach (string File_Now in Directory.GetFiles(bfb.SelectedFolder, "*", SearchOption.TopDirectoryOnly))
                {
                    string Name_Only = Path.GetFileNameWithoutExtension(File_Now);
                    string Ex = Path.GetExtension(File_Now);
                    if (Ex != ".wav" && Ex != ".mp3")
                        continue;
                    for (int Number = 0; Number < Key_Names.Count; Number++)
                    {
                        if (Key_Names[Number].Text == Name_Only)
                        {
                            if (Number < 10)
                                Load_Sound(Number, "D" + Number, File_Now);
                            else
                                Load_Sound(Number, Key_Names[Number].Text, File_Now);
                        }
                    }
                }
            }
        }
        //ヘルプ
        private void Load_Dir_Help_B_Click(object sender, RoutedEventArgs e)
        {
            string Message_01 = "指定したフォルダ内のファイル名からキーのサウンドを割り当てます。\n";
            string Message_02 = "例:ファイル名がA.wavまたはA.mp3であればAキーのサウンドへ自動で割り当てます。";
            string Message_03 = ".wavまたは.mp3形式のみ対応しています。";
            MessageBox.Show(Message_01 + Message_02 + Message_03);
        }
        //速度の変化
        private void Speed_S_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Speed_T.Text = "速度:" + (int)Speed_S.Value;
            for (int Number = 0; Number < Sound_Streams.Count; Number++)
                if (Sound_Streams[Number] != 0)
                    Bass.BASS_ChannelSetAttribute(Sound_Streams[Number], BASSAttribute.BASS_ATTRIB_TEMPO_FREQ, Sound_Freqs[Number] * (float)(Speed_S.Value / 50));
        }
        //速度のバーを右クリックすると初期値へ
        private void Speed_S_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Speed_S.Value = 50;
        }
        //現在開いているセーブファイルを取得しファイルに保存
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Load_File_Path == "")
                return;
            string Dir = Directory.GetCurrentDirectory();
            StreamWriter stw = File.CreateText(Dir + "\\Resources\\Load_Path.dat.tmp");
            stw.Write(Load_File_Path);
            stw.Close();
            Sub_Code.File_Encrypt(Dir + "\\Resources\\Load_Path.dat.tmp", Dir + "\\Resources\\Load_Path.dat", "SRTTbacon_Play_Key_Voice_Load_Path", true);
        }
        private void Clear_B_Click(object sender, RoutedEventArgs e)
        {
            bool IsClear = true;
            for (int Number = 0; Number < Sound_Full_Files.Count; Number++)
            {
                if (Sound_Full_Files[Number] != "")
                {
                    IsClear = false;
                    break;
                }
            }
            if (IsClear)
            {
                Message_Feed_Out("既にクリア状態です。");
                return;
            }
            MessageBoxResult result = MessageBox.Show("内容をクリアしますか?", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                for (int Number = 0; Number < Sound_Streams.Count; Number++)
                {
                    Bass.BASS_ChannelStop(Sound_Streams[Number]);
                    Bass.BASS_StreamFree(Sound_Streams[Number]);
                    IsKeysDown[Number] = 0;
                    Sound_Freqs[Number] = 44100f;
                    Sound_Names[Number].Text = "";
                    Sound_Full_Files[Number] = "";
                    Load_File_Path = "";
                    Message_Feed_Out("内容をクリアしました。");
                }
            }
        }
    }
}