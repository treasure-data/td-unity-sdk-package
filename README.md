TreasureData Unity SDK
===============

Unity SDK for [TreasureData](http://www.treasuredata.com/). With this SDK, you can import the events on your applications into TreasureData easily.

## Installation

Download this [Unity package](https://github.com/treasure-data/td-unity-sdk-package/blob/master/TD-Unity-SDK-0.1.4.unitypackage) and import it  into your Unity project using `Assets -> Import Package -> Custom Package`.



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


### Add events to local buffer

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
```
Or, simply (without callbacks)

```
		td.AddEvent("testdb", "unitytbl", ev);
```

Specify the database and table to which you want to import the events.

### Upload Events to TreasureData


```
        td.UploadEvents (
            delegate() {
                Debug.LogWarning ("UploadEvents Success!!! ");
            },
            delegate(string errorCode, string errorMsg) {
                Debug.LogWarning ("UploadEvents Error!!! errorCode=" + errorCode + ", errorMsg=" + errorMsg);
            }
        );
```
Or, simply (without callbacks)

```
        td.UploadEvents();
```

The sent events is going to be buffered for a few minutes before they get imported into TreasureData storage.

### Start/End session

When you call `StartSession` method,  the SDK generates a session ID that's kept until `EndSession` is called. The session id is outputs as a column name "td_session_id". Also, `StartSession` and `EndSession` methods add an event that includes `{"td_session_event":"start" or "end"}`.

```
		TreasureData.InitializeDefaultDatabase("testdb");
		td = new TreasureData("your_api_key");
		td.StartSession("unitytbl");
			:
		td.EndSession("unitytbl");
		// Outputs =>>
		//   [{"td_session_id":"cad88260-67b4-0242-1329-2650772a66b1",
		//		"td_session_event":"start", "time":1418880000},
		//        :
		//    {"td_session_id":"cad88260-67b4-0242-1329-2650772a66b1",
		//		"td_session_event":"end", "time":1418880123}
		//    ]
```

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
				td.StartSession("unitytbl");

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
