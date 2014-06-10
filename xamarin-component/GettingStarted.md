You can add a `MapView` to your xml layout using:

```xml
<org.osmdroid.views.MapView
    android:id="@+id/mapview"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    tilesource="Mapnik" />
```

This will allow you to configure the tile source imagery for your `MapView` but not much else.

However, for more control over your `MapView`, you will want to create a `MapView` programmatically.

```csharp
public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
{
    _resourceProxy = new ResourceProxyImpl(inflater.Context.ApplicationContext);
    _mapView = new MapView(inflater.Context, 256, _resourceProxy);
    return _mapView;
}
```
