using System;
using System.IO;

// Token: 0x02000002 RID: 2
internal class AsyncStreamReader
{
	public bool Active { get; private set; }

	// Token: 0x06000003 RID: 3 RVA: 0x00002059 File Offset: 0x00000259
	public AsyncStreamReader(StreamReader reader)
	{
		this._reader = reader;
		this._buffer = new byte[4096];
		this.Active = false;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000207F File Offset: 0x0000027F
	protected void Begin()
	{
		if (this.Active)
		{
			this._reader.BaseStream.BeginRead(this._buffer, 0, this._buffer.Length, new AsyncCallback(this.Read), null);
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000022A8 File Offset: 0x000004A8
	private void Read(IAsyncResult result)
	{
		if (this._reader != null)
		{
			int num = this._reader.BaseStream.EndRead(result);
			string value = null;
			if (num <= 0)
			{
				this.Active = false;
			}
			else
			{
				value = this._reader.CurrentEncoding.GetString(this._buffer, 0, num);
			}
			AsyncStreamReader.EventHandler<string> valueRecieved = this.ValueRecieved;
			if (valueRecieved != null)
			{
				valueRecieved(this, value);
			}
			this.Begin();
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020B6 File Offset: 0x000002B6
	public void Start()
	{
		if (!this.Active)
		{
			this.Active = true;
			this.Begin();
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000020CD File Offset: 0x000002CD
	public void Stop()
	{
		this.Active = false;
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000008 RID: 8 RVA: 0x00002310 File Offset: 0x00000510
	// (remove) Token: 0x06000009 RID: 9 RVA: 0x00002348 File Offset: 0x00000548
	public event AsyncStreamReader.EventHandler<string> ValueRecieved;

	// Token: 0x04000001 RID: 1
	private StreamReader _reader;

	// Token: 0x04000002 RID: 2
	protected readonly byte[] _buffer;

	// Token: 0x02000003 RID: 3
	// (Invoke) Token: 0x0600000B RID: 11
	public delegate void EventHandler<Args>(object sender, string value);
}
