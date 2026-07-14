# ShelteredSaveEditor

**Features:**
- **Inventory Editor**: Displays Item Names, type IDs, and descriptions fetched directly from `Sheltered - Item #.csv`. Includes an **Item Search Bar** to quickly filter items by name, ID, or description.
- **Input Validation**: Safely restricts boolean inputs to a dropdown list (True/False) and locks numeric fields to valid numbers only, preventing file corruption.
- **World Info Editor**: Edit game difficulty, fog settings, and other details.
- **Family Members Editor**: Customize first/last names, stats, behaviors, and illnesses.
- **Tree Editor**: Access and modify the raw decrypted XML structure.
- **Special Cheats**: Display the mystery hatch password and toggle its status.

**Warning**
- This is not compatible with Sheltered 2. For Sheltered 2, use: [mjra/Sheltered-2-SE (github.com)](https://github.com/mjra/Sheltered-2-SE)
- **ALWAYS BACKUP YOUR SAVE FILE FIRST!** Never edit your file without keeping a backup.

---

## Compilation Instructions

The project is built on **.NET Framework v4.6.1**.

### 1. Build via Visual Studio
* Open `ShelteredSE.sln` or `ShelteredSE.csproj` in Visual Studio.
* Select the **Debug** or **Release** configuration.
* Press **F5** or click **Start** to build and run the application.

### 2. Build via CLI (MSBuild)
If you have the .NET Framework developer pack installed, you can build from PowerShell:
```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\amd64\MSBuild.exe" /t:Rebuild /p:Configuration=Debug
```

If you do not have the targeting pack installed and get targeting errors (like `MSB3644`), override the framework path to compile against the installed local runtime:
```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\amd64\MSBuild.exe" /t:Rebuild /p:Configuration=Debug /p:FrameworkPathOverride="C:\Windows\Microsoft.NET\Framework64\v4.0.30319"
```

Once built, the executable is generated at `bin\Debug\ShelteredSE.exe` along with necessary CSV/XML resources.

---

**Usage:**
* Run `ShelteredSE.exe`.
* Click **Open Save File**, choose your `.dat` save file, and start editing!

