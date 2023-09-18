using GoogleMapsComponents.Maps;
using GoogleMapsComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages
{
    public partial class MapAdvancedMarker
    {
        private GoogleMap _map1;
        private MapOptions _mapOptions;
        private readonly Stack<Marker> _markers = new Stack<Marker>();
        private LatLngBounds _bounds;
        private MarkerClustering _markerClustering;
        public int ZIndex { get; set; } = 0;

        [Inject]
        public IJSRuntime JsObjectRef { get; set; }


        protected override void OnInitialized()
        {
            _mapOptions = new MapOptions()
            {
                Zoom = 13,
                Center = new LatLngLiteral()
                {
                    Lat = 13.505892,
                    Lng = 100.8162
                },
                MapTypeId = MapTypeId.Roadmap,
                MapId = "3a3b33f0edd6ed2a"
            };
        }

        private async Task AddMarker()
        {
            var mapCenter = await _map1.InteropObject.GetCenter();
            ZIndex++;

            var marker = await AdvancedMarker.CreateAsync(_map1.JsRuntime, new MarkerOptions()
            {
                Position = mapCenter,
                Map = _map1.InteropObject,
                //Label = $"Test {markers.Count}",
                ZIndex = ZIndex,
                //CollisionBehavior = CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY,//2021-07 supported only in beta google maps version
                //Animation = Animation.Bounce
                //Icon = new Icon()
                //{
                //    Url = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png"
                //}
                //Icon = "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png"
            });

            _markers.Push(marker);

            //return;
            await _bounds.Extend(mapCenter);

            var icon = await marker.GetIcon();

            Console.WriteLine($"Get icon result type is : {icon.Value?.GetType()}");

            icon.Switch(
                s => Console.WriteLine(s),
                i => Console.WriteLine(i.Url),
                _ => { });

            _markers.Push(marker);

            await marker.AddListener<MouseEvent>("click", async e =>
            {
                //https://github.com/rungwiroon/BlazorGoogleMaps/issues/246
                //var before = marker.EventListeners;
                //await marker.ClearListeners("click");
                //var after = marker.EventListeners;

                var markerLabel = await marker.GetLabel();
                StateHasChanged();

                await e.Stop();
            });
        }
    }
}
