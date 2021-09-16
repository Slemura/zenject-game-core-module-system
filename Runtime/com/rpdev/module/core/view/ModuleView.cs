using System;
using com.rpdev.foundation.module.core.controller;
using com.rpdev.foundation.module.core.model;
using com.rpdev.module.common;
using UnityEngine;
using Zenject;

namespace com.rpdev.foundation.module.core.view {

	public interface IModuleView : IActiveObject, IDisposable {
		
		T           ModuleFacade<T>() where T : IModuleFacade;
		public void SetPosition(Vector3    position);
		public void SetRotation(Quaternion rotation);
	}
	
	public class ModuleView : MonoBehaviour, IModuleView, IInitializable {

		private IModuleModel  _model;
		private bool          _has_model;
		private IModuleFacade _facade;

		protected T    Model<T>() => (T)_model;
		protected bool HasModel   => _has_model;

		public T ModuleFacade<T>() where T : IModuleFacade {
			return (T)_facade;
		}

		[Inject]
		public void InjectDependencies(IModuleFacade module_facade, [InjectOptional] IModuleModel module_model) {
			_model     = module_model;
			_facade    = module_facade;
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