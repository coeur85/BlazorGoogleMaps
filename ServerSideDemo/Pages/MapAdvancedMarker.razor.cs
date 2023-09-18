using GoogleMapsComponents.Maps;
using GoogleMapsComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace ServerSideDemo.Pages
{
    public partial class MapAdvancedMarker
    {
        private GoogleMap _map1;
        private MapOptions _mapOptions;
        private LatLngBounds _bounds;
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
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _bounds = await LatLngBounds.CreateAsync(_map1.JsRuntime);
            }
        }
        private async Task AddMarker()
        {
            var mapCenter = await _map1.InteropObject.GetCenter();
            ZIndex++;

            var marker = await AdvancedMarker.CreateAsync(_map1.JsRuntime, new AdvancedMarkerOptions()
            {
                Position = mapCenter,
                Map = _map1.InteropObject,
                Title = "test advanced marker tilte",
                ZIndex = ZIndex,
                Content = "await CreateDiv(innerHTML)"//"pinBackground.element"
            }); ;



            //return;
            await _bounds.Extend(mapCenter);

            var icon = await marker.GetIcon();

            Console.WriteLine($"Get icon result type is : {icon.Value?.GetType()}");

            icon.Switch(
                s => Console.WriteLine(s),
                i => Console.WriteLine(i.Url),
                _ => { });



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

        private string BuilInfoWindow()
        {
            string html = @"<div>
                <div class='icon'>
                    <i aria-hidden='true' class='fa fa-icon fa-home' title='home'></i>
                    <span class='fa-sr-only'>home</span>
                </div>
                <div class='details'>
                    <div class='price'>1000$</div>
                    <div class='address'>215 Emily St, MountainView, CA</div>
                    <div class='features'>
                    <div>
                        <i aria-hidden='true' class='fa fa-bed fa-lg bed' title='bedroom'></i>
                        <span class='fa-sr-only'>bedroom</span>
                        <span>5</span>
                    </div>
                    <div>
                        <i aria-hidden='true' class='fa fa-bath fa-lg bath' title='bathroom'></i>
                        <span class='fa-sr-only'>bathroom</span>
                        <span>4.5</span>
                    </div>
                    <div>
                        <i aria-hidden='true' class='fa fa-ruler fa-lg size' title='size'></i>
                        <span class='fa-sr-only'>size</span>
                        <span>300 ft<sup>2</sup></span>
                    </div>
                    </div>
                </div>
            </div>";
            return html;
        }
    }
}
