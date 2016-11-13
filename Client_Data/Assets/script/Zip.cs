using UnityEngine;
using System.IO; 
using ICSharpCode.SharpZipLib.GZip;

//----------------------------------------------------------
// Zip
//----------------------------------------------------------
using System;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;


public class Zip:MonoBehaviour
{
	public static byte [] ZipFromFile(string filePath) {
		using (var fileStreamIn = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
			using (var memoryStreamOut = new MemoryStream()) {
				using (ZipOutputStream zipOutStream = new ZipOutputStream(memoryStreamOut)) {
					byte[] buffer = new byte[4096];
					
					// get zipped-file name from filePath.
					var name = Path.GetFileName(filePath);
					
					ZipEntry entry = new ZipEntry(name);
					entry.DateTime = DateTime.Now;
					zipOutStream.PutNextEntry(entry);
					
					int size;
					do {
						size = fileStreamIn.Read(buffer, 0, buffer.Length);
						zipOutStream.Write(buffer, 0, size);
					} while (size > 0);
					
					zipOutStream.Finish();
					
					zipOutStream.Close();
				}
				return memoryStreamOut.ToArray();
			}
		}
	}

	byte[] ReadFile(){
		Debug.Log ("ReadFile1");
		System.IO.FileStream fs = new System.IO.FileStream(
			@"/Users/user/Library/Application Support/DefaultCompany/Onseifile/data.zip",
			System.IO.FileMode.Open,
			System.IO.FileAccess.Read);
		//ファイルを読み込むバイト型配列を作成する
		byte[] bs = new byte[fs.Length];
		//ファイルの内容をすべて読み込む
		fs.Read(bs, 0, bs.Length);
		//閉じる
		fs.Close();
		Debug.Log ("ReadFile2");
		return bs;
	}

	const string dataPath = "/Users/user/Library/Application Support/DefaultCompany/Onseifile/data.zip";
	bool isLoading=true;

	//Button1のClickイベントハンドラ
	public void OnZip(/*object sender, EventArgs e*/)
	{
		isLoading = true;
		//作成するZIP書庫のパス
		string zipFileName = @"/Users/user/Library/Application Support/DefaultCompany/Onseifile/data.zip";
		//圧縮するフォルダのパス
		string sourceDirectory = @"/Users/user/Library/Application Support/DefaultCompany/Onseifile";
		//サブディレクトリも圧縮するかどうか
		bool recurse = true;
		
		//FastZipEventsの作成
		ICSharpCode.SharpZipLib.Zip.FastZipEvents fastZipEvents =
			new ICSharpCode.SharpZipLib.Zip.FastZipEvents();
		fastZipEvents.CompletedFile =
			new ICSharpCode.SharpZipLib.Core.CompletedFileHandler(CompletedFile);
		fastZipEvents.DirectoryFailure =
			new ICSharpCode.SharpZipLib.Core.DirectoryFailureHandler(DirectoryFailure);
		fastZipEvents.FileFailure =
			new ICSharpCode.SharpZipLib.Core.FileFailureHandler(FileFailure);
		fastZipEvents.ProcessDirectory =
			new ICSharpCode.SharpZipLib.Core.ProcessDirectoryHandler(ProcessDirectory);
		fastZipEvents.ProcessFile =
			new ICSharpCode.SharpZipLib.Core.ProcessFileHandler(ProcessFile);
		fastZipEvents.Progress =
			new ICSharpCode.SharpZipLib.Core.ProgressHandler(Progress);
		
		//FastZipオブジェクトの作成
		ICSharpCode.SharpZipLib.Zip.FastZip fastZip =
			new ICSharpCode.SharpZipLib.Zip.FastZip(fastZipEvents);
		//空のフォルダも書庫に入れるか
		fastZip.CreateEmptyDirectories = true;
		
		//圧縮してZIP書庫を作成
		fastZip.CreateZip(zipFileName, sourceDirectory, recurse, null, null);
		//tiensyori
		StartCoroutine(SendData());

	}
	
	//1つのファイルの圧縮、展開を始める時
	private void ProcessFile(System.Object sender,
	                         ICSharpCode.SharpZipLib.Core.ScanEventArgs e)
	{
		Console.WriteLine("\"{0}\"の処理を開始", e.Name);
	}
	
	//1つのファイルの圧縮、展開の進行状況
	private void Progress(System.Object sender,
	                      ICSharpCode.SharpZipLib.Core.ProgressEventArgs e)
	{
		Console.WriteLine("{0}%({1}/{2})",
		                  e.PercentComplete, e.Processed, e.Target);
	}
	
	//1つのファイルの圧縮、展開が完了した時
	private void CompletedFile(System.Object sender,
	                           ICSharpCode.SharpZipLib.Core.ScanEventArgs e)
	{
		Debug.Log ("処理が完了"+e.Name);
		isLoading = false;
		//Console.WriteLine("\"{0}\"の処理が完了", e.Name);
	}
	
	//1つのフォルダの圧縮、展開が完了した時
	private void ProcessDirectory(System.Object sender,
	                              ICSharpCode.SharpZipLib.Core.DirectoryEventArgs e)
	{
		Debug.Log ("フォルダ処理が完了"+e.Name);
		//Console.WriteLine("フォルダ\"{0}\"の処理を開始", e.Name);
		
	}
	
	//ファイルの圧縮、展開でエラーが発生した時
	private void FileFailure(System.Object sender,
	                         ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs e)
	{
		Console.WriteLine("\"{0}\"の処理中にエラー({1})が発生",
		                  e.Name, e.Exception.Message);
		
		//e.ContinueRunning = false;
		//とすると、以降の処理をキャンセル
	}
	
	//フォルダの圧縮、展開でエラーが発生した時
	private void DirectoryFailure(System.Object sender,
	                              ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs e)
	{
		Console.WriteLine("フォルダ\"{0}\"の処理中にエラー({1})が発生",
		                  e.Name, e.Exception.Message);
	}

	private string path = "http://example.com/";
	
	// Use this for initialization
	IEnumerator SendData (/*byte[] data*/) {
		Debug.Log ("SendData");
		while (isLoading) {
			yield return new WaitForSeconds (0.5f);
		}
		Debug.Log ("SendData2");
		var data = ReadFile();
		using(WWW www = new WWW(path,data)){
			
			yield return www;
			
			if(!string.IsNullOrEmpty(www.error)){
				
				Debug.LogError("www Error:" + www.error);
				yield break;
				
			}
			
			Debug.Log(www.text);
			
		}
	}


}