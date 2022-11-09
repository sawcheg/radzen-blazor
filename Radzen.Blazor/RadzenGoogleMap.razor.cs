using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radzen.Blazor
{
    /// <summary>
    /// RadzenGoogleMap component.
    /// </summary>
    /// <example>
    /// <code>
    /// &lt;RadzenGoogleMap Zoom="3" Center=@(new GoogleMapPosition() { Lat = 42.6977, Lng = 23.3219 }) MapClick=@OnMapClick MarkerClick=@OnMarkerClick"&gt;
    ///     &lt;Markers&gt;
    ///         &lt;RadzenGoogleMapMarker Title="London" Label="London" Position=@(new GoogleMapPosition() { Lat = 51.5074, Lng = 0.1278 }) /&gt;
    ///         &lt;RadzenGoogleMapMarker Title="Paris " Label="Paris" Position=@(new GoogleMapPosition() { Lat = 48.8566, Lng = 2.3522 }) /&gt;
    ///     &lt;/Markers&gt;
    /// &lt;/RadzenGoogleMap&gt;
    /// @code {
    ///   void OnMapClick(GoogleMapClickEventArgs args)
    ///   {
    ///     Console.WriteLine($"Map clicked at Lat: {args.Position.Lat}, Lng: {args.Position.Lng}");
    ///   }
    ///   
    ///   void OnMarkerClick(RadzenGoogleMapMarker marker)
    ///   {
    ///     Console.WriteLine($"Map {marker.Title} marker clicked. Marker position -> Lat: {marker.Position.Lat}, Lng: {marker.Position.Lng}");
    ///   }
    /// }
    /// </code>
    /// </example>
    public partial class RadzenGoogleMap : RadzenComponent
    {
        /// <summary>
        /// Gets or sets the data - collection of RadzenGoogleMapMarker.
        /// </summary>
        /// <value>The data.</value>
        [Parameter]
        public IEnumerable<RadzenGoogleMapMarker> Data { get; set; }

        /// <summary>
        /// Gets or sets the map click callback.
        /// </summary>
        /// <value>The map click callback.</value>
        [Parameter]
        public EventCallback<GoogleMapClickEventArgs> MapClick { get; set; }

        /// <summary>
        /// Gets or sets the marker click callback.
        /// </summary>
        /// <value>The marker click callback.</value>
        [Parameter]
        public EventCallback<RadzenGoogleMapMarker> MarkerClick { get; set; }

        /// <summary>
        /// Gets or sets the Google API key.
        /// </summary>
        /// <value>The Google API key.</value>
        [Parameter]
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the Google map options: https://developers.google.com/maps/documentation/javascript/reference/map#MapOptions.
        /// </summary>
        /// <value>The Google map options.</value>
        [Parameter]
        public Dictionary<string, object> Options
        {
            get => mapOptions; set
            {
                bool toDelete = mapOptions?.Keys.Any(k => !(value?.ContainsKey(k) ?? false)) ?? false;
                bool toInsert = value?.Keys.Any(k => !(mapOptions?.ContainsKey(k) ?? false)) ?? false;
                needUpdate = toDelete || toInsert;
                if (!needUpdate && value != null && mapOptions != null)
                {
                    foreach (var key in value.Keys.Where(k => mapOptions.ContainsKey(k)))
                        if (!Equals(value[key], mapOptions[key]))
                        {
                            needUpdate = true;
                            break;
                        }
                }
                mapOptions = value;
                if (needUpdate)
                    InvokeAsync(StateHasChanged);
            }
        }
        Dictionary<string, object> mapOptions;

        double zoom = 8;
        /// <summary>
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>The zoom.</value>
        [Parameter]
        public double Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                if (zoom != value)
                {
                    zoom = value;
                    needUpdate = true;
                    InvokeAsync(StateHasChanged);
                }
            }
        }

        GoogleMapPosition center = new() { Lat = 0, Lng = 0 };
        /// <summary>
        /// Gets or sets the center map position.
        /// </summary>
        /// <value>The center.</value>
        [Parameter]
        public GoogleMapPosition Center
        {
            get
            {
                return center;
            }
            set
            {
                if (!center.Equals(value))
                {
                    center = value;
                    needUpdate = true;
                    InvokeAsync(StateHasChanged);
                }
            }
        }

        /// <summary>
        /// Gets or sets the markers.
        /// </summary>
        /// <value>The markers.</value>
        [Parameter]
        public RenderFragment Markers { get; set; }

        readonly List<RadzenGoogleMapMarker> markers = new();

        /// <summary>
        /// Gets or sets name or url of the cursor to display when the map is being dragged
        /// </summary>
        /// <value>The dragging cursor</value>
        [Parameter]
        [CustomJsOption("draggableCursor")]
        public string DraggingCursor
        {
            get => draggingCursor;
            set
            {
                if (value != draggingCursor)
                {
                    draggingCursor = value;
                    mapOptionsChanged = true;
                    InvokeAsync(StateHasChanged);
                }
            }
        }
        private string draggingCursor = string.Empty;

        private bool mapOptionsChanged;
        private bool needUpdate;

        /// <summary>
        /// Adds the marker.
        /// </summary>
        /// <param name="marker">The marker.</param>
        public void AddMarker(RadzenGoogleMapMarker marker)
        {
            if (markers.IndexOf(marker) == -1)
            {
                markers.Add(marker);
                needUpdate = true;
            }
        }

        /// <summary>
        /// Removes the marker.
        /// </summary>
        /// <param name="marker">The marker.</param>
        public void RemoveMarker(RadzenGoogleMapMarker marker)
        {
            if (markers.IndexOf(marker) != -1)
            {
                markers.Remove(marker);
                needUpdate = true;
            }
        }

        /// <inheritdoc />
        protected override string GetComponentCssClass()
        {
            return "rz-map";
        }

        /// <summary>
        /// Handles the <see cref="E:MapClick" /> event.
        /// </summary>
        /// <param name="args">The <see cref="GoogleMapClickEventArgs"/> instance containing the event data.</param>
        [JSInvokable("RadzenGoogleMap.OnMapClick")]
        public async System.Threading.Tasks.Task OnMapClick(GoogleMapClickEventArgs args)
        {
            await MapClick.InvokeAsync(args);
        }

        /// <summary>
        /// Called when marker click.
        /// </summary>
        /// <param name="marker">The marker.</param>
        [JSInvokable("RadzenGoogleMap.OnMarkerClick")]
        public async System.Threading.Tasks.Task OnMarkerClick(RadzenGoogleMapMarker marker)
        {
            await MarkerClick.InvokeAsync(marker);
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            var data = Data ?? markers;
            var markersForUpdate = data.Where(m => m.ParamsChanged).ToList();
            var dataMarkers = data.Select(m => new { m.Title, m.Label, m.Position, Icon = m.IconSrc ?? string.Empty });
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("Radzen.createMap", Element, Reference, UniqueID, ApiKey, Zoom, Center, dataMarkers);
            }
            else if (markersForUpdate.Any() || needUpdate)
            {
                await JSRuntime.InvokeVoidAsync("Radzen.updateMap", UniqueID, Zoom, Center, dataMarkers);
                markersForUpdate.ForEach(m => m.ParamsChanged = false);
                needUpdate = false;
            }            
            if (firstRender || mapOptionsChanged)
            {
                var options = GetType().GetProperties()
                   .Where(p => Attribute.IsDefined(p, typeof(CustomJsOptionAttribute)))
                   .Select(p => new
                   {
                       Name = (Attribute.GetCustomAttribute(p, typeof(CustomJsOptionAttribute)) as CustomJsOptionAttribute).PropertyName,
                       Value = p.GetValue(this)
                   })
                   .ToDictionary(k => k.Name, v => v.Value);
                if (options.Count > 0)
                    await JSRuntime.InvokeVoidAsync("Radzen.customizeMap", UniqueID, options);
                mapOptionsChanged = false;
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            if (IsJSRuntimeAvailable)
            {
                JSRuntime.InvokeVoidAsync("Radzen.destroyMap", UniqueID);
            }
        }
    }
}
