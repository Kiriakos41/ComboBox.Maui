using System.Collections;
using System.ComponentModel;
using static Microsoft.Maui.Controls.VisualMarker;
namespace ComboBox.Maui;
public class ComboBox : StackLayout
{
    private Entry _entry;
    private ListView _listView;
    private bool _suppressFiltering;
    private bool _supressSelectedItemFiltering;
    //Bindable properties
    public static readonly BindableProperty ListViewHeightRequestProperty = BindableProperty.Create(nameof(ListViewHeightRequest), typeof(double), typeof(ComboBox), defaultValue: null, propertyChanged: (bindable, oldVal, newVal) =>
    {
        var comboBox = (ComboBox)bindable;
        comboBox._listView.HeightRequest = (double)newVal;
    });
    public double ListViewHeightRequest
    {
        get => (double)GetValue(ListViewHeightRequestProperty);
        set => SetValue(ListViewHeightRequestProperty, value);
    }
    public static readonly BindableProperty EntryBackgroundColorProperty = BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(ComboBox), defaultValue: null, propertyChanged: (bindable, oldVal, newVal) =>
    {
        var comboBox = (ComboBox)bindable;
        comboBox._entry.BackgroundColor = (Color)newVal;
    });
    public Color EntryBackgroundColor
    {
        get => (Color)GetValue(EntryBackgroundColorProperty);
        set => SetValue(EntryBackgroundColorProperty, value);
    }
    public static readonly BindableProperty EntryFontSizeProperty = BindableProperty.Create(nameof(EntryFontSize), typeof(double), typeof(ComboBox), defaultValue: null, propertyChanged: (bindable, oldVal, newVal) =>
    {
        var comboBox = (ComboBox)bindable;
        comboBox._entry.FontSize = (double)newVal;
    });
    [TypeConverter(typeof(FontSizeConverter))]
    public double EntryFontSize
    {
        get => (double)GetValue(EntryFontSizeProperty);
        set => SetValue(EntryFontSizeProperty, value);
    }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ComboBox), defaultValue: null, propertyChanged: (bindable, oldVal, newVal) =>
    {
        var comboBox = (ComboBox)bindable;
        comboBox._listView.ItemsSource = (IEnumerable)newVal;
    });
    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set
        {
            SetValue(ItemsSourceProperty, value);
            FilterItems(_entry.Text);
        }
    }
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(ComboBox), defaultValue: null, propertyChanged: (bindable, oldVal, newVal) =>
    {
        var comboBox = (ComboBox)bindable;
        comboBox._listView.SelectedItem = newVal;
    });
    public object SelectedItem
    {
        get => (object)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    public static new readonly BindableProperty VisualProperty = BindableProperty.Create(nameof(Visual), typeof(IVisual), typeof(ComboBox), defaultValue: new DefaultVisual(), propertyChanged: (bindable, oldVal, newVal) =>
    {
        var comboBox = (ComboBox)bindable;
        comboBox._listView.Visual = (IVisual)newVal;
        comboBox._entry.Visual = (IVisual)newVal;
    });
    public new IVisual Visual
    {
        get => (IVisual)GetValue(VisualProperty);
        set => SetValue(VisualProperty, value);
    }
    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ComboBox), defaultValue: "", propertyChanged: (bindable, oldVal, newVal) =>
    {
        var comboBox = (ComboBox)bindable;
        comboBox._entry.Placeholder = (string)newVal;
    });
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ComboBox), defaultValue: "", propertyChanged: (bindable, oldVal, newVal) =>
    {
        var comboBox = (ComboBox)bindable;
        comboBox._entry.Text = (string)newVal;
    });
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ComboBox), defaultValue: null, propertyChanged: (bindable, oldVal, newVal) =>
    {
        var comboBox = (ComboBox)bindable;
        comboBox._listView.ItemTemplate = (DataTemplate)newVal;
    });
    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

    public static readonly BindableProperty ShownTextProperty =
    BindableProperty.Create(nameof(ShownText), typeof(string), typeof(ComboBox));

    public string ShownText
    {
        get => (string)GetValue(ShownTextProperty);
        set => SetValue(ShownTextProperty, value);
    }
    protected virtual void OnSelectedItemChanged(SelectedItemChangedEventArgs e)
    {
        EventHandler<SelectedItemChangedEventArgs> handler = SelectedItemChanged;
        handler?.Invoke(this, e);
        _listView.IsVisible = false;

    }
    public event EventHandler<TextChangedEventArgs> TextChanged;
    protected virtual void OnTextChanged(TextChangedEventArgs e)
    {
        EventHandler<TextChangedEventArgs> handler = TextChanged;
        handler?.Invoke(this, e);
    }
    public ComboBox()
    {
        //Entry used for filtering list view
        _entry = new Entry();
        _entry.Margin = new Thickness(0);
        _entry.Keyboard = Keyboard.Create(KeyboardFlags.None);
        _entry.Focused += (sender, args) => _listView.IsVisible = true;
        _entry.Unfocused += (sender, args) => _listView.IsVisible = false;
        //Text changed event, bring it back to the surface
        _entry.TextChanged += (sender, args) =>
        {
            if (_suppressFiltering)
                return;

            if (String.IsNullOrEmpty(args.NewTextValue))
            {
                _supressSelectedItemFiltering = true;
                _listView.SelectedItem = null;
                _supressSelectedItemFiltering = false;
                _listView.IsVisible = false;
            }
            else
            {
                _listView.IsVisible = true;
                FilterItems(args.NewTextValue);
            }

            OnTextChanged(args);
        };
        //List view - used to display search options
        _listView = new ListView();
        _listView.SelectionMode = ListViewSelectionMode.Single;
        _listView.IsVisible = false;
        _listView.SeparatorVisibility = SeparatorVisibility.None;
        _listView.Margin = new Thickness(0);
        Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.ListView.SetSeparatorStyle(_listView, Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.SeparatorStyle.FullWidth);
        _listView.HeightRequest = 0;

        _listView.HorizontalOptions = LayoutOptions.StartAndExpand;
        _listView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(ComboBox.SelectedItem), source: this));
        //Item selected event, surface it back to the top
        _listView.ItemTemplate = new DataTemplate(() =>
        {
            var viewCell = new ViewCell();

            // Assuming the item has a string property called "Name" for display
            var label = new Label();
            label.SetBinding(Label.TextProperty, $"{ShownText}"); // Bind to your property name

            // Set normal background color
            viewCell.View = new StackLayout
            {
                Children = { label },
                Padding = new Thickness(10)
            };

            viewCell.View.BackgroundColor = Colors.Transparent;

            viewCell.Tapped += (sender, args) =>
            {
                // Change the color of the selected item to gray
                viewCell.View.BackgroundColor = Colors.Gray;
            };

            return viewCell;
        });

        // Add event handler to reset colors
        _listView.ItemSelected += (sender, args) =>
        {
            if (!_supressSelectedItemFiltering)
            {
                _suppressFiltering = true;
                var selectedItem = args.SelectedItem;
                _entry.Text = !String.IsNullOrEmpty(ShownText) && selectedItem != null ? selectedItem.GetType().GetProperty(ShownText).GetValue(selectedItem, null).ToString() : selectedItem?.ToString();
                _suppressFiltering = false;
                _listView.IsVisible = false;
                OnSelectedItemChanged(args);
                _entry.Unfocus();
            }

            // Reset background colors of all items when selection changes
            foreach (var cell in _listView.ItemsSource)
            {
                // Assuming you have a reference to the cells, or use a DataTemplateSelector
                // Logic here to reset each item's background to white
            }
        };

        //Add bottom border
        Children.Add(_entry);
        Children.Add(_listView);

        // Initialize the ListView height
        UpdateListViewHeight();

    }
    private void FilterItems(string filterText)
    {
        if (ItemsSource == null)
            return;

        var filteredItems = ItemsSource
            .Cast<object>()
            .Where(item =>
            {
                var itemText = item.GetType().GetProperty(ShownText)?.GetValue(item, null)?.ToString();
                return !string.IsNullOrEmpty(itemText) && itemText.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0;
            })
            .ToList();

        // Update the ListView's ItemsSource
        _listView.ItemsSource = filteredItems;

        // Determine visibility and update height
        if (filteredItems.Any())
        {
            _listView.IsVisible = true;
            UpdateListViewHeight();
        }
        else
        {
            _listView.IsVisible = false;
        }
    }
    private void UpdateListViewHeight()
    {
        const double maxHeight = 100; // Maximum height
        const double itemHeight = 44; // Approximate height of each item (you may need to adjust this)

        if (_listView.ItemsSource == null)
            return;

        var itemCount = (_listView.ItemsSource as IEnumerable)?.Cast<object>().Count() ?? 0;
        var calculatedHeight = itemCount * itemHeight;

        // Set the height to the calculated value but not exceeding maxHeight
        _listView.HeightRequest = Math.Min(calculatedHeight, maxHeight);
    }


    public new bool Focus()
    {
        return _entry.Focus();
    }
    public new void Unfocus()
    {
        _entry.Unfocus();
    }
}