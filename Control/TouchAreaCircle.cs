﻿using UnityEditor;
using UnityEngine;

namespace MaTech.Control {
    public class TouchAreaCircle : TouchArea3DBase {
        public float radius = 1;
        protected bool CheckInRadius(Vector2 localPosition) {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (radius == 0) return false;
            if (radius > 0) return localPosition.sqrMagnitude < radius * radius;
            return localPosition.sqrMagnitude > radius * radius;
        }
        
        protected override bool Contains(Vector2 localPosition) {
            return CheckInRadius(localPosition);
        }
        
        #if UNITY_EDITOR
        protected override void DrawLocalGizmos() {
            Handles.DrawWireDisc(Vector3.zero, Vector3.forward, radius);
            float endRadius = radius * (radius >= 0 ? 1.5f : 0.5f);
            Handles.DrawAAPolyLine(Vector3.up * radius, Vector3.up * endRadius);
            Handles.DrawAAPolyLine(Vector3.down * radius, Vector3.down * endRadius);
            Handles.DrawAAPolyLine(Vector3.left * radius, Vector3.left * endRadius);
            Handles.DrawAAPolyLine(Vector3.right * radius, Vector3.right * endRadius);
        }
        #endif
    }
}