using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Stuff
{
	public class Helpers
	{
		private static string STRING = "abcdefghijklmnopqrstuvwxyz";
		private static string INTEGER = "0123456789";

		private static Random charsRandom = new Random();
		private static Random lengthRandom = new Random();

		public static string Random(int length = 0)
		{
			if (length == 0)
			{
				length = 30;
			}

			lengthRandom.Next(1, length);

			string element = STRING.ToUpper() + STRING + INTEGER;

			return new string(
				(from s in Enumerable.Repeat(element, length)
				 select s[charsRandom.Next(s.Length)]).ToArray()
			);
		}

		public static string BytesToString(long byteCount)
		{
			string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

			if (byteCount == 0)
				return "0" + sizes[0];

			long bytes = Math.Abs(byteCount);
			int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
			double num = Math.Round(bytes / Math.Pow(1024, place), 3);

			return (Math.Sign(byteCount) * num) + sizes[place];
		}

		public static string ConvertToHex(byte[] bytes)
		{
			StringBuilder sb = new StringBuilder();

			foreach (byte b in bytes)
			{
				sb.Append(b.ToString("x2"));
			}

			return sb.ToString();
		}

		public static string MD5_STRING(byte[] bytes)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] hash = md5.ComputeHash(bytes, 0, bytes.Length);
				return ConvertToHex(hash);
			}
		}

		public static string SHA1_STRING(byte[] bytes)
		{
			using (SHA1 sha1 = SHA1.Create())
			{
				byte[] hash = sha1.ComputeHash(bytes, 0, bytes.Length);
				return ConvertToHex(hash);
			}
		}

		public static string SHA256_STRING(byte[] bytes)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] hash = sha256.ComputeHash(bytes, 0, bytes.Length);
				return ConvertToHex(hash);
			}
		}
	}
}
