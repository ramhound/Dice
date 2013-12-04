using System.Threading;
using UnityEngine;
using System;
using Launchie;

public class LaunchieLogic : MonoBehaviour
{
	// location of patches
	public string url;
	
	// current version of game
	public string version;
	
	double _progress = 0;
	Launchie.Launchie _l;
	LaunchieGUI lgui;
	
	System.Threading.ManualResetEvent errorEvent = new System.Threading.ManualResetEvent(false);
	Exception errorException = null;
	
	void Start ()
	{
		if (Application.isEditor) {
			Debug.LogWarning ("Launchie mustn't run from editor!");
			return;
		}
		
		if ( url == null || url == "" || version == null || version == "" ) {
			Debug.LogWarning ("Launchie `url` and `version` cannot be empty!");
			return;
		}
		
		lgui = (LaunchieGUI)GetComponent("LaunchieGUI");
		
		new System.Threading.Thread (Asyncwork).Start();
	}
	
	void Asyncwork ()
	{
		_l = new Launchie.Launchie (url, version);
		_l.setOnError( OnError );
		
		int check_state = _l.Check ();
		
		if (check_state == 1)
		{
			lgui.setState(1);
			lgui.setText( "There is update: " + _l.getAvailableVersion() );
			_l.setOnDone( DownloadReleaseNotesDone );
			_l.DownloadReleaseNotes ();
		}
		else if (check_state == 0)
		{
			lgui.setState(2);
			lgui.setText( "Game is up to date :)" );
			// there are no updates and you can load your game levels
			// here you can add something like this
			// Application.Loadlevel(1);
		}
		
		errorEvent.WaitOne ();
		
		if (errorException != null) {
			// handle any errors
		}
	}
	
	void DownloadReleaseNotesDone ()
	{
		lgui.setText( "There is update: " + _l.getAvailableVersion() + "\n" + _l.getReleaseNotes() );
	}
	
	public void DownloadPatch ()
	{
		if (lgui.getState() == 1) {
			_l.setOnProgress( DownloadPatchProgress );
			_l.setOnDone( DownloadPatchDone );
			
			lgui.setState(3);
			lgui.setText( "Downloading " + _l.getAvailableVersion() + "\n" + _l.getReleaseNotes() );
			_l.Download ();
		}
	}
	
	void DownloadPatchDone ()
	{
		_progress = 0;
		lgui.setProgress(_progress);
		_l.setOnProgress( ExtractProgress );
		_l.setOnDone( ExtractDone );
		
			
		lgui.setState(4);
		lgui.setText( "Unpacking " + _l.getAvailableVersion() + "\n" + _l.getReleaseNotes() );
		_l.Extract ();
	}
	
	void DownloadPatchProgress ( double progress )
	{
		lgui.setProgress(progress);
	}
	
	void ExtractProgress ( double progress )
	{
		lgui.setProgress(progress);
	}
	
	void ExtractDone ()
	{
		_l.Finish ();
		lgui.setState(5);
		lgui.setText( "Download complete!\nPlease restart game.");
	}
	
	void OnError (Exception ex)
	{
		errorException = ex;
		errorEvent.Set ();
	}
}