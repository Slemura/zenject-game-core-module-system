using System;
using com.rpdev.foundation.module.core.model;
using com.rpdev.foundation.module.core.view;
using Zenject;

namespace com.rpdev.foundation.module.core.controller {
	
	public interface IModuleFacade : IActiveObject, IDisposable {
		
	}

	public interface IModuleController {
		
	}
	
	public class ModuleController : IModuleFacade, IModuleController, IInitializable {

		private   IModuleModel _model;
		private   IModuleView  _view;
		private   SignalBus    _signal_bus;
		private   bool         _has_model;
		private   bool         _has_view;
		
		protected SignalBus Bus      => _signal_bus;
		protected bool      HasModel => _has_model;
		protected bool      HasView  => _has_view;
		
		protected T View<T>() where T : IModuleView {
			return (T)_view;
		}

		protected T Model<T>() where T : IModuleModel {
			return (T) _model;
		}

		[Inject]
		private void InjectDependencies([InjectOptional]IModuleModel module_model, [InjectOptional]IModuleView module_view, SignalBus signal_bus) {
			
			_signal_bus = signal_bus;
			_model      = module_model;
			_view       = module_view;
			
			_has_model  = _model != null;
			_has_view   = _view  != null;
		}

		[Inject]
		public virtual void Initialize() {
			
		}

		public virtual void Activate() {
			View<ModuleView>().Activate();
		}

		public virtual void Deactivate() {
			View<ModuleView>().Deactivate();
		}
		
		
		public virtual void Dispose() {}
	}
}