# 131-Nguyen Dang Man-21CNTT4

## How to Install and Run

- Download unity here: https://unity.com/download (recommended version: Unity 2022.3.54f1 LTS)
- Download .NET SDK (if building outside Unity)

---

## After Unity Editor installed

📦 Step1: Unzip project

- Download and Unzip 131-Nguyễn Đăng Mãn-21CNTT4.zip project file into any folder of your choice

  🛠️ Step 2: Open Unity

- Open Unity Hub, press button "Add" and click " Add project from disk"
- Select the unzipped folder (double click)

  ▶️ Step 3: Setup Netcode

- Click "window" on taskbar Unity Hub, select "Package Manager"
- Search Netcode for GameObjects and install ( install 1.2.0 version )
  - If you don't see Netcode for GameObjects version 1.2.0
  - Click plus icon on the Package Manager and add package by name
  - And type the exact name " com.unity.netcode.gameobjects "
  - And then over here for version, let type " 1.2.0 " and click "Add" button
- Open MainMenuScene and run

  🌐 Step 4: If you play Mutilplayergame

- Run two instances of the game on the same or different machines. One player selects Host, the other selects Client.
- If on different machines, make sure both are on the same LAN

---

📦 Optional: Build Standalone

- File > Build Settings
- Arrange the necessary scenes (MainMenuScene, LobbyScene, GameScene, LoadingScene/CharacterSelectScene)
- Choose platform (Windows/Mac/Linux)
- Click Build
