using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Play_Key_Voice
{
    public class Sub_Code
    {
        private static string Path = Directory.GetCurrentDirectory();
        //現在の時間を文字列で取得
        //引数:DateTime.Now,間に入れる文字,どの部分から開始するか,どの部分で終了するか(その数字の部分は含まれる)
        //First,End->1 = Year,2 = Month,3 = Date,4 = Hour,5 = Minutes,6 = Seconds
        public static string Get_Time_Now(DateTime dt, string Between, int First, int End)
        {
            if (First > End)
                return "";
            if (First == End)
                return Get_Time_Index(dt, First);
            string Temp = "";
            for (int Number = First; Number <= End; Number++)
            {
                if (Number != End)
                    Temp += Get_Time_Index(dt, Number) + Between;
                else
                    Temp += Get_Time_Index(dt, Number);
            }
            return Temp;
        }
        //時間を取得
        static string Get_Time_Index(DateTime dt, int Index)
        {
            if (Index > 0 && Index < 7)
            {
                if (Index == 1)
                    return dt.Year.ToString();
                else if (Index == 2)
                    return dt.Month.ToString();
                else if (Index == 3)
                    return dt.Day.ToString();
                else if (Index == 4)
                    return dt.Hour.ToString();
                else if (Index == 5)
                    return dt.Minute.ToString();
                else if (Index == 6)
                    return dt.Second.ToString();
            }
            return "";
        }
        //エラーをログに記録(改行コードはあってもなくてもよい)
        public static void Error_Log_Write(string Text)
        {
            DateTime dt = DateTime.Now;
            string Time = Get_Time_Now(dt, ".", 1, 6);
            if (Text.EndsWith("\n"))
                File.AppendAllText(Directory.GetCurrentDirectory() + "/Error_Log.txt", Time + ":" + Text);
            else
                File.AppendAllText(Directory.GetCurrentDirectory() + "/Error_Log.txt", Time + ":" + Text + "\n");
        }
        //ファイルを暗号化
        //引数:元ファイルのパス,暗号先のパス,元ファイルを削除するか
        public static bool File_Encrypt(string From_File, string To_File, string Password, bool IsFromFileDelete)
        {
            try
            {
                if (!File.Exists(From_File))
                    return false;
                using (var eifs = new FileStream(From_File, FileMode.Open, FileAccess.Read))
                {
                    using (var eofs = new FileStream(To_File, FileMode.Create, FileAccess.Write))
                        FileEncode.FileEncryptor.Encrypt(eifs, eofs, Password);
                }
                if (IsFromFileDelete)
                    File.Delete(From_File);
                return true;
            }
            catch (Exception e)
            {
                Error_Log_Write(e.Message);
                return false;
            }
        }
        //ファイルを復号化
        //引数:元ファイルのパス,復号先のパス,元ファイルを削除するか
        public static bool File_Decrypt(string From_File, string To_File, string Password, bool IsFromFileDelete)
        {
            try
            {
                if (!File.Exists(From_File))
                    return false;
                using (var eifs = new FileStream(From_File, FileMode.Open, FileAccess.Read))
                {
                    using (var eofs = new FileStream(To_File, FileMode.Create, FileAccess.Write))
                        FileEncode.FileEncryptor.Decrypt(eifs, eofs, Password);
                }
                if (IsFromFileDelete)
                    File.Delete(From_File);
                return true;
            }
            catch (Exception e)
            {
                Error_Log_Write(e.Message);
                return false;
            }
        }
        //指定したフォルダにアクセスできるか
        public static bool CanDirectoryAccess(string Dir_Path)
        {
            try
            {
                WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                DirectorySecurity security = Directory.GetAccessControl(Dir_Path);
                AuthorizationRuleCollection authRules = security.GetAccessRules(true, true, typeof(SecurityIdentifier));
                foreach (FileSystemAccessRule accessRule in authRules)
                    if (principal.IsInRole(accessRule.IdentityReference as SecurityIdentifier))
                        if ((FileSystemRights.WriteData & accessRule.FileSystemRights) == FileSystemRights.WriteData)
                            if (accessRule.AccessControlType == AccessControlType.Allow)
                                return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        //フォルダ選択画面の初期フォルダを取得
        public static string Get_OpenDirectory_Path()
        {
            string InDir = Path;
            if (File.Exists(Path + "/Resources/OpenDirectoryPath.dat"))
            {
                try
                {
                    Sub_Code.File_Decrypt(Path + "/Resources/OpenDirectoryPath.dat", Path + "/Resources/OpenDirectoryPath.tmp", "Directory_Save_SRTTbacon", false);
                    StreamReader str = new StreamReader(Path + "/Resources/OpenDirectoryPath.tmp");
                    string Read = str.ReadLine();
                    str.Close();
                    if (Directory.Exists(Read))
                        InDir = Read;
                    File.Delete(Path + "/Resources/OpenDirectoryPath.tmp");
                }
                catch
                {
                }
            }
            return InDir;
        }
        //フォルダ選択画面の初期フォルダを更新
        public static bool Set_Directory_Path(string Dir)
        {
            if (!Directory.Exists(Dir))
                return false;
            try
            {
                StreamWriter stw = File.CreateText(Path + "/Resources/OpenDirectoryPath.tmp");
                stw.Write(Dir);
                stw.Close();
                Sub_Code.File_Encrypt(Path + "/Resources/OpenDirectoryPath.tmp", Path + "/Resources/OpenDirectoryPath.dat", "Directory_Save_SRTTbacon", true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}