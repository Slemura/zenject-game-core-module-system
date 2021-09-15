using System;
using com.rpdev.foundation.module.core.model;
using UnityEngine;
using Zenject;

namespace com.rpdev.foundation.module.core.view {

	public interface IModuleView : IActiveObject, IDisposable {
		public void SetPosition(Vector3 position);

		public void SetRotation(Quaternion rotation);
	}
	
	public class ModuleView : MonoBehaviour, IModuleView {

		private   IModuleModel _model;
		private   bool         _has_model;

		protected T    Model<T>() => (T)_model;
		protected bool HasModel   => _has_model;
		
		[Inject]
		public void InjectDependencies([InjectOptional] IModuleModel module_model) {
			_model     = module_model;
			_has_model = _model != null;
		}

		public void SetPosition(Vector3 position) {
			transform.localPosition = position;
		}

		public void SetRotation(Quaternion rotation) {
			transform.localRotation = rotation;
		}
		
		public void Activate() {
			
		}

		public void Deactivate() {
			
		}
		
		
		public void Dispose() { }

	}
}