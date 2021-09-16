using System;
using com.rpdev.foundation.module.core.model;
using com.rpdev.module.common;
using UnityEngine;
using Zenject;

namespace com.rpdev.foundation.module.core.view {

	public interface IModuleView : IActiveObject, IDisposable {
		public void SetPosition(Vector3 position);

		public void SetRotation(Quaternion rotation);
	}
	
	public class ModuleView : MonoBehaviour, IModuleView, IInitializable {

		private   IModuleModel _model;
		private   bool         _has_model;

		protected T    Model<T>() => (T)_model;
		protected bool HasModel   => _has_model;
		
		[Inject]
		public void InjectDependencies([InjectOptional] IModuleModel module_model) {
			_model     = module_model;
			_has_model = _model != null;
		}
		
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