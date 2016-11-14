# Change Log

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