using System;
using System.Runtime.InteropServices;

// Token: 0x02000005 RID: 5
public static class Win32
{
	// Token: 0x06000012 RID: 18
	[DllImport("kernel32.dll")]
	public static extern int SuspendThread(IntPtr hThread);

	// Token: 0x06000013 RID: 19
	[DllImport("kernel32.dll")]
	public static extern int ResumeThread(IntPtr hThread);

	// Token: 0x06000014 RID: 20
	[DllImport("kernel32.dll")]
	public static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);

	// Token: 0x06000015 RID: 21
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool CloseHandle(IntPtr hHandle);

	// Token: 0x06000016 RID: 22
	[DllImport("kernel32.dll")]
	public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

	// Token: 0x06000017 RID: 23
	[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

	// Token: 0x06000018 RID: 24
	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetModuleHandle(string lpModuleName);

	// Token: 0x06000019 RID: 25
	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

	// Token: 0x0600001A RID: 26
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

	// Token: 0x0600001B RID: 27
	[DllImport("kernel32.dll")]
	public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
}
