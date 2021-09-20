using System;
using com.rpdev.module.common;
using UnityEngine;
using Zenject;

namespace com.rpdev.foundation.module.core.view {

	public interface IModuleView : IActiveObject, IDisposable {
		public void SetPosition(Vector3    position);
		public void SetRotation(Quaternion rotation);
	}
	
	public class ModuleView : MonoBehaviour, IModuleView, IInitializable {

		[Inject]
		public virtual void Initialize() {
			
		}
		
		public void SetPosition(Vector3 position) {
			transform.localPosition = position;
		}

		public void SetRotation(Quaternion rotation) {
			transform.localRotation = rotation;
		}
		
		public virtual void Activate() {
			
		}

		public virtual void Deactivate() {
			
		}
		
		
		public virtual void Dispose() { }
	}
}