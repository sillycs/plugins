using System;
using System.Collections.Generic;

namespace Stuff
{
	[Serializable]
	internal class PacketStuff
	{
		public Dictionary<string, byte[]> Objects { get; set; }
	}
}