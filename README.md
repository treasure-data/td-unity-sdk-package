TreasureData Unity SDK
===============

Unity SDK for [TreasureData](http://www.treasuredata.com/). With this SDK, you can import the events on your applications into TreasureData easily.

## Installation

Download this [Unity package](https://github.com/treasure-data/td-unity-sdk/blob/master/TD-Unity-SDK.unitypackage) and import it  into your Unity project using `Assets -> Import Package -> Custom Package`.

## Build your project

### For Android

You don't need to do anything special.

### For iOS

After build your project for iOS, you'll be seeing the following errors.

```
"_SecCertificateCreateWithData", referenced from:
  -[TDHttpClient shouldTrustProtectionSpace:] in libTreasureData.a(TDHttpClient.o)
"_SecTrustSetAnchorCertificates", referenced from:
  -[TDHttpClient shouldTrustProtectionSpace:] in libTreasureData.a(TDHttpClient.o)
"_SecTrustEvaluate", referenced from:
  -[TDHttpClient shouldTrustProtectionSpace:] in libTreasureData.a(TDHttpClient.o)
Symbol(s) not found for architecture armv7
Linker command failed with exit code 1 (use -v to see invocation)
```

Please add `Security.framework` to `Link Binary With Librarys` on the `Build Phases` tab.


## Usage

### Instantiate TreasureData object with your API key

```
public class ExampleScript : MonoBehaviour {
	private TreasureData td = null;

	// Use this for initialization
	void Start () {
		td = new TreasureData("your_api_key");
```

or

```
		TreasureData.initializeDefaultApiKey("your_default_api_key");
		TreasureData td = new TreasureData();

```

We recommend to use a write-only API key for the SDK. To obtain one, please:

1. Login into the Treasure Data Console at http://console.treasuredata.com;
2. Visit your Profile page at http://console.treasuredata.com/users/current;
3. Insert your password under the 'API Keys' panel;
4. In the bottom part of the panel, under 'Write-Only API keys', either copy the API key or click on 'Generate New' and copy the new API key.


### Add Events

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

## About Error Code

`TreasureData#addEvent()` and `uploadEvents()` call back `onError` delegate method with `errorCode` argument. This argument is useful to know the cause type of the error. There are the following error codes.

- "init_error"
  - The initialization failed.
- "invalid_param"
  - The parameter passed to the API was invalid
- "invalid_event"
  - The event was invalid
- "data_conversion"
  - Failed to convert the data to/from JSON
- "storage_error"
  - Failed to read/write data in the storage
- "network_error"
  - Failed to communicate with the server due to network problem
- "server_response"
  - The server returned an error response


## Additioanl Configuration

### Endpoint

The API endpoint (default: https://in.treasuredata.com) can be modified using the `InitializeApiEndpoint` API after the TreasureData client constructor has been called and the underlying client initialized. For example:

```
		TreasureData.InitializeApiEndpoint("https://in.treasuredata.com");
		td = new TreasureData(WRITE_APIKEY);
```

### Encryption key

If you've set an encryption key via `TreasureData.InitializeEncryptionKey()`, our SDK saves the event data as encrypted when called `AddEvent`.

```
		TreasureData.InitializeEncryptionKey("hello world");
            :
		td.AddEvent("testdb", "unitytbl", ev);
```
