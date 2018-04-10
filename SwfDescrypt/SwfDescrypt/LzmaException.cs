using System;

namespace SwfDescrypt
{
	public class LzmaException : Exception
	{
		public LzmaException(string msg) : base(msg) { }
	}
}