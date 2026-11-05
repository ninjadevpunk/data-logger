# Data Logger
![.NET](https://img.shields.io/badge/.NET-8.0-blue)![C#](https://img.shields.io/badge/C%23-10.0-green)

## Description

Data Logger is a desktop application that records and tracks various types of data, stores it in a database, and allows you to search and analyze it efficiently.

## Features

Log and store data entries with timestamps.

Search and filter data records.

Uses a local SQLite database stored in `C:\Data Logger Central`.

## Usage
Assuming you're using alpha4 please open the app and sign up. The app will take you to the dashboard. Navigate to the coding log dashboard (Qt and Android Studio disabled). Click on `Create Log` on the right and create a log. Enter the coding project's name, the application you're using, leave the set start time and then enter relevant details such as output before adding a sticky note/post-it in the post-it editor. Click `CURRENT` to update end time before clicking annotate and the log will be cached for 20 minutes before being stored. The log is viewable and editable by right-clicking the cached log. To view the log once it's stored please navigate to the report dashboard by clicking report. Your log will appear there if it has been stored. Use the drop-down filters if you need to find the log. Once it is found, right-click to view/edit or delete it from the database.

## Installation

Download the latest published zip from the official site.

Extract the folder anywhere on your system.

Run `Data Logger 1.3.exe`. The app will request permission to create the SQLite database and its folder.


Optionally, you can open the project in Visual Studio (requires .NET 8) and run from there.


## Tech Stack & Dependencies

- Entity Framework

- Newtonsoft.JSON (JSON serialization)

- Extended.Wpf.Toolkit

- Svg.Skia

- SyncFusion.PDF
