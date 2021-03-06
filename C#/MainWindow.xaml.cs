﻿using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Qosmetics_QSaber_Fix
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int MajorV = 1;
        int MinorV = 1;
        int PatchV = 2;
        Boolean Preview = false;

        Boolean draggable = true;
        String exe = System.Reflection.Assembly.GetEntryAssembly().Location;

        public MainWindow()
        {
            InitializeComponent();
            exe = exe.Replace("\\Qosmetics QSaber Replacer.exe", "");
            if(!Directory.Exists(exe + "\\tmp"))
            {
                Directory.CreateDirectory(exe + "\\tmp");
            }
            if(File.Exists(exe + "\\Qosmetics QSaber Replacer Update.exe"))
            {
                File.Delete(exe + "\\Qosmetics QSaber Replacer Update.exe");
            }
            UpdateB.Visibility = Visibility.Hidden;
            Update();
            txtbox.AppendText("\n\nPlease Connect your Quest to your PC and hit run.");
        }

        private void Drag(object sender, RoutedEventArgs e)
        {
            bool mouseIsDown = System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed;


            if (mouseIsDown)
            {
                if (draggable)
                {
                    this.DragMove();
                }

            }

        }

        public void noDrag(object sender, MouseEventArgs e)
        {
            draggable = false;
        }

        public void doDrag(object sender, MouseEventArgs e)
        {
            draggable = true;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(exe + "\\tmp"))
            {
                Directory.Delete(exe + "\\tmp", true);
            }
            this.Close();
        }

        private void Mini(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void Choose(object sender, RoutedEventArgs e)
        {
            
                delete();
            
        }


        private void delete()
        {

            //delete old qsabers

            String User = System.Environment.GetEnvironmentVariable("USERPROFILE");
            ProcessStartInfo s = new ProcessStartInfo();
            s.CreateNoWindow = false;
            s.UseShellExecute = false;
            s.FileName = "adb.exe";
            s.WindowStyle = ProcessWindowStyle.Minimized;
            s.Arguments = "shell rm -f /sdcard/Android/data/com.beatgames.beatsaber/files/sabers/testSaber.qsaber";
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(s))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {

                ProcessStartInfo se = new ProcessStartInfo();
                se.CreateNoWindow = false;
                se.UseShellExecute = false;
                se.FileName = User + "\\AppData\\Roaming\\SideQuest\\platform-tools\\adb.exe";
                se.WindowStyle = ProcessWindowStyle.Minimized;
                se.Arguments = "shell rm -f /sdcard/Android/data/com.beatgames.beatsaber/files/sabers/testSaber.qsaber";
                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(se))
                    {
                        exeProcess.WaitForExit();
                    }
                }
                catch
                {
                    // Log error.
                    txtbox.AppendText("\n\n\nAn Error Occured (Code: ADB100). Check following");
                    txtbox.AppendText("\n\n- Your Quest is connected and USB Debugging enabled.");
                    txtbox.AppendText("\n\n- You have adb installed.");
                }

            }

            ProcessStartInfo sm = new ProcessStartInfo();
            sm.CreateNoWindow = false;
            sm.UseShellExecute = false;
            sm.FileName = "adb.exe";
            sm.WindowStyle = ProcessWindowStyle.Minimized;
            sm.Arguments = "shell rm -f /sdcard/Qosmetics/sabers/testSaber.qsaber";
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(sm))
                {
                    exeProcess.WaitForExit();
                    txtbox.AppendText("\n\nDeleted old QSaber(s). Now disable your QSaber in BMBF sync to Beat Saber and then enable it again.");
                }
            }
            catch
            {

                ProcessStartInfo sme = new ProcessStartInfo();
                sme.CreateNoWindow = false;
                sme.UseShellExecute = false;
                sme.FileName = User + "\\AppData\\Roaming\\SideQuest\\platform-tools\\adb.exe";
                sme.WindowStyle = ProcessWindowStyle.Minimized;
                sme.Arguments = "shell rm -f /sdcard/Qosmetics/sabers/testSaber.qsaber";
                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(sme))
                    {
                        exeProcess.WaitForExit();
                        txtbox.AppendText("\n\nDeleted old QSaber(s). Now disable your QSaber in BMBF sync to Beat Saber and then enable it again.");
                    }
                }
                catch
                {
                    // Log error.
                    txtbox.AppendText("\n\n\nAn Error Occured (Code: ADB100). Check following");
                    txtbox.AppendText("\n\n- Your Quest is connected and USB Debugging enabled.");
                    txtbox.AppendText("\n\n- You have adb installed.");
                }


            }
        }


        public void Update()
        {
            try
            {
                //Download Update.txt
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile("https://raw.githubusercontent.com/ComputerElite/Qosmetics-QSaber-Replacer/master/Update.txt", exe + "\\tmp\\Update.txt");

                    }
                    catch
                    {
                        txtbox.AppendText("\n\n\nAn error Occured (Code: UD100). Couldn't check for Updates. Check following");
                        txtbox.AppendText("\n\n- Your PC has internet.");
                    }
                }
                StreamReader VReader = new StreamReader(exe + "\\tmp\\Update.txt");

                String line;
                int l = 0;

                int MajorU = 0;
                int MinorU = 0;
                int PatchU = 0;
                while ((line = VReader.ReadLine()) != null)
                {
                    if (l == 0)
                    {
                        String URL = line;
                    }
                    if (l == 1)
                    {
                        MajorU = Convert.ToInt32(line);
                    }
                    if (l == 2)
                    {
                        MinorU = Convert.ToInt32(line);
                    }
                    if (l == 3)
                    {
                        PatchU = Convert.ToInt32(line);
                    }
                    l++;
                }

                if (MajorU > MajorV || MinorU > MinorV || PatchU > PatchV)
                {
                    //Newer Version available
                    UpdateB.Visibility = Visibility.Visible;
                }

                String MajorVS = Convert.ToString(MajorV);
                String MinorVS = Convert.ToString(MinorV);
                String PatchVS = Convert.ToString(PatchV);
                String MajorUS = Convert.ToString(MajorU);
                String MinorUS = Convert.ToString(MinorU);
                String PatchUS = Convert.ToString(PatchU);

                String VersionVS = MajorVS + MinorVS + PatchVS;
                int VersionV = Convert.ToInt32(VersionVS);
                String VersionUS = MajorUS + MinorUS + PatchUS + " ";
                int VersionU = Convert.ToInt32(VersionUS);
                if (VersionV > VersionU)
                {
                    //Newer Version that hasn't been released yet
                    txtbox.AppendText("\n\nLooks like you have a preview version. Downgrade now from " + MajorV + "." + MinorV + "." + PatchV + " to " + MajorU + "." + MinorU + "." + PatchU + " xD");
                    UpdateB.Visibility = Visibility.Visible;
                    UpdateB.Content = "Downgrade Now xD";
                }
                if (VersionV == VersionU && Preview)
                {
                    //User has Preview Version but a release Version has been released
                    txtbox.AppendText("\n\nLooks like you have a preview version. The release version has been released. Please Update now. ");
                    UpdateB.Visibility = Visibility.Visible;
                }

            }
            catch
            {

            }
        }

        private void Start_Update(object sender, RoutedEventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/ComputerElite/Qosmetics-QSaber-Replacer/raw/master/Qosmetics%20QSaber%20Replacer%20Update.exe", exe + "\\Qosmetics QSaber Replacer Update.exe");
            }
            //Process.Start(exe + "\\QSE_Update.exe");
            ProcessStartInfo s = new ProcessStartInfo();
            s.CreateNoWindow = false;
            s.UseShellExecute = false;
            s.FileName = exe + "\\Qosmetics QSaber Replacer Update.exe";
            try
            {
                using (Process exeProcess = Process.Start(s))
                {
                }
                this.Close();
            }
            catch
            {
                // Log error.
                txtbox.AppendText("\nAn Error Occured (Code: UD200)");
            }
        }
    }

    


}
