using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureDataExampleScript : MonoBehaviour {
	private TreasureData td = null;
	private int counter = 0;
	
	// Use this for initialization
	void Start () {
		TreasureData.EnableLogging();
		// TreasureData.InitializeApiEndpoint("https://anotherapiendpoint.treasuredata.com");
		// TreasureData.InitializeEncryptionKey("hello world");
		td = new TreasureData("YOUR_WRITE_APIKEY");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
			return;
		}
	}

	void OnMouseDown() {
		if (this.name.Equals("AddEvent")) {
			Dictionary<string, object> ev = new Dictionary<string, object>();
			ev["str"] = "strstr";
			ev["int"] = 12345;
			ev["long"] = 12345678912345678;
			ev["float"] = 12.345;
			ev["double"] = 12.3459832987654;
			ev["bool"] = true;
			td.AddEvent("testdb", "unitytbl", ev,
			            delegate() {
				print ("AddEvent Success!!! : " + counter);
			},
			delegate(string errorCode, string errorMsg) {
				print ("AddEvent Error!!! : " + counter + ", errorCode=" + errorCode + ", errorMsg=" + errorMsg);
			}
			);
			counter++;
		}
		else if (this.name.Equals("UploadEvents")) {
			td.UploadEvents (
				delegate() {
				print ("UploadEvents Success!!! : " + counter);
			},
			delegate(string errorCode, string errorMsg) {
				print ("UploadEvents Error!!! : " + counter + ", errorCode=" + errorCode + ", errorMsg=" + errorMsg);
			}
			);
			counter++;
		}
	}
}
