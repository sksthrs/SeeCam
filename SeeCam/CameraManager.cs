using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;

namespace SeeCam
{
    public class CameraManager
    {
        public event NewFrameEventHandler NewFrame = (s,e) => { };
        private VideoCaptureDevice device;
        private FilterInfo currentFilter;
        private FilterInfoCollection filters;

        public IEnumerable<CameraInfo> EnumerateCameras()
        {
            filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            var ix = 0;
            var isUsingCamera = device?.IsRunning ?? false;
            foreach (FilterInfo filter in filters)
            {
                var isUsing = (isUsingCamera)
                    ? filter.MonikerString == currentFilter.MonikerString
                    : false;
                yield return new CameraInfo
                {
                    Index = ix,
                    Name = filter.Name,
                    IsUsing = isUsing
                };
                ix++;
            }
        }

        public int CountCameras()
        {
            filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            return filters.Count;
        }

        public string SelectCamera(int index)
        {
            if (filters == null) return null;
            if (index < 0 || index >= filters.Count) return null;
            if (filters[index].MonikerString == currentFilter?.MonikerString)
            {
                return currentFilter.Name;
            }

            if (device != null && device.IsRunning)
            {
                device.NewFrame -= NewFrame;
                device.SignalToStop();
            }

            currentFilter = filters[index];
            var moniker = currentFilter.MonikerString;
            try
            {
                device = new VideoCaptureDevice(moniker);
                device.NewFrame += NewFrame;
                device.Start();
                return currentFilter.Name;
            }
            catch (Exception)
            {
                device = null;
                return null;
            }
        }

        public void StopCamera()
        {
            if (device?.IsRunning != true) return;
            device.NewFrame -= NewFrame;
            device.SignalToStop();
            currentFilter = null;
        }
    }

    public class CameraInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public bool IsUsing { get; set; }
    }
}
