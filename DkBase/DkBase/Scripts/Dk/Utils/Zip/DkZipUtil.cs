//using Ionic.Zlib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using UnityEngine;
using System;

namespace Dk.Util
{
	public class DkZipUtil
	{
		public static byte[] GZip(byte[] data)
		{
			byte[] bytes = null;

			using(MemoryStream outstream = new MemoryStream())
			{
				using( GZipOutputStream encoder = new GZipOutputStream( outstream ) )
				{
					encoder.Write(data,0,data.Length);
				}

				bytes = outstream.ToArray();
			}
			return bytes;
		}
						
		public static byte[] unGZip(byte[] data )
		{
			byte[] bytes = null;

			using(MemoryStream inputStream = new MemoryStream())
			{
				using(GZipInputStream decoder = new GZipInputStream(new MemoryStream(data)))
				{
					CopyStream( decoder, inputStream );
				}

				bytes = inputStream.ToArray();
			}

			return bytes;
		}

		public static byte[] Zip(List<byte[]> byteList, List<string> pathList)
		{

			if(byteList == null || pathList == null || byteList.Count == 0)
			{
				return null;
			}
			else if(byteList.Count != pathList.Count)
			{
				Debug.Log("[error][Zip] : byteList count not the same with path list");
				return null;
			}


			byte[] outBytes = null;
			using(MemoryStream outstream = new MemoryStream())
			{
				using(ZipOutputStream encoder = new ZipOutputStream(outstream))
				{
					for(int i = 0 ; i < byteList.Count ; i++)
					{
						byte[] bytes = byteList[i];
						string path	= pathList[i];
	
						ZipEntry entry = new ZipEntry(path);
						entry.DateTime = DateTime.Now;
						entry.Size = bytes.Length;
						
						encoder.PutNextEntry(entry);
						encoder.SetLevel(6);
						encoder.Write(bytes,0,bytes.Length);
					}
				}

				outBytes = outstream.ToArray();
			}

			return outBytes;
		}

		public static void unZip( byte[] data , out List<byte[]> byteList , out List<string> pathList)
		{
			byteList = new List<byte[]>();
			pathList = new List<string>();

			if(data == null)
			{
				Debug.Log("[error] unzip data is null !");
				return;
			}

//			using(MemoryStream stream = new MemoryStream(data))
//			{
//				ZipFile zf = new ZipFile(stream);
//				foreach (ZipEntry zipEntry in zf) 
//				{
//					if (!zipEntry.IsFile) 
//					{
//						continue;          
//					}
//					String entryFileName = zipEntry.Name;
//					using(Stream zipStream = zf.GetInputStream(zipEntry))
//					{
//						using(MemoryStream outstream = new MemoryStream())
//						{		
//							CopyStream( zipStream, outstream );	
//							byte[] bytes = outstream.ToArray();
//							byteList.Add(bytes);
//							pathList.Add(entryFileName);
//						}
//					}
//				}
//			}

			using(ZipInputStream decoder = new ZipInputStream(new MemoryStream(data)))
			{
				ZipEntry entry;
				while((entry = decoder.GetNextEntry()) != null)
				{
					string pathToZip = entry.Name;

					if(!string.IsNullOrEmpty(pathToZip))
					{
						using(MemoryStream outstream = new MemoryStream())
						{
							CopyStream( decoder, outstream );	
							byte[] bytes = outstream.ToArray();
							byteList.Add(bytes);
							pathList.Add(pathToZip);
						}
					}
				}
				//Debug.Log("unzip item count : "+count);
			}
		}
		
		private static void CopyStream( Stream input, Stream output )
		{
			int size = 2048;
			byte[] buffer = new byte[size];
			//long TempPos = input.Position;
			while( true )
			{
				size = input.Read( buffer, 0, size );
				if( size <= 0 ) break;
				output.Write( buffer, 0, size );
			}
			//input.Position = TempPos;// or you make Position = 0 to set it at the start 
		}
	}
}
