Treasure Data Unity SDK
===============

Unity SDK for [Treasure Data](http://www.treasuredata.com/). With this SDK, you can import the events on your applications into Treasure Data easily.

## Installation

Download this [Unity package](https://github.com/treasure-data/td-unity-sdk-package/raw/master/TD-Unity-SDK-0.1.10.unitypackage) and import it  into your Unity project using `Assets -> Import Package -> Custom Package`.

### For iOS Application development

On Xcode, the following steps are needed.

* In `Build Phases -> Link Binary With Libraries`, add `libz.tbd`
* In `Build Phases -> Compile Sources`, add `-fno-objc-arc` compile flag to `NativePlugin.mm`


## Usage

### Instantiate TreasureData object with your API key

```
public class ExampleScript : MonoBehaviour {
	private static TreasureData td = null;
	private static Object _lock = new Object();

	// Use this for initialization
	void Start () {
		lock(_lock) {
			if (td == null) {
				td = new TreasureData("your_api_key");
```

or

```
TreasureData.initializeDefaultApiKey("your_api_key");
TreasureData td = new TreasureData();

```

We recommend to use a write-only API key for the SDK. To obtain one, please:

1. Login into the Treasure Data Console at http://console.treasuredata.com;
2. Visit your Profile page at http://console.treasuredata.com/users/current;
3. Insert your password under the 'API Keys' panel;
4. In the bottom part of the panel, under 'Write-Only API keys', either copy the API key or click on 'Generate New' and copy the new API key.


### Add a event to local buffer

To add a event to local buffer, you can call `TreasureData`'s `AddEvent` API.


```
Dictionary<string, object> ev = new Dictionary<string, object>();
ev["str"] = "strstr";
ev["int"] = 12345;
ev["long"] = 12345678912345678;
ev["float"] = 12.345;
ev["double"] = 12.3459832987654;
ev["bool"] = true;
td.AddEvent("testdb", "unitytbl", ev,
    delegate() {
        Debug.LogWarning ("AddEvent Success!!!");
    },
    delegate(string errorCode, string errorMsg) {
        Debug.LogWarning ("AddEvent Error!!! errorCode=" + errorCode + ", errorMsg=" + errorMsg);
    }
);

// Or, simply...
//   td.AddEvent("testdb", "unitytbl", ev);
```

Specify the database and table to which you want to import the events.

### Upload Events to TreasureData

To upload events buffered events to Treasure Data, you can call `TreasureData`'s `UploadEvents` API.

```
// You can call this API to uplaod buffered events whenever you want.
td.UploadEvents (
    delegate() {
        Debug.LogWarning ("UploadEvents Success!!! ");
    },
    delegate(string errorCode, string errorMsg) {
        Debug.LogWarning ("UploadEvents Error!!! errorCode=" + errorCode + ", errorMsg=" + errorMsg);
    }
);

// Or, simply...
//    td.UploadEvents();
```

It depends on the characteristic of your application when to upload and how often to upload buffered events. But we recommend the followings at least as good timings to upload.

- When the current screen is closing or moving to background
- When closing the application

The sent events is going to be buffered for a few minutes before they get imported into Treasure Data storage.

### Retry uploading and deduplication

This SDK imports events in exactly once style with the combination of these features.

- This SDK keeps buffered events with adding unique keys and retries to upload them until confirming the events are uploaded and stored on server side (at least once)
- The server side remembers the unique keys of all events within the past 1 hours by default and prevents duplicated imports (at most once)

As for the deduplication window is 1 hour by default, so it's important not to keep buffered events more than 1 hour to avoid duplicated events.

### Start/End session

When you call `StartGlobalSession` method,  the SDK generates a session that's kept until `EndGlobalSession` is called. The session id will be output as a column name "td_session_id" in TreasureData. Also, you can get the session id with `GetGlobalSessionId`.

```
TreasureData.InitializeDefaultDatabase("testdb");
td = new TreasureData("your_api_key");
print("Session ID = " + TreasureData.GetSessionId());   // >>> (null)

TreasureData.StartGlobalSession();
print("Session ID = " + TreasureData.GetSessionId());   // >>> cad88260-67b4-0242-1329-2650772a66b1
	:
td.AddEvent("testdb", "unitytbl", ev);
	:
TreasureData.EndGlobalSession();
print("Session ID = " + TreasureData.GetSessionId());   // >>> (null)
	:
td.AddEvent("testdb", "unitytbl", ev);
// Outputs =>>
//   [{"td_session_id":"cad88260-67b4-0242-1329-2650772a66b1",
//		 ..., "time":1418880000},
//        :
//    {..., "time":1418880123}
//   ]
```

As long as `StartGlobalSession` has been called but `EndGlobalSession` hasn't been called, the session is continued. Also, if `StartGlobalSession` is called again within 10 seconds of the last calling `EndGlobalSession`, then the session will be resumed, instead of a new session being created.

If you want to use instance level fine grained sessions, you can use `TreasureData#StartSession(tableName)` / `TreasureData#EndSession(tableName)` / `TreasureData#GetSessionId()`, which add an session event at calling `StartSession` / `EndSession` and don't have session resume feature.

### Detect if it's the first running

You can detect if it's the first running or not easily using `IsFirstRun` method and then clear the flag with `ClearFirstRun`.

```
private static TreasureData td = null;
private static Object _lock = new Object();

void Start () {
	lock(_lock) {
		if (td == null) {
			TreasureData.EnableLogging();
			TreasureData.InitializeDefaultDatabase("testdb");

			td = new TreasureData("your_api_key");
			td.EnableAutoAppendUniqId();
			td.EnableAutoAppendModelInformation();
			td.EnableAutoAppendAppInformation();
			td.EnableAutoAppendLocaleInformation();
			TreasureData.StartGlobalSession();

			if (td.IsFirstRun()) {
				td.AddEvent("unitytbl", "installed", true,
					delegate() {
						td.ClearFirstRun();
					},
					delegate(string errorCode, string errorMsg) {
						print ("AddEvent Error!!! : errorCode=" + errorCode + ", errorMsg=" + errorMsg);
					}
				);
				td.UploadEvents ();
			}
		}
	}
}
```


## About error code

`TreasureData#AddEvent()` and `UploadEvents()` call back `onError` delegate method with `errorCode` argument. This argument is useful to know the cause type of the error. There are the following error codes.

- `init_error` :  The initialization failed.
- `invalid_param` : The parameter passed to the API was invalid
- `invalid_event` : The event was invalid
- `data_conversion` : Failed to convert the data to/from JSON
- `storage_error` : Failed to read/write data in the storage
- `network_error` : Failed to communicate with the server due to network problem 
- `server_response` : The server returned an error response



## Additioanl Configuration

### Endpoint

The API endpoint (default: https://in.treasuredata.com) can be modified using the `InitializeApiEndpoint` API after the TreasureData client constructor has been called and the underlying client initialized. For example:

```
TreasureData.InitializeApiEndpoint("https://in.treasuredata.com");
td = new TreasureData("your_api_key");
```

### Encryption key

If you've set an encryption key via `TreasureData.InitializeEncryptionKey()`, our SDK saves the event data as encrypted when called `AddEvent`.

```
TreasureData.InitializeEncryptionKey("hello world");
	:
td.AddEvent("testdb", "unitytbl", ev);
```


### Default database

```
TreasureData.InitializeDefaultDatabase("testdb");
	:
td.AddEvent("unitytbl", ev);
```	

### Adding UUID of the device to each event automatically

UUID of the device will be added to each event automatically if you call `EnableAutoAppendUniqId`. This value won't change until the application is uninstalled.

```
td.EnableAutoAppendUniqId();
	:
td.AddEvent("unitytbl", "name", "foobar");
// Outputs =>>
//   {"td_uuid_id":"cad88260-67b4-0242-1329-2650772a66b1", "name":"foobar", ... }
```

It outputs the value as a column name `td_uuid`.

### Adding an UUID to each event record automatically

UUID will be added to each event record automatically if you call `EnableAutoAppendRecordUUID`. Each event has different UUID.

```
td.EnableAutoAppendRecordUUID();
// If you want to customize the column name, pass it to the API
// td.EnableAutoAppendRecordUUID("my_record_uuid");
	:
td.AddEvent(...);
```

It outputs the value as a column name `record_uuid` by default.


### Adding the device model information to each event automatically

Device model infromation will be added to each event automatically if you call `EnableAutoAppendModelInformation`.

```
td.EnableAutoAppendModelInformation();
	:
td.AddEvent("unitytbl", "name", "foobar");
// Outputs =>>
//   {"td_device":"iPod touch", "name":"foobar", ... }
```

It outputs the following column names and values:

- iOS
	- `td_device` : UIDevice.model
	 - `td_model` : UIDevice.model
	 - `td_os_ver` : UIDevice.model.systemVersion
	 - `td_os_type` : "iOS"
- Android
	- `td_board` : android.os.Build#BOARD
	- `td_brand` : android.os.Build#BRAND
	- `td_device` : android.os.Build#DEVICE
	- `td_display` : android.os.Build#DISPLAY
	- `td_model` : android.os.Build#MODEL
	- `td_os_ver` : android.os.Build.VERSION#SDK_INT
	- `td_os_type` : "Android"

### Adding application package version information to each event automatically

Application package version infromation will be added to each event automatically if you call `EnableAutoAppendAppInformation`.

```
td.EnableAutoAppendAppInformation();
	:
td.AddEvent("unitytbl", "name", "foobar");
// Outputs =>>
//   {"td_app_ver":"1.2.3", "name":"foobar", ... }
```

It outputs the following column names and values:

- iOS
	- `td_app_ver` : Core Foundation key `CFBundleShortVersionString`
	- `td_app_ver_num` : Core Foundation key `CFBundleVersion`
- Android
	- `td_app_ver` : android.content.pm.PackageInfo.versionName (from Context.getPackageManager().getPackageInfo())
	- `td_app_ver_num` : android.content.pm.PackageInfo.versionCode (from Context.getPackageManager().getPackageInfo())

### Adding locale configuration information to each event automatically

Locale configuration infromation will be added to each event automatically if you call `EnableAutoAppendLocaleInformation`.

```
td.EnableAutoAppendLocaleInformation();
	:
td.AddEvent("unitytbl", "name", "foobar");
// Outputs =>>
//   {"td_locale_lang":"en", "name":"foobar", ... }
```

It outputs the following column names and values:

- iOS
	- `td_locale_country` : [[NSLocale currentLocale] objectForKey: NSLocaleCountryCode]
	- `td_locale_lang` : [[NSLocale currentLocale] objectForKey: NSLocaleLanguageCode]
- Android
	- `td_locale_country` : java.util.Locale.getCountry() (from Context.getResources().getConfiguration().locale)
	- `td_locale_lang` : java.util.Locale.getLanguage() (from Context.getResources().getConfiguration().locale)


### Use server side upload timestamp

If you want to use server side upload timestamp not only client device time that is recorded when your application calls `AddEvent`, use `EnableServerSideUploadTimestamp`.

```
// Add server side upload time as a customized column name
td.EnableServerSideUploadTimestamp("server_upload_time");

// If you want to use server side upload times as `time` column,
// call the API without arguments like this.
//
// td.EnableServerSideUploadTimestamp();

```

### Enable/Disable debug log

```
TreasureData.EnableLogging();
```

```
TreasureData.DisableLogging();
```

## Development mode to run application without real devices

This SDK works as an Unity Native Plugin. So you need to run your application with the SDK on a real device especially when you want to run it on iOS platform.

If you want to run your application with the SDK without real devices, you can do that with a special mode for development that emulates the behaviour with a pure C# implementation.

### How to use

#### Configuration

- On PC / iOS / Android / other platforms
  - Add a symbol `TD_SDK_DEV_MODE` to "Player Settings > Scripting Define Symbols"
- On Unity IDE
  - Always enabled. Nothing to do.

#### Modify source code

- Create `SimpleTDClient` instance by calling `SimpleTDClient.Create()` static method in `MonoBehaviour#Start` method
- Attach the instance to `TreasureData` instance

```
public class TreasureDataExampleScript : MonoBehaviour {
    private static TreasureData td = null;
    // private TreasureData tdOnlyInTheScene = null;
			:
	void Start () {
		td = new TreasureData("YOUR_WRITE_APIKEY");
		/* Optional configurations
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
		*/

        // If you want to use a TDClient over scenes, pass `true` to `SimpleTDClient.Create` to prevent it from being removed.
		td.SetSimpleTDClient(SimpleTDClient.Create(true));

        // If you want to use a TDClient only within a scene, don't pass `true` to `SimpleTDClient.Create` so that you can prevent object leaks.
		// tdOnlyInTheScene.SetSimpleTDClient(SimpleTDClient.Create());
			:
```

#### Different behaviours from normal mode

- In development mode,
  - buffered events are stored in memory not in persistent storages.
  - when running into an upload failure, buffered events get lost.
