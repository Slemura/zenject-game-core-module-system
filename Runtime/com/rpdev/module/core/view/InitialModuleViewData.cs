using System;
using UnityEngine;

namespace com.rpdev.foundation.module.core.view {
    
    [Serializable]
    public class InitialModuleViewData {
        public Transform  parent_container;
        public Vector3    position;
        public Quaternion rotation;
    }
}