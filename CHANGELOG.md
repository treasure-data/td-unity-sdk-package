# Change Log

## Version 0.1.12
_2018-03-29_

* Support automatically tracking of lifecycle events

## Version 0.1.11
_2017-08-23_

* Support `withDontDestroyOnLoad` mode on SimpleTDClient.Create so that a SimpleTDClient instance isn't removed when moving over scenes

## Version 0.1.10
_2017-08-07_

* Add `Development mode` to run application without real devices. See https://github.com/treasure-data/td-unity-sdk-package/blob/master/README.md#development-mode-to-run-application-without-real-devices

## Version 0.1.9
_2017-03-14_

* Rename TreasureData.StartSession() to TreasureData.StartGlobalSession()
* Rename TreasureData.EndSession() to TreasureData.EndGlobalSession()
* Add TreasureData.GetGlobalSessionId()
* Add TreasureData#GetSessionId()
* Fix memory leak introduced by `-fno-objc-arc` compile option on NativePlugin.mm

## Version 0.1.8
_2017-03-01_

* Fix TreasureData#EnableRetryUploading() for Android platform 


## Version 0.1.7
_2016-11-22_

* Add the following APIs
	* TreasureData#EnableAutoAppendRecordUUID() and DisableAutoAppendRecordUUID()
	* TreasureData#EnableServerSideUploadTimestamp() and DisableServerSideUploadTimestamp()


## Version 0.1.6
_2016-11-15_

* Upgrade internal td-android-sdk to 0.1.14 from 0.1.13. See details on https://github.com/treasure-data/td-android-sdk/blob/master/CHANGELOG.md
* Upgrade internal td-ios-sdk to 0.1.21 from 0.1.17. See details on https://github.com/treasure-data/td-ios-sdk/blob/master/CHANGELOG.md
* Add the following bug fixes
	* For iOS
		* Fix crash that can't be usually reproducd, but can be happened according to some crash report stats. This crash can occur only in iOS 10. In case of iOS 9 or less, this issue can cause a deadlock
		* Fix compile errors with Xcode8


## Version 0.1.5
_2016-06-28_

* Upgrade internal td-android-sdk to 0.1.13 from 0.1.6. See details on https://github.com/treasure-data/td-android-sdk/blob/master/CHANGELOG.md
* Upgrade internal td-ios-sdk to 0.1.17 from 0.1.6. See details on https://github.com/treasure-data/td-ios-sdk/blob/master/CHANGELOG.md
* Add the following APIs
	* TreasureData#EnableAutoAppendAppInformation() and DisableAutoAppendAppInformation()
	* TreasureData#EnableAutoAppendLocaleInformation() and DisableAutoAppendLocaleInformation()
	* TreasureData#StartSession() and EndSession()
* Add the following bug fixes
	* For Android
		* Fix the bug that can cause a failure of sending HTTP request
	* For iOS
		* Fix crash that happens when Data Protection is enabled and API is called 10 seconds after iOS is locked
		* Fix crash that occurs when handling invalid database or table name
		* Fix bug TreasureData#uploadEventsWithCallback doesn't call callbacks

## Version 0.1.4
_2015-04-03_

* Fix an error "the return type of CallObjectMethodA does not match" on Android 5

## Version 0.1.3
_2014-12-25_

* Remove an implemented SSL cert. It's important to use this version or later
* Enable to automatically append these information to each event:
	* device ID (UUID)
	* session ID
	* OS/device model information

## Version 0.1.2
_2014-09-19_

* Fix the crash when calling TreasureData.uploadEvents without any param on iOS
* Add example of collecting an installation event

## Version 0.1.1
_2014-08-08_

* Fix some bugs related to encryption

## Version 0.1.0
_2014-08-06_

* Initial release
