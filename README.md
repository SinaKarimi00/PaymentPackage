# 💳 Mirka Payment Package Documentation

## 📚 Table of Contents

- [✨ Introduction](#-introduction)
- [⚙️ Installation](#%EF%B8%8F-installation)
- [🧩 Supported Platforms](#-supported-platforms)
- [📱 Android Setup Guide](#-android-setup-guide)
- [🧰 Troubleshooting](#-troubleshooting)
- [🏁 Final Note](#-final-note)

---

## ✨ Introduction

**Mirka Payment Package** is a Unity plugin that provides a unified interface for handling **in-app purchases** through
both **Cafe Bazaar** ☕ and **Myket** 🛍️ stores.  
It abstracts away all the complex implementation details and allows developers to integrate billing with just a few
simple API calls.

---

## ⚙️ Installation

0. You should import [RTl Text Mesh Pro](https://github.com/pnarimani/RTLTMPro/tree/master) to your project.
1. 📦 Clone or download the package from the release section and place the package folder inside your project's Packages
   folder, next to the manifest.json file.  <br>
   For more information, visit [Release](https://github.com/SinaKarimi00/PaymentPackage/releases).
2. To support **RTL TextMeshPro** in your project, you need to add the following scoped registry to your
   `manifest.json`:

```json
"scopedRegistries": [
{
"name": "OpenUPM",
"url": "https://package.openupm.com",
"scopes": [
"com.nosuchstudio"]}]
```

3. 🧰 After importing, Unity will automatically copy the required `.jar` and `.aar` dependencies into your project when
   you build.

---

## 🧩 Supported Platforms

| Platform     | Status | Notes                                             |
|--------------|--------|---------------------------------------------------|
| **Android**  | ✅      | Supports both Cafe Bazaar ☕ and Myket 🛍️ billing |
| **iOS**      | ❌      | Not supported yet                                 |
| **PC/WebGL** | ❌      | Not supported yet                                 |

---

## 📱 Android Setup Guide

To enable in-app billing for **Myket** 🛍️ and **Cafe Bazaar** ☕, follow the steps below carefully:

---

### 🛍️ 1. Myket Configuration

Add the following lines to your `AndroidManifest.xml` file:

#### 🔹 Activity

```xml

<activity android:name="com.myket.MyketIABProxyActivity"
          android:theme="@android:style/Theme.Translucent.NoTitleBar.Fullscreen"/>

```

#### 🔹 Permissions

```xml

<uses-permission android:name="ir.mservices.market.BILLING"/>
```

#### 🔹 Receiver

```xml

<receiver android:name="com.myket.util.IABReceiver" android:exported="true">
    <intent-filter>
        <action android:name="ir.mservices.market.ping"/>
        <action android:name="ir.mservices.market.purchase"/>
        <action android:name="ir.mservices.market.getPurchase"/>
        <action android:name="ir.mservices.market.billingSupport"/>
        <action android:name="ir.mservices.market.skuDetail"/>
        <action android:name="ir.mservices.market.consume"/>
    </intent-filter>
</receiver>
```

#### 🔹 Queries (Optional, Recommended)

If your app targets **Android 11 (API 30)** or higher, add the following inside the `<manifest>` tag:

```xml

<queries>
    <package android:name="ir.mservices.market"/>
    <intent>
        <action android:name="ir.mservices.market.InAppBillingService.BIND"/>
        <data android:mimeType="*/*"/>
    </intent>
</queries>
```

---

#### Sample Android Manifest

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android" xmlns:tools="http://schemas.android.com/tools"
          package="your-package-name" android:versionCode="1" android:versionName="1.0">
    <uses-permission android:name="android.permission.INTERNET"/>
    <uses-permission android:name="ir.mservices.market.BILLING"/>
    <queries>
        <package android:name="ir.mservices.market"/>
        <intent>
            <action android:name="ir.mservices.market.InAppBillingService.BIND"/>
            <data android:mimeType="*/*"/>
        </intent>
    </queries>
    <application android:label="@string/app_name" android:icon="@drawable/app_icon">
        <!-- The MessagingUnityPlayerActivity is a class that extends
             UnityPlayerActivity to work around a known issue when receiving
             notification data payloads in the background. -->
        <activity android:name="com.myket.MyketIABProxyActivity"
                  android:theme="@android:style/Theme.Translucent.NoTitleBar.Fullscreen"/>
        <receiver android:name="com.myket.util.IABReceiver" android:exported="true">
            <intent-filter>
                <action android:name="ir.mservices.market.ping"/>
                <action android:name="ir.mservices.market.purchase"/>
                <action android:name="ir.mservices.market.getPurchase"/>
                <action android:name="ir.mservices.market.billingSupport"/>
                <action android:name="ir.mservices.market.skuDetail"/>
                <action android:name="ir.mservices.market.consume"/>
            </intent-filter>
        </receiver>
        <activity android:name="com.google.firebase.MessagingUnityPlayerActivity"
                  android:theme="@style/UnityThemeSelector"
                  android:configChanges="fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen">
            <intent-filter>
                <action android:name="android.intent.action.MAIN"/>
                <category android:name="android.intent.category.LAUNCHER"/>
            </intent-filter>
            <meta-data android:name="unityplayer.UnityActivity" android:value="true"/>
        </activity>
        <service android:name="com.google.firebase.messaging.MessageForwardingService"
                 android:permission="android.permission.BIND_JOB_SERVICE" android:exported="true">
        </service>
    </application>
</manifest>

```

### ☕ 2. Cafe Bazaar Configuration

Add the **Poolakey** billing library to your Gradle dependencies.

🧩 Open your `mainTemplate.gradle` file (enable `Custom Gradle Template` under  
`Player Settings > Publishing Settings > Build`) and add the following inside the `dependencies` block:

```gradle
implementation 'com.github.cafebazaar.Poolakey:poolakey:2.0.0' // Required for Cafe Bazaar billing
```

> 📁 **Reference:**  
> This dependency is automatically linked by  
> `Assets/Bazaar/Poolakey/Scripts/Editor/CafeBazaarPlugin_Dependencies.xml`

---

### ✅ Summary

| Platform          | Required Changes                                                                |
|-------------------|---------------------------------------------------------------------------------|
| **Myket** 🛍️     | Add `<uses-permission>`, `<receiver>`, and `<queries>` to `AndroidManifest.xml` |
| **Cafe Bazaar** ☕ | Add Poolakey dependency to `mainTemplate.gradle`                                |

---

> 💡 **Tip:**  
> Always rebuild your Gradle project after making these changes.  
> You can verify integration success by checking the Unity logs during payment initialization.

---

### ⚙️ 3. Configure PaymentKitConfigs and Purchase Buttons in Unity

This guide explains how to properly set up **PaymentKit** inside a Unity project.

---

### 1) Add the Prefab to the Scene

First, drag and drop the **PaymentWrapper** prefab from the **Assets** folder into your **Scene**.

> **Recommendation:** Make sure **PaymentWrapper** exists only once in the Scene (avoid duplicates).

---

### 2) Create the Config ScriptableObjects

Create two **ScriptableObject** config files from the following menu path:

- `Create → Payment → PaymentKitConfig`

Then, assign the store keys inside each config:

- `PaymentKitConfig (Myket)` → insert the **Myket** key
- `PaymentKitConfig (Bazaar)` → insert the **Bazaar** key

---

### 3) Subscribe to Purchase Success Callback

To correctly detect **successful purchases** and receive the purchase callback, you must subscribe to the purchase
success event:

```csharp
PaymentContainer.PaymentWrapper.RegisterOnPurchaseSuccess(OnPurchaseSuccess);
```

### Example Handler Function

```csharp
private void OnPurchaseSuccess(string productId)
{
    // Your logic after a successful purchase
}
```

> **Note:** It is recommended to register this callback inside `Start()` or right after `PaymentWrapper`
> initialization.  
> If needed, unregister it inside `OnDestroy()` to prevent unwanted callbacks.

---

### 4) Connect and Initialize the PurchaseButton

The **PurchaseButton** component must be attached to your purchase UI button (or linked to it) and must be properly
initialized.

### Checklist

Make sure the following is correctly set up:

- **PurchaseButton** is attached to the correct **UI Button**
- The **ProductId / SKU** value is correct (if such a field exists)
- **PurchaseButton** is initialized before the user clicks the button
- If **PurchaseButton** has an `Init(...)` method, call it after **PaymentWrapper** and config objects are ready

> 💡 **Tip:**  
> You can use the **PaymentSwitcherTool API** in your build tools. An example usage is available in the **Sample**
> directory.

---

## 🧰 Troubleshooting

| ⚠️ Issue                            | 💭 Possible Cause        | 🛠️ Solution                                                          |
|-------------------------------------|--------------------------|-----------------------------------------------------------------------|
| Build fails with missing dependency | Gradle not updated       | Ensure `mainTemplate.gradle` includes Poolakey dependency             |
| Payment not working on device       | Missing Myket permission | Add `ir.mservices.market.BILLING` permission to `AndroidManifest.xml` |
| Unity build error                   | Outdated Gradle version  | Update Gradle and Android SDK tools                                   |

---

## 🏁 Final Note

Good luck with your game! 🎉 <br>
Developed by **Sina Karimi** ❤️ 
