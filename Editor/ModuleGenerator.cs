using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
	
namespace com.rpdev.foundation.module.editor {

	public class ModuleGenerator {

		private const string CONTROLLER_TEMPLATE               = "templates\\ModuleController.tt";
		private const string CONTROLLER_TEMPLATE_ALONE         = "templates\\ModuleControllerAlone.tt";
		private const string CONTROLLER_TEMPLATE_WITHOUT_VIEW  = "templates\\ModuleControllerWithoutView.tt";
		private const string CONTROLLER_TEMPLATE_WITHOUT_MODEL = "templates\\ModuleControllerWithoutModel.tt";
		private const string MODEL_TEMPLATE                    = "templates\\ModuleModel.tt";
		private const string VIEW_TEMPLATE                     = "templates\\ModuleView.tt";
		private const string VIEW_TEMPLATE_WITHOUT_MODEL       = "templates\\ModuleViewWithoutModel.tt";
		private const string INSTALLER_TEMPLATE                = "templates\\ModuleInstaller.tt";
		private const string ADDITIONAL_DATA_TEMPLATE          = "templates\\ModuleAdditionalModelData.tt";
		
		private readonly ModuleSettings _settings;
		private          string         _module_namespace;
		
		public ModuleGenerator(ModuleSettings settings) {
			_settings = settings;
		}

		public void Generate() {
			string file_path     = GetCurrentFileName();
			
			Debug.Log($"::: Generate module :::");

			int    last_splash   = file_path.LastIndexOf("\\");
			string template_path = file_path.Substring(0, last_splash);
			
			string modified_name = Regex.Replace(_settings.module_name, @"(?<!_)([A-Z])", "_$1");
			
			Debug.Log($"Modified module name {modified_name}");
			
			_module_namespace = modified_name.Substring(1, modified_name.Length - 1).ToLower();
			
			DirectoryInfo module_directory = Directory.CreateDirectory(Path.Combine(_settings.module_path, _module_namespace));

			DirectoryInfo controller_directory = Directory.CreateDirectory(Path.Combine(module_directory.FullName, "controller"));

			if (_settings.is_have_model && _settings.is_havel_view) {
				GenerateFile(Path.Combine(template_path, CONTROLLER_TEMPLATE), Path.Combine(controller_directory.FullName, _settings.module_name + "Controller.cs"));	
			} else if (_settings.is_havel_view) {
				GenerateFile(Path.Combine(template_path, CONTROLLER_TEMPLATE_WITHOUT_MODEL), Path.Combine(controller_directory.FullName, _settings.module_name + "Controller.cs"));
			} else if (_settings.is_have_model) {
				GenerateFile(Path.Combine(template_path, CONTROLLER_TEMPLATE_WITHOUT_VIEW), Path.Combine(controller_directory.FullName, _settings.module_name + "Controller.cs"));
			} else {
				GenerateFile(Path.Combine(template_path, CONTROLLER_TEMPLATE_ALONE), Path.Combine(controller_directory.FullName, _settings.module_name + "Controller.cs"));
			}
			
			if (_settings.is_have_model) {
				
				DirectoryInfo model_directory = Directory.CreateDirectory(Path.Combine(module_directory.FullName, "model"));
				
				GenerateFile(Path.Combine(template_path, MODEL_TEMPLATE), Path.Combine(model_directory.FullName, _settings.module_name + "Model.cs"));
				
				if (_settings.is_additional_custom_data) {
					GenerateFile(Path.Combine(template_path, ADDITIONAL_DATA_TEMPLATE), Path.Combine(model_directory.FullName, _settings.module_name + "AdditionalModelData.cs"));
				}
			}

			if (_settings.is_havel_view) {
				DirectoryInfo view_directory = Directory.CreateDirectory(Path.Combine(module_directory.FullName, "view"));
				
				if (_settings.is_have_model) {
					GenerateFile(Path.Combine(template_path, VIEW_TEMPLATE), Path.Combine(view_directory.FullName, _settings.module_name + "View.cs"));	
				} else {
					GenerateFile(Path.Combine(template_path, VIEW_TEMPLATE_WITHOUT_MODEL), Path.Combine(view_directory.FullName, _settings.module_name + "View.cs"));
				}
			}

			if (_settings.is_custom_installer) {
				GenerateFile(Path.Combine(template_path, INSTALLER_TEMPLATE), Path.Combine(module_directory.FullName, _settings.module_name + "Installer.cs"));
			}
			AssetDatabase.Refresh();
		}

		private void GenerateFile(string file_path, string new_file_path) {
			
			StreamReader stream_reader = new StreamReader(file_path);
			string content = stream_reader.ReadToEnd();
			stream_reader.Close();

			string modified_content = content.Replace("#ModuleName#", _settings.module_name).Replace("#module_namespace#", _module_namespace).Replace("#DerivedModule#", _settings.derived_module);
			
			Debug.Log($"Create module part {new_file_path}");
			StreamWriter stream_writer = new StreamWriter(new_file_path);
			stream_writer.Write(modified_content);
			stream_writer.Close();
		}
		
		private string GetCurrentFileName([System.Runtime.CompilerServices.CallerFilePath] string file_name = null) {
			return file_name;
		}
		
		public struct ModuleSettings {
			
			public string module_name;
			public string module_path;
			public string derived_module;
			
			public bool   is_have_model;
			public bool   is_havel_view;
			public bool   is_custom_installer;
			public bool   is_additional_custom_data;

		}
	}
}