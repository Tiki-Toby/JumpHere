using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    class ZoomInUngame : BaseCameraScript, IViewSizeChanged
    {
        public float ZoomInSmoothness = 1f;
        public float ZoomOutSmoothness = 1f;

        public float MaxZoomInAmount = 5f;
        public float MaxZoomOutAmount = 8f;
        public Vector2 offset;

        float _zoomVelocity;
        float _currentVelocity;

        public int PriorityOrder => 1;

        private void Start()
        {
            offset -= new Vector2(BaseCameraController.OffsetX, BaseCameraController.OffsetY);
        }

        public float HandleSizeChanged(float nextSize)
        {
            if (!enabled)
                return nextSize;

            var currentSize = BaseCameraController.ScreenSizeInWorldCoordinates.y;

            _currentVelocity = BaseCameraController.Targets.velocity.magnitude / Time.deltaTime;
            var targetSize = currentSize;
            // Zoom out
            if (GameHandler.Instance.IsGame && !GameHandler.Instance.IsPause)
            {
                targetSize = MaxZoomOutAmount;
            }
            // Zoom in
            else
            {
                targetSize = MaxZoomInAmount;
                BaseCameraController.ApplyInfluence(offset);
            }

            if (Mathf.Abs(currentSize - targetSize) > .001f)
            {
                float smoothness = (targetSize < currentSize) ? ZoomInSmoothness : ZoomOutSmoothness;
                targetSize = Mathf.SmoothDamp(currentSize, targetSize, ref _zoomVelocity, smoothness, Mathf.Infinity, Time.deltaTime);
            }
            else
                targetSize = currentSize;
            var zoomAmount = targetSize - currentSize;

            return nextSize + zoomAmount;
        }
    }
}
