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

1. 📦 Clone or download the package from the release section and import it to your project.

2. To support **RTL TextMeshPro** in your project, you need to add the following scoped registry to your `manifest.json`:

```json
"scopedRegistries": [
  {
    "name": "OpenUPM",
    "url": "https://package.openupm.com",
    "scopes": [
      "com.nosuchstudio"
    ]
  }
]
```

3. 🧭 Add the package to your Unity project:
    - Go to **Window → Package Manager**
    - Click **Add package from disk...**
    - Select the `package.json` file inside `com.mirka.payment`

4. 🧰 After importing, Unity will automatically copy the required `.jar` and `.aar` dependencies into your project when
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

### ⚙️ 3. Configure PaymentKitConfigs and Purchase Buttons

1. **Create PaymentKitConfigs for both stores**
    - Create a `PaymentKitConfig` for **Myket** and **Cafe Bazaar**.
    - Name them **`MyketPaymentKitConfig`** and **`CafeBazaarPaymentKitConfig`**.
    - Set your market keys in each config.
    - Register them as **local Addressables** and ensure that the Addressable names exactly match the config names.

2. **Register callbacks in PaymentWrapper**
    - You can register your `onPurchaseSuccess` and `onPurchaseFailed` actions in the `PaymentWrapper`.

3. **Attach PurchaseButton to shop GameObjects**
    - Add the `PurchaseButton` component to your shop GameObjects.
    - Configure its dependencies, such as payouts, accordingly.

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
Developed by **Sina Karimi** from **Mirka** game studio ❤️ 
