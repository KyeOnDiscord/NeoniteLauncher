using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.Win32;
using FortniteLauncher.Properties;
namespace FortniteLauncher
{
	public partial class GUI : MaterialForm
	{
		public GUI()
		{
			InitializeComponent();
			MaterialSkinManager instance = MaterialSkinManager.Instance;
			instance.AddFormToManage(this);
			instance.Theme = MaterialSkinManager.Themes.DARK;
			instance.ColorScheme = new ColorScheme(Primary.Blue400, Primary.Blue900, Primary.Blue900, Accent.Blue200, TextShade.WHITE);
			materialTextBox1.Text = Settings.Default.Username;
			materialTextBox2.Text = Settings.Default.Path;
		}
		[DllImport("wininet.dll")]
		public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
		[DllImport("user32.dll")]
		public new static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public new static extern bool ReleaseCapture();
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int processId);
		private bool IsValidPath(string path)
		{
			if (new Regex("^[a-zA-Z]:\\\\$").IsMatch(path.Substring(0, 3)))
			{
				string text = new string(Path.GetInvalidPathChars());
				text += ":/?*\"";
				return !new Regex("[" + Regex.Escape(text) + "]").IsMatch(path.Substring(3, path.Length - 3));
			}
			return false;
		}
		private void materialButton2_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialogBrowse.ShowDialog() == DialogResult.OK)
			{
				materialTextBox2.Text = folderBrowserDialogBrowse.SelectedPath;
			}
		}
		private void materialButton1_Click(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(Console.ReadLine());
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
			if (!new Regex("^([a-zA-Z0-9])*$").IsMatch(this.materialTextBox1.Text))
			{
				MessageBox.Show("Invalid Username. usernames cannot contain any special characters.");
				return;
			}
			if (string.IsNullOrEmpty(this.materialTextBox2.Text) || this.materialTextBox1.Text.Length < 3)
			{
				MessageBox.Show("Username cannot be empty or below 3 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			try
			{
				if (!this.IsValidPath(this.materialTextBox2.Text))
				{
					MessageBox.Show("Invalid Fortnite path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			catch
			{
				MessageBox.Show("Invalid Fortnite path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			string text = Path.Combine(this.materialTextBox2.Text, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe");
			if (!File.Exists(text))
			{
				MessageBox.Show("\"FortniteClient-Win64-Shipping.exe\" was not found, please make sure it exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			Settings.Default.Username = this.materialTextBox1.Text;
			Settings.Default.Path = this.materialTextBox2.Text;
			Settings.Default.Save();
			string text2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Platanium.dll");
			if (!File.Exists(text2))
			{
				MessageBox.Show("\"Platanium.dll\" was not found, please make sure it exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			string text3 = "-AUTH_LOGIN=\"" + this.materialTextBox1.Text + "@unused.com\" -AUTH_PASSWORD=unused -AUTH_TYPE=epic";
			_clientAnticheat = 2;

			switch (_clientAnticheat)
            {
				case 0:
					text3 += " -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -nobe -fltoken=none";
					break;

				case 1:
					text3 += " -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=f7b9gah4h5380d10f721dd6a";
					break;

				case 2:
					text3 += " -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -nobe -fltoken=8c4aa8a9b77acdcbd918874b";
					break;
            }
			_clientProcess = new Process
			{
				StartInfo = new ProcessStartInfo(text, text3)
				{
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = false
				}
			};
			Process process = new Process();
			string fileName = Path.Combine(this.materialTextBox2.Text, "FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe");
			process.StartInfo.FileName = fileName;
			process.Start();
			foreach (object obj in process.Threads)
			{
				ProcessThread processThread = (ProcessThread)obj;
				Win32.SuspendThread(Win32.OpenThread(2, false, processThread.Id));
			}
			Process process2 = new Process();
			string fileName2 = Path.Combine(this.materialTextBox2.Text, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_EAC.exe");
			process2.StartInfo.FileName = fileName2;
			process2.StartInfo.Arguments = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -nobe -fltoken=8c4aa8a9b77acdcbd918874b";
			process2.Start();
			foreach (object obj2 in process2.Threads)
			{
				ProcessThread processThread2 = (ProcessThread)obj2;
				Win32.SuspendThread(Win32.OpenThread(2, false, processThread2.Id));
			}
			GUI._clientProcess.Start();
			if (num != 0)
			{
				Console.WriteLine("Invalid Argument!");
				Console.ReadKey();
				return;
			}
			registryKey.SetValue("ProxyEnable", 0);
			registryKey.SetValue("ProxyServer", 0);
			if ((int)registryKey.GetValue("ProxyEnable", 1) != 1)
			{
				Console.WriteLine("The proxy has been turned off.");
			}
			else
			{
				Console.WriteLine("Unable to disable the proxy.");
			}
			registryKey.Close();
			GUI.InternetSetOption(IntPtr.Zero, 39, IntPtr.Zero, 0);
			GUI.InternetSetOption(IntPtr.Zero, 37, IntPtr.Zero, 0);
			IntPtr hProcess = Win32.OpenProcess(1082, false, GUI._clientProcess.Id);
			IntPtr procAddress = Win32.GetProcAddress(Win32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
			uint num2 = (uint)((text2.Length + 1) * Marshal.SizeOf(typeof(char)));
			IntPtr intPtr = Win32.VirtualAllocEx(hProcess, IntPtr.Zero, num2, 12288U, 4U);
			UIntPtr uintPtr;
			Win32.WriteProcessMemory(hProcess, intPtr, Encoding.Default.GetBytes(text2), num2, out uintPtr);
			Win32.CreateRemoteThread(hProcess, IntPtr.Zero, 0U, procAddress, intPtr, 0U, IntPtr.Zero);
			return;
		}

		private void materialButton3_Click_1(object sender, EventArgs e)
		{
			Process.Start("https://discord.gg/DJ6VUmD");
		}
		public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
		public const int INTERNET_OPTION_REFRESH = 37;
		private static Process _clientProcess;
		private static byte _clientAnticheat;

        private void materialButton4_Click(object sender, EventArgs e)
        {
			Process.Start("http://127.0.0.1:5595/panel?user=" + materialTextBox1.Text);
        }
    }
}
