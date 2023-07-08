using System.Text;

namespace SAPI.LLAPI.Utilities
{
	// ReSharper disable once InconsistentNaming
	internal static class FileIO
	{
		private static string tempFile;
		
		/// <summary>
		/// Implementation of SaveFile
		/// </summary>
		/// <param name="enc">file encoding</param>
		/// <param name="boundary">file boundary</param>
		/// <param name="input">input stream</param>
		/// <returns>path to file</returns>
		public static string SaveFile(Encoding enc, String boundary, Stream input)
		{
			Byte[] boundaryBytes = enc.GetBytes(boundary);
			Int32 boundaryLen = boundaryBytes.Length;

			tempFile = Path.GetTempFileName();

			using (FileStream output = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
			{
				Byte[] buffer = new Byte[1024];
				int len = input.Read(buffer, 0, 1024);
				int startPos = -1;

				// Find start boundary
				while (true)
				{
					if (len == 0)
						Debug.Log("Start boundary not found. Probably file was not sent alongside the request.");


					startPos = IndexOf(buffer, len, boundaryBytes);
					if (startPos >= 0)
						break;

					Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
					len = input.Read(buffer, boundaryLen, 1024 - boundaryLen);
				}

				// Skip four lines (Boundary, Content-Disposition, Content-Type, and a blank)
				for (int i = 0; i < 4; i++)
				{
					while (true)
					{
						if (len == 0)
							Debug.Log("Preamble not found");

						startPos = Array.IndexOf(buffer, enc.GetBytes("\n")[0], startPos);
						if (startPos >= 0)
						{
							startPos++;
							break;
						}

						len = input.Read(buffer, 0, 1024);
					}
				}

				Array.Copy(buffer, startPos, buffer, 0, len - startPos);
				len -= startPos;

				while (true)
				{
					int endPos = IndexOf(buffer, len, boundaryBytes);
					if (endPos >= 0)
					{
						if (endPos > 0) output.Write(buffer, 0, endPos - 2);
						break;
					}

					if (len <= boundaryLen)
						Debug.Log("End boundary not found");
					else
					{
						output.Write(buffer, 0, len - boundaryLen);
						Array.Copy(buffer, len - boundaryLen, buffer, 0, boundaryLen);
						len = input.Read(buffer, boundaryLen, 1024 - boundaryLen) + boundaryLen;
					}
				}
			}

			return tempFile;
		}

		public static string GetBoundary(string type) => "--" + type.Split(';')[1].Split('=')[1];

		private static int IndexOf(Byte[] buffer, int len, Byte[] boundaryBytes)
		{
			for (Int32 i = 0; i <= len - boundaryBytes.Length; i++)
			{
				bool match = true;
				for (Int32 j = 0; j < boundaryBytes.Length && match; j++)
					match = buffer[i + j] == boundaryBytes[j];

				if (match)
					return i;
			}

			return -1;
		}
	}
}