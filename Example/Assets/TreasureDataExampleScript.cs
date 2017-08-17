using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureDataExampleScript : MonoBehaviour
{
    private static TreasureData td = null;
    private int counter = 0;
    private static Object _lock = new Object ();
    
    // Use this for initialization
    void Start()
    {
        lock (_lock) {
            if (td == null) {
                /* Just for testing. Please ignore these API calls: start */
                TreasureData.InitializeApiEndpoint("https://in.treasuredata.com");
                TreasureData.DisableLogging();
                /* Just for testing. Please ignore these API calls: end */

                TreasureData.EnableLogging();
                // TreasureData.InitializeApiEndpoint("https://anotherapiendpoint.treasuredata.com");
                TreasureData.InitializeEncryptionKey("hello world");
                TreasureData.InitializeDefaultDatabase("testdb");

                td = new TreasureData ("YOUR_WRITE_APIKEY");

                /* For development mode to run application without real devices
                 * See https://github.com/treasure-data/td-unity-sdk-package#development-mode-to-run-application-without-real-devices
                 */
                SimpleTDClient.SetDummyAppVersionNumber("77");
                SimpleTDClient.SetDummyBoard("bravo");
                SimpleTDClient.SetDummyBrand("htc_asia_wwe");
                SimpleTDClient.SetDummyDevice("bravo");
                SimpleTDClient.SetDummyDisplay("ERE27");
                SimpleTDClient.SetDummyModel("HTC Desire");
                SimpleTDClient.SetDummyOsVer("2.1");
                SimpleTDClient.SetDummyOsType("android");
                SimpleTDClient.SetDummyLocaleCountry("JP");
                SimpleTDClient.SetDummyLocaleLang("ja");
                td.SetSimpleTDClient(SimpleTDClient.Create());

                /* Just for testing. Please ignore these API calls: start */
                td.DisableAutoAppendUniqId();
                td.DisableAutoAppendModelInformation();
                td.DisableAutoAppendAppInformation();
                td.DisableAutoAppendLocaleInformation();
                td.DisableServerSideUploadTimestamp();
                td.DisableAutoAppendRecordUUID();
                td.DisableRetryUploading();
                td.EnableRetryUploading();
                td.EnableAutoAppendRecordUUID("test_random_uuid");
                td.EnableServerSideUploadTimestamp();
                td.StartSession("dummy_tbl");
                td.EndSession("dummy_tbl");
                /* Just for testing. Please ignore these API calls: end */

                td.EnableAutoAppendUniqId();
                td.EnableAutoAppendModelInformation();
                td.EnableAutoAppendAppInformation();
                td.EnableAutoAppendLocaleInformation();
                td.EnableAutoAppendRecordUUID();
                td.EnableServerSideUploadTimestamp("server_time");
                print("GetGlobalSessionId() before StartGlobalSession(): " + TreasureData.GetGlobalSessionId());
                TreasureData.StartGlobalSession();
                print("GetGlobalSessionId() after StartGlobalSession(): " + TreasureData.GetGlobalSessionId());
    

                if (td.IsFirstRun()) {
                    td.AddEvent("unitytbl", "installed", true,
                        delegate() {
                            td.ClearFirstRun();
                        },
                        delegate(string errorCode, string errorMsg) {
                            print("AddEvent Error!!! : errorCode=" + errorCode + ", errorMsg=" + errorMsg);
                        }
                    );
                    td.UploadEvents();
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
            return;
        }
    }

    void OnMouseDown()
    {
        if (this.name.Equals("AddEvent")) {
            Dictionary<string, object> ev = new Dictionary<string, object> ();
            ev ["str"] = "strstr";
            ev ["int"] = 12345;
            ev ["long"] = 12345678912345678;
            ev ["float"] = 12.345;
            ev ["double"] = 12.3459832987654;
            ev ["bool"] = true;
            td.AddEvent("unitytbl", ev,
                delegate() {
                    print("AddEvent Success!!! : " + counter);
                },
                delegate(string errorCode, string errorMsg) {
                    print("AddEvent Error!!! : " + counter + ", errorCode=" + errorCode + ", errorMsg=" + errorMsg);
                }
            );
            td.AddEvent("unitytbl", "another_event", "foobar");
            counter++;
        }
        else if (this.name.Equals("UploadEvents")) {
            print("GetGlobalSessionId() before EndGlobalSession(): " + TreasureData.GetGlobalSessionId());
            TreasureData.EndGlobalSession();
            print("GetGlobalSessionId() after EndGlobalSession(): " + TreasureData.GetGlobalSessionId());
            td.AddEvent("unitytbl", "event_type", "upload");
            td.UploadEvents(
                delegate() {
                    print("UploadEvents Success!!! : " + counter);
                },
                delegate(string errorCode, string errorMsg) {
                    print("UploadEvents Error!!! : " + counter + ", errorCode=" + errorCode + ", errorMsg=" + errorMsg);
                }
            );
            counter++;
        }
    }
}
