![515901-200](https://github.com/user-attachments/assets/8d35eb74-f195-4117-ad83-06e2a83b92c4)

**Maui.ComboBox** ⬇️

A custom ComboBox control specifically designed for .NET MAUI, optimized for Android applications. <br>
This control brings the familiar ComboBox functionality to your MAUI projects.

 Platform  | Supported |
| ------------- | ------------- |
| iOS  |❌|
| Android  |✅|
| Mac Catalyst	  |❌|
| Windows  |❌|
| Tizen	 |❌|

Getting Started

[![NuGet](https://img.shields.io/nuget/v/ComboBox.Maui.svg)](https://www.nuget.org/packages/ComboBox.Maui/) [![NuGet Downloads](https://img.shields.io/nuget/dt/ComboBox.Maui.svg)](https://www.nuget.org/packages/ComboBox.Maui/)



**Add Namespace**
```xml
xmlns:controls="clr-namespace:ComboBox.Maui;assembly=ComboBox.Maui"
```
**Create Control**
```xml
<controls:ComboBox ItemsSource="{Binding Items}" ShownText="Name" Icon="Search.png" />
```
**How it works**

```
• ItemsSource: Binds a collection of items (e.g., a list) from your ViewModel to the ComboBox.

• ShownText: Specifies which property of your model (e.g., Item) to display in the ComboBox.

• Setting ShownText="Name" will display the Name property of each Item in the ComboBox.

• Placeholder: Displays a small, hint text in the ComboBox when no item is selected.

• Icon: When you provide an image name, it displays the icon in front of the entry field. If the image name is empty or not provided, the icon will not be shown.
```
**Sample**

https://github.com/user-attachments/assets/9b4a8c17-4a02-420a-a66b-9f77f0b3e44c

**Sample with icon (Search.png)**

![image](https://github.com/user-attachments/assets/da149f22-ed59-48b1-9eaf-08e085b8fc7b)
