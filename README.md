![515901-200](https://github.com/user-attachments/assets/8d35eb74-f195-4117-ad83-06e2a83b92c4)

Maui.ColorPicker ⬇️

a custom control for .net maui ComboBox

Getting Started
**Add Namespace**
```xml
xmlns:controls="clr-namespace:ComboBox.Maui;assembly=ComboBox.Maui"
```
**Create Control**
```xml
<controls:ComboBox ItemsSource="{Binding Items}" ShownText="Name" />
```
**How it works**

```
• ItemsSource: Binds a collection of items (e.g., a list) from your ViewModel to the ComboBox.

• ShownText: Specifies which property of your model (e.g., Item) to display in the ComboBox.

• Setting ShownText="Name" will display the Name property of each Item in the ComboBox.

• Placeholder: Displays a small, hint text in the ComboBox when no item is selected.
```
**Sample:**

https://github.com/user-attachments/assets/9b4a8c17-4a02-420a-a66b-9f77f0b3e44c

