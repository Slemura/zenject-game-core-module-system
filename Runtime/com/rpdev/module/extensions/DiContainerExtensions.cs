using System;
using com.rpdev.foundation.module.core;
using com.rpdev.foundation.module.core.controller;
using com.rpdev.foundation.module.core.model;
using com.rpdev.foundation.module.core.view;
using com.rpdev.module.common.commands;
using Zenject;

namespace com.rpdev.module.extensions {

	public static class DiContainerExtensions {
		
		public static void BindSimpleSignalToCommand<S, C>(this DiContainer container) where C : ICustomCommand {
			container.BindSignal<S>()
					 .ToMethod<C>((command, signal) => command.Execute())
					 .From(x => x.AsSingle());
		}

		public static void BindSignalToCommand<S, C>(this DiContainer container) where C : ICustomCommand<S> {
			container.BindSignal<S>()
					 .ToMethod<C>((command, signal) => {
						 command.Execute(signal);
					 })
					 .From(x => x.AsSingle());
		}
		
		public static void BindComplexSignalToCommand<S, C, T>(this DiContainer container, string property_name) where C : ICustomCommand<T> {
			container.BindSignal<S>()
					 .ToMethod<C>((command, signal) => {
						 Type signal_type = signal.GetType();
						 T param = (T)signal_type.GetField(property_name).GetValue(signal);
						 command.Execute(param);
					 })
					 .From(x => x.AsSingle());
		}
		
		public static void BindComplexSignalToCommand<S, C, T, V>(this DiContainer container, string[] properties_name) where C : ICustomCommand<T, V> {
			
			container.BindSignal<S>()
					 .ToMethod<C>((command, signal) => {
						 Type signal_type = signal.GetType();
						 T    param_a     = (T) signal_type.GetField(properties_name[0]).GetValue(signal);
						 V    param_b     = (V) signal_type.GetField(properties_name[1]).GetValue(signal);
						 command.Execute(param_a, param_b);
					 })
					 .From(x => x.AsSingle());
		}

		public static void BindComplexModuleFromDerivedInstallerWithAdditionalData<TFacade, TModuleController, TModuleInstaller>(this DiContainer container, ModuleView view_prefab, InitialModuleViewData view_initial_data, ModuleAdditionalData module_additional_data, bool exist_view = false)
			where TFacade : IModuleFacade 
			where TModuleController : ModuleController, TFacade
			where TModuleInstaller : InstallerBase {

			container.Bind<TFacade>()
					 .To<TModuleController>()
					 .FromSubContainerResolve()
					 .ByMethod(sub_container => {
						 ModuleView view_instance = exist_view ? view_prefab : ViewCustomFactory<ModuleView>.CreateViewFromPrefab(sub_container, view_prefab as ModuleView, view_initial_data);
						 sub_container.Install<ModuleView, InitialModuleViewData, ModuleAdditionalData, TModuleInstaller>(view_instance, view_initial_data, module_additional_data);
					 })
					 .WithKernel()
					 .AsSingle()
					 .NonLazy();
		}
		
		public static void BindComplexModuleFromDerivedInstaller<TFacade, TModuleController, TModuleInstaller>(this DiContainer container, ModuleView view_prefab, InitialModuleViewData view_initial_data, bool exist_view = false)
			where TFacade : IModuleFacade 
			where TModuleController : IModuleController, TFacade
			where TModuleInstaller : InstallerBase {

			container.Bind<TFacade>()
					 .To<TModuleController>()
					 .FromSubContainerResolve()
					 .ByMethod(sub_container => {
						 ModuleView view_instance = exist_view ? view_prefab : ViewCustomFactory<ModuleView>.CreateViewFromPrefab(sub_container, view_prefab as ModuleView, view_initial_data);
						 sub_container.Install<ModuleView, InitialModuleViewData, TModuleInstaller>(view_instance, view_initial_data);
					 })
					 .WithKernel()
					 .AsSingle()
					 .NonLazy();
		}
		
		public static void BindComplexModuleWithAdditionalData<TFacade, TModuleController, TModuleModel>(this DiContainer container, ModuleView view_prefab, InitialModuleViewData view_initial_data, ModuleAdditionalData module_additional_data, bool exist_view = false) 
			where TFacade : IModuleFacade 
			where TModuleController : ModuleController, TFacade 
			where TModuleModel : IModuleModel {
			
			container.Bind<TFacade>()
					 .To<TModuleController>()
					 .FromSubContainerResolve()
					 .ByMethod(sub_container => {
						 ModuleView view_instance = exist_view ? view_prefab : ViewCustomFactory<ModuleView>.CreateViewFromPrefab(sub_container, view_prefab as ModuleView, view_initial_data);
						 WithAdditionalDataModuleInstaller<TModuleController, TModuleModel, ModuleView>.Install(sub_container, view_instance, view_initial_data, module_additional_data);
					 })
					 .WithKernel()
					 .AsSingle()
					 .NonLazy();
		}
		
		public static void BindNoneModelModule<TFacade, TModuleController>(this DiContainer container, ModuleView view_prefab, InitialModuleViewData view_initial_data, bool exist_view = false) where TFacade : IModuleFacade 
			where TModuleController : ModuleController, TFacade {
			
			container.Bind<TFacade>()
					 .To<TModuleController>()
					 .FromSubContainerResolve()
					 .ByMethod(sub_container => {
						 ModuleView view_instance = exist_view ? view_prefab : ViewCustomFactory<ModuleView>.CreateViewFromPrefab(sub_container, view_prefab as ModuleView, view_initial_data);
						 NoneModelModuleInstaller<TModuleController, ModuleView>.Install(sub_container, view_instance, view_initial_data);
					 })
					 .WithKernel()
					 .AsSingle()
					 .NonLazy();
		}

		public static void BindComplexModule<TFacade, TModuleController, TModuleModel>(this DiContainer container, ModuleView view_prefab, InitialModuleViewData view_initial_data, bool exist_view = false) 
																																							  where TFacade : IModuleFacade 
			                                                                                                                                                  where TModuleController : ModuleController, TFacade 
																																							  where TModuleModel : IModuleModel {
			container.Bind<TFacade>()
					 .To<TModuleController>()
					 .FromSubContainerResolve()
					 .ByMethod(sub_container => {

						 ModuleView view_instance;
						 
						 if (exist_view) {
							 view_instance = view_prefab as ModuleView;
						 } else {
							 view_instance = ViewCustomFactory<ModuleView>.CreateViewFromPrefab(sub_container, view_prefab as ModuleView, view_initial_data);
						 }
						 
						 ModuleInstaller<TModuleController, TModuleModel, ModuleView>.Install(sub_container, view_instance, view_initial_data);
						 
					 })
					 .WithKernel()
					 .AsSingle()
					 .NonLazy();
		}
		
		
		private static void Install<TParam1, TParam2, TParam3, TDerivedInstaller>(this DiContainer container, TParam1 param1, TParam2 param2, TParam3 param3) where TDerivedInstaller : InstallerBase {
			container.InstantiateExplicit<TDerivedInstaller>(InjectUtil.CreateArgListExplicit(param1, param2, param3)).InstallBindings();
		}
		private static void Install<TParam1, TParam2, TDerivedInstaller>(this DiContainer container, TParam1 param1, TParam2 param2) where TDerivedInstaller : InstallerBase {
			container.InstantiateExplicit<TDerivedInstaller>(InjectUtil.CreateArgListExplicit(param1, param2)).InstallBindings();
		}
	}
}