using com.rpdev.foundation.module.core.controller;
using com.rpdev.foundation.module.core.model;
using com.rpdev.foundation.module.core.view;
using UnityEngine;
using Zenject;

namespace com.rpdev.foundation.module.core {

	public class NoneViewModuleInstaller<TController, TModel> : Installer<NoneViewModuleInstaller<TController, TModel>> 
		                                                           where TController : IModuleController 
																   where TModel : IModuleModel {
		
		public override void InstallBindings() {
			InstallSignals();
			
			Container.BindInterfacesAndSelfTo<TController>().AsSingle().NonLazy();
			Container.BindInterfacesTo<TModel>().AsSingle();
		}
		
		protected virtual void InstallSignals() {
			
		}
	}
	
	public class NoneViewWithAdditionalDatModuleInstaller<TController, TModel> : Installer<ModuleAdditionalData, NoneViewWithAdditionalDatModuleInstaller<TController, TModel>> 
																						   where TController : IModuleController 
																						   where TModel : IModuleModel {
		private readonly ModuleAdditionalData _module_additional_data;

		public NoneViewWithAdditionalDatModuleInstaller(ModuleAdditionalData module_additional_data) {
			_module_additional_data = module_additional_data;
		}
		
		public override void InstallBindings() {
			InstallSignals();
			
			Container.BindInterfacesAndSelfTo<TController>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<ModuleAdditionalData>().FromInstance(_module_additional_data).WhenInjectedInto<TModel>();
			Container.BindInterfacesTo<TModel>().AsSingle();
		}
		
		protected virtual void InstallSignals() {
			
		}
	}
	
	public class NoneModelModuleInstaller<TController, TView> : Installer<TView, InitialModuleViewData, NoneModelModuleInstaller<TController, TView>>

		where TController : IModuleController
		where TView : ModuleView {
		
		private readonly TView                 _view;
		private readonly InitialModuleViewData _view_initial_module_view_data;

		protected NoneModelModuleInstaller(TView view, InitialModuleViewData view_initial_module_view_data) {
			_view                          = view;
			_view_initial_module_view_data = view_initial_module_view_data;
		}

		public override void InstallBindings() {
			InstallSignals();
			
			Container.BindInterfacesAndSelfTo<TController>().AsSingle().NonLazy();
			Container.BindInterfacesTo<TView>().FromMethod(CreateViewInstance).AsSingle();
		}
		
		private TView CreateViewInstance(InjectContext inject) {
			
			TView     view           = Container.InstantiatePrefabForComponent<TView>(_view);
			Transform view_transform = view.transform;
			
			view_transform.SetParent(_view_initial_module_view_data.parent_container);
			view_transform.localPosition = _view_initial_module_view_data.position;
			view_transform.localRotation = _view_initial_module_view_data.rotation;
			
			return view;
		}
		
		protected virtual void InstallSignals() { }
	}
	
	public class WithAdditionalDataModuleInstaller<TController, TModel, TView> : Installer<TView, 
																					   InitialModuleViewData, 
																					   ModuleAdditionalData, 
																					   WithAdditionalDataModuleInstaller<TController, TModel, TView>>
													
																					 where TController : IModuleController 
																					 where TModel : IModuleModel 
																					 where TView : ModuleView {
		private readonly TView                  _view;
		private readonly InitialModuleViewData _view_initial_module_view_data;
		private readonly ModuleAdditionalData        _additional_data;

		public WithAdditionalDataModuleInstaller(TView view, InitialModuleViewData view_initial_module_view_data, ModuleAdditionalData additional_data) {
			_view              = view;
			_view_initial_module_view_data = view_initial_module_view_data;
			_additional_data   = additional_data;
		}
		
		public override void InstallBindings() {
			
			InstallSignals();
			
			Container.BindInterfacesAndSelfTo<TController>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<ModuleAdditionalData>().FromInstance(_additional_data).WhenInjectedInto<TModel>();
			Container.BindInterfacesTo<TModel>().AsSingle();
			
			Container.BindInterfacesTo<TView>()
					 .FromMethod(CreateViewInstance)
					 .AsSingle();
		}

		protected virtual void InstallSignals() {
			
		}

		private TView CreateViewInstance(InjectContext inject) {
			
			TView     view           = Container.InstantiatePrefabForComponent<TView>(_view);
			Transform view_transform = view.transform;
			
			view_transform.SetParent(_view_initial_module_view_data.parent_container);
			view_transform.localPosition = _view_initial_module_view_data.position;
			view_transform.localRotation = _view_initial_module_view_data.rotation;
			
			return view;
		}
	}
	
	
	public class ModuleInstaller<TController, TModel, TView> : Installer<TView, InitialModuleViewData, ModuleInstaller<TController, TModel, TView>>
	
		where TController : IModuleController 
		where TModel : IModuleModel 
		where TView : ModuleView {
		
		private readonly TView                 _view;
		private readonly InitialModuleViewData _view_initial_module_view_data;

		protected ModuleInstaller(TView view, InitialModuleViewData view_initial_module_view_data) {
			_view                          = view;
			_view_initial_module_view_data = view_initial_module_view_data;
		}

		
		public override void InstallBindings() {
			
			InstallSignals();
			
			Container.BindInterfacesAndSelfTo<TController>().AsSingle().NonLazy();
			Container.BindInterfacesTo<TModel>().AsSingle();
			
			Container.BindInterfacesTo<TView>()
					 .FromMethod(CreateViewInstance)
					 .AsSingle();

			
		}
		
		private TView CreateViewInstance(InjectContext inject) {
			
			TView     view           = Container.InstantiatePrefabForComponent<TView>(_view);
			Transform view_transform = view.transform;
			
			view_transform.SetParent(_view_initial_module_view_data.parent_container);
			view_transform.localPosition = _view_initial_module_view_data.position;
			view_transform.localRotation = _view_initial_module_view_data.rotation;
			
			return view;
		}
		
		protected virtual void InstallSignals() { }
	}

}