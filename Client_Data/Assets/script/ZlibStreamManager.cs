using UnityEngine;
using System.Collections;

using System;
using Ionic.Zlib;

public class ZlibStreamManager : MonoBehaviour{
	
	static System.IO.MemoryStream StringToMemoryStream(string s){
		byte[] a=System.Text.Encoding.UTF8.GetBytes(s);
		return new System.IO.MemoryStream(a);
	}
	
	static String MemoryStreamToString(System.IO.MemoryStream ms){
		byte[] ByteArray = ms.ToArray();
		return System.Text.Encoding.UTF8.GetString(ByteArray);
	}
	
	static void CopyStream(System.IO.Stream src, System.IO.Stream dest){
		byte[] buffer=new byte[1024];
		int len=src.Read(buffer, 0, buffer.Length);
		while(len > 0){
			dest.Write(buffer, 0, len);
			len=src.Read(buffer, 0, buffer.Length);
		}
		dest.Flush();
	}
	
	void Start(){
		try{
			System.IO.MemoryStream msSinkCompressed;
			System.IO.MemoryStream msSinkDecompressed;
			ZlibStream zOut;
			String originalText="日本語でOK♪";
			Debug.Log("original:" + originalText);
			
			//圧縮
			msSinkCompressed=new System.IO.MemoryStream();
			zOut=new ZlibStream(msSinkCompressed, CompressionMode.Compress, CompressionLevel.BestCompression, true);
			CopyStream(StringToMemoryStream(originalText), zOut);
			zOut.Close();
			
			
			//解凍
			msSinkCompressed.Seek(0, System.IO.SeekOrigin.Begin);
			msSinkDecompressed = new System.IO.MemoryStream();
			zOut=new ZlibStream(msSinkDecompressed, CompressionMode.Decompress, true);
			CopyStream(msSinkCompressed, zOut);
			
			string decompressed=MemoryStreamToString(msSinkDecompressed);
			Debug.Log("decompressed:" + decompressed);
			
			if(originalText == decompressed){
				Debug.Log("成功！");
			}else{
				Debug.Log("失敗。。。");
			}
		}catch (System.Exception e){
			Debug.Log("Exception: " + e);
		}
	}
	
}
