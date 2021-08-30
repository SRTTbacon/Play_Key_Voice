using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Play_Key_Voice
{
    public partial class MainCode : Window
    {
        //1～9までのレイアウトを追加
        private void Set_Layout_Number(int Number)
        {
            double Location = 212.5 * (Number - 1);
            int Top_Amount = 150;
            if (Number == 0)
            {
                Location = 850;
                Top_Amount = 80;
            }
            Sound_Full_Files.Add("");
            IsKeysDown.Add(0);
            Sound_Streams.Add(0);
            Sound_Freqs.Add(44100f);
            Key_Names.Add(new TextBlock());
            Key_Names[Number].Foreground = Brushes.Aqua;
            Key_Names[Number].Width = 30;
            Key_Names[Number].Height = 50;
            Key_Names[Number].TextWrapping = TextWrapping.Wrap;
            Key_Names[Number].Text = Number.ToString();
            Key_Names[Number].FontSize = 40;
            Key_Names[Number].VerticalAlignment = VerticalAlignment.Top;
            Key_Names[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Key_Names[Number].TextAlignment = TextAlignment.Center;
            Key_Names[Number].Margin = new Thickness(-1920 + Location, Top_Amount, 0, 0);
            Sound_Names.Add(new TextBox());
            Sound_Names[Number].Width = 140;
            Sound_Names[Number].Height = 40;
            Sound_Names[Number].FontSize = 25;
            Sound_Names[Number].TextAlignment = TextAlignment.Center;
            Sound_Names[Number].TextWrapping = TextWrapping.NoWrap;
            Sound_Names[Number].VerticalAlignment = VerticalAlignment.Top;
            Sound_Names[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Sound_Names[Number].Text = "";
            Sound_Names[Number].AllowDrop = true;
            Sound_Names[Number].PreviewDragOver += Key_Name_DragOver;
            Sound_Names[Number].Drop += delegate (object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] Drop_Files = e.Data.GetData(DataFormats.FileDrop) as string[];
                    string Ex = System.IO.Path.GetExtension(Drop_Files[0]);
                    if (Ex == ".wav" || Ex == ".mp3")
                        Load_Sound(Number, "D" + Number, Drop_Files[0]);
                }
            };
            Sound_Names[Number].Margin = new Thickness(-1890 + Location, Top_Amount + 10, 0, 0);
            Sound_Select.Add(new Button());
            Sound_Select[Number].Foreground = Brushes.Aqua;
            Sound_Select[Number].FontSize = 25;
            Sound_Select[Number].Background = Brushes.Transparent;
            Sound_Select[Number].BorderBrush = Brushes.Aqua;
            Sound_Select[Number].Content = "・・・";
            Sound_Select[Number].VerticalAlignment = VerticalAlignment.Top;
            Sound_Select[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Sound_Select[Number].Width = 40;
            Sound_Select[Number].Height = 40;
            Sound_Select[Number].Margin = new Thickness(-1750 + Location, Top_Amount + 10, 0, 0);
            Sound_Select[Number].Focusable = false;
            Sound_Select[Number].Click += delegate
            {
                Sound_Select_B_Click(Number, "D" + Number);
            };
            Parent_Dock.Children.Add(Key_Names[Number]);
            Parent_Dock.Children.Add(Sound_Names[Number]);
            Parent_Dock.Children.Add(Sound_Select[Number]);
        }
        //Q～Pまでのレイアウトを追加
        private void Set_Leyout_Q_To_P(int Number, string Key)
        {
            double Location = 212.5 * (Number - 10);
            int Top_Amount = 350;
            if (Number == 19)
            {
                Location = 850;
                Top_Amount = 280;
            }
            Sound_Full_Files.Add("");
            IsKeysDown.Add(0);
            Sound_Streams.Add(0);
            Sound_Freqs.Add(44100f);
            Key_Names.Add(new TextBlock());
            Key_Names[Number].Foreground = Brushes.Aqua;
            Key_Names[Number].Width = 30;
            Key_Names[Number].Height = 50;
            Key_Names[Number].TextWrapping = TextWrapping.Wrap;
            Key_Names[Number].Text = Key;
            Key_Names[Number].FontSize = 35;
            Key_Names[Number].VerticalAlignment = VerticalAlignment.Top;
            Key_Names[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Key_Names[Number].TextAlignment = TextAlignment.Center;
            Key_Names[Number].Margin = new Thickness(-1920 + Location, Top_Amount + 5, 0, 0);
            Sound_Names.Add(new TextBox());
            Sound_Names[Number].Width = 140;
            Sound_Names[Number].Height = 40;
            Sound_Names[Number].FontSize = 25;
            Sound_Names[Number].TextAlignment = TextAlignment.Center;
            Sound_Names[Number].TextWrapping = TextWrapping.NoWrap;
            Sound_Names[Number].VerticalAlignment = VerticalAlignment.Top;
            Sound_Names[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Sound_Names[Number].Text = "";
            Sound_Names[Number].AllowDrop = true;
            Sound_Names[Number].PreviewDragOver += Key_Name_DragOver;
            Sound_Names[Number].Drop += delegate (object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] Drop_Files = e.Data.GetData(DataFormats.FileDrop) as string[];
                    string Ex = System.IO.Path.GetExtension(Drop_Files[0]);
                    if (Ex == ".wav" || Ex == ".mp3")
                        Load_Sound(Number, Key, Drop_Files[0]);
                }
            };
            Sound_Names[Number].Margin = new Thickness(-1890 + Location, Top_Amount + 10, 0, 0);
            Sound_Select.Add(new Button());
            Sound_Select[Number].Foreground = Brushes.Aqua;
            Sound_Select[Number].FontSize = 25;
            Sound_Select[Number].Background = Brushes.Transparent;
            Sound_Select[Number].BorderBrush = Brushes.Aqua;
            Sound_Select[Number].Content = "・・・";
            Sound_Select[Number].VerticalAlignment = VerticalAlignment.Top;
            Sound_Select[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Sound_Select[Number].Width = 40;
            Sound_Select[Number].Height = 40;
            Sound_Select[Number].Margin = new Thickness(-1750 + Location, Top_Amount + 10, 0, 0);
            Sound_Select[Number].Focusable = false;
            Sound_Select[Number].Click += delegate
            {
                Sound_Select_B_Click(Number, Key);
            };
            Parent_Dock.Children.Add(Key_Names[Number]);
            Parent_Dock.Children.Add(Sound_Names[Number]);
            Parent_Dock.Children.Add(Sound_Select[Number]);
        }
        //A～Lまでのレイアウトを追加
        private void Set_Leyout_A_To_L(int Number, string Key)
        {
            double Location = 212.5 * (Number - 20);
            int Top_Amount = 475;
            Sound_Full_Files.Add("");
            IsKeysDown.Add(0);
            Sound_Streams.Add(0);
            Sound_Freqs.Add(44100f);
            Key_Names.Add(new TextBlock());
            Key_Names[Number].Foreground = Brushes.Aqua;
            Key_Names[Number].Width = 30;
            Key_Names[Number].Height = 50;
            Key_Names[Number].TextWrapping = TextWrapping.Wrap;
            Key_Names[Number].Text = Key;
            Key_Names[Number].FontSize = 35;
            Key_Names[Number].VerticalAlignment = VerticalAlignment.Top;
            Key_Names[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Key_Names[Number].TextAlignment = TextAlignment.Center;
            Key_Names[Number].Margin = new Thickness(-1920 + Location, Top_Amount + 5, 0, 0);
            Sound_Names.Add(new TextBox());
            Sound_Names[Number].Width = 140;
            Sound_Names[Number].Height = 40;
            Sound_Names[Number].FontSize = 25;
            Sound_Names[Number].TextAlignment = TextAlignment.Center;
            Sound_Names[Number].TextWrapping = TextWrapping.NoWrap;
            Sound_Names[Number].VerticalAlignment = VerticalAlignment.Top;
            Sound_Names[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Sound_Names[Number].Text = "";
            Sound_Names[Number].AllowDrop = true;
            Sound_Names[Number].PreviewDragOver += Key_Name_DragOver;
            Sound_Names[Number].Drop += delegate (object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] Drop_Files = e.Data.GetData(DataFormats.FileDrop) as string[];
                    string Ex = System.IO.Path.GetExtension(Drop_Files[0]);
                    if (Ex == ".wav" || Ex == ".mp3")
                        Load_Sound(Number, Key, Drop_Files[0]);
                }
            };
            Sound_Names[Number].Margin = new Thickness(-1890 + Location, Top_Amount + 10, 0, 0);
            Sound_Select.Add(new Button());
            Sound_Select[Number].Foreground = Brushes.Aqua;
            Sound_Select[Number].FontSize = 25;
            Sound_Select[Number].Background = Brushes.Transparent;
            Sound_Select[Number].BorderBrush = Brushes.Aqua;
            Sound_Select[Number].Content = "・・・";
            Sound_Select[Number].VerticalAlignment = VerticalAlignment.Top;
            Sound_Select[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Sound_Select[Number].Width = 40;
            Sound_Select[Number].Height = 40;
            Sound_Select[Number].Margin = new Thickness(-1750 + Location, Top_Amount + 10, 0, 0);
            Sound_Select[Number].Focusable = false;
            Sound_Select[Number].Click += delegate
            {
                Sound_Select_B_Click(Number, Key);
            };
            Parent_Dock.Children.Add(Key_Names[Number]);
            Parent_Dock.Children.Add(Sound_Names[Number]);
            Parent_Dock.Children.Add(Sound_Select[Number]);
        }
        //Z～Mまでのレイアウトを追加
        private void Set_Leyout_Z_To_M(int Number, string Key)
        {
            double Location = 230 * (Number - 29);
            int Top_Amount = 600;
            Sound_Full_Files.Add("");
            IsKeysDown.Add(0);
            Sound_Streams.Add(0);
            Sound_Freqs.Add(44100f);
            Key_Names.Add(new TextBlock());
            Key_Names[Number].Foreground = Brushes.Aqua;
            Key_Names[Number].Width = 30;
            Key_Names[Number].Height = 50;
            Key_Names[Number].TextWrapping = TextWrapping.Wrap;
            Key_Names[Number].Text = Key;
            Key_Names[Number].FontSize = 35;
            Key_Names[Number].VerticalAlignment = VerticalAlignment.Top;
            Key_Names[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Key_Names[Number].TextAlignment = TextAlignment.Center;
            Key_Names[Number].Margin = new Thickness(-1780 + Location, Top_Amount + 5, 0, 0);
            Sound_Names.Add(new TextBox());
            Sound_Names[Number].Width = 140;
            Sound_Names[Number].Height = 40;
            Sound_Names[Number].FontSize = 25;
            Sound_Names[Number].TextAlignment = TextAlignment.Center;
            Sound_Names[Number].TextWrapping = TextWrapping.NoWrap;
            Sound_Names[Number].VerticalAlignment = VerticalAlignment.Top;
            Sound_Names[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Sound_Names[Number].Text = "";
            Sound_Names[Number].AllowDrop = true;
            Sound_Names[Number].PreviewDragOver += Key_Name_DragOver;
            Sound_Names[Number].Drop += delegate (object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] Drop_Files = e.Data.GetData(DataFormats.FileDrop) as string[];
                    string Ex = System.IO.Path.GetExtension(Drop_Files[0]);
                    if (Ex == ".wav" || Ex == ".mp3")
                        Load_Sound(Number, Key, Drop_Files[0]);
                }
            };
            Sound_Names[Number].Margin = new Thickness(-1740 + Location, Top_Amount + 10, 0, 0);
            Sound_Select.Add(new Button());
            Sound_Select[Number].Foreground = Brushes.Aqua;
            Sound_Select[Number].FontSize = 25;
            Sound_Select[Number].Background = Brushes.Transparent;
            Sound_Select[Number].BorderBrush = Brushes.Aqua;
            Sound_Select[Number].Content = "・・・";
            Sound_Select[Number].VerticalAlignment = VerticalAlignment.Top;
            Sound_Select[Number].HorizontalAlignment = HorizontalAlignment.Left;
            Sound_Select[Number].Width = 40;
            Sound_Select[Number].Height = 40;
            Sound_Select[Number].Margin = new Thickness(-1600 + Location, Top_Amount + 10, 0, 0);
            Sound_Select[Number].Focusable = false;
            Sound_Select[Number].Click += delegate
            {
                Sound_Select_B_Click(Number, Key);
            };
            Parent_Dock.Children.Add(Key_Names[Number]);
            Parent_Dock.Children.Add(Sound_Names[Number]);
            Parent_Dock.Children.Add(Sound_Select[Number]);
        }
        //テキストボックスにファイルをドラッグしたときの処理
        private void Key_Name_DragOver(object sender, DragEventArgs e)
        {
            string[] Drag_Files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string Ex = System.IO.Path.GetExtension(Drag_Files[0]);
            if (Ex == ".wav" || Ex == ".mp3")
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }
    }
}