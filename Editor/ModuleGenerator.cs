
	
namespace com.rpdev.foundation.module.editor {
	
#if UNITY_EDITOR
	using System.IO;
	using System.Text.RegularExpressions;
	using UnityEditor;
	using UnityEngine;	

	public class ModuleGenerator {

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

			
			string interface_info = ModuleGeneratorConstants.CONTROLLER_FACADE_INTERFACE_BODY;
			string interface_impl = ModuleGeneratorConstants.CONTROLLER_FACADE_INTERFACE_NAME;
			
			if (_settings.is_custom_installer) {
				interface_info += "\n\t" + ModuleGeneratorConstants.CONTROLLER_INTERFACE_BODY;
				interface_impl += ", " + ModuleGeneratorConstants.CONTROLLER_INTERFACE_NAME;
			}
			
			string props_info = (_settings.is_have_model ? ModuleGeneratorConstants.MODEL_PROPERTY : "") + "\n" + 
								"\t\t" + (_settings.is_havel_view ? ModuleGeneratorConstants.VIEW_PROPERTY : "");
			
			GenerateFile(Path.Combine(template_path, ModuleGeneratorConstants.CONTROLLER_TEMPLATE),
						 Path.Combine(controller_directory.FullName, _settings.module_name + "Controller.cs"), interface_info, interface_impl, props_info);
			

			if (_settings.is_have_model) {
				
				DirectoryInfo model_directory = Directory.CreateDirectory(Path.Combine(module_directory.FullName, "model"));
				
				if (_settings.is_additional_custom_data) {
					GenerateFile(Path.Combine(template_path, ModuleGeneratorConstants.ADDITIONAL_DATA_TEMPLATE), Path.Combine(model_directory.FullName, _settings.module_name + "AdditionalModelData.cs"));
					GenerateFile(Path.Combine(template_path, ModuleGeneratorConstants.MODEL_TEMPLATE), Path.Combine(model_directory.FullName, _settings.module_name + "Model.cs"), "\t" + ModuleGeneratorConstants.ADDITIONAL_DATA_PROPERTY, ModuleGeneratorConstants.ADDITIONAL_DATA_INTERFACE_PROPERTY);
				} else {
					GenerateFile(Path.Combine(template_path, ModuleGeneratorConstants.MODEL_TEMPLATE), Path.Combine(model_directory.FullName, _settings.module_name + "Model.cs"), "", "");
				}
			}

			if (_settings.is_havel_view) {
				DirectoryInfo view_directory = Directory.CreateDirectory(Path.Combine(module_directory.FullName, "view"));

				GenerateFile(Path.Combine(template_path, ModuleGeneratorConstants.VIEW_TEMPLATE),
							 Path.Combine(view_directory.FullName, _settings.module_name + "View.cs"));
			}

			if (_settings.is_custom_installer) {
				GenerateFile(Path.Combine(template_path, ModuleGeneratorConstants.INSTALLER_TEMPLATE), Path.Combine(module_directory.FullName, _settings.module_name + "Installer.cs"));
			}
			
			AssetDatabase.Refresh();
		}

		private void GenerateFile(string file_path, string new_file_path) {
			
			StreamReader stream_reader = new StreamReader(file_path);
			string       content       = stream_reader.ReadToEnd();
			stream_reader.Close();

			string modified_content = content.Replace("#ModuleName#", _settings.module_name).Replace("#module_namespace#", _module_namespace).Replace("#DerivedModule#", _settings.derived_module);
			
			Debug.Log($"Create module part {new_file_path}");
			StreamWriter stream_writer = new StreamWriter(new_file_path);
			stream_writer.Write(modified_content);
			stream_writer.Close();
		}
		
		private void GenerateFile(string file_path, string new_file_path, string props_info) {
			
			StreamReader stream_reader = new StreamReader(file_path);
			string       content       = stream_reader.ReadToEnd();
			stream_reader.Close();
			
			string modified_content = content.Replace("#Properties#", props_info);
			modified_content = modified_content.Replace("#ModuleName#", _settings.module_name).Replace("#module_namespace#", _module_namespace).Replace("#DerivedModule#", _settings.derived_module);

			Debug.Log($"Create module part {new_file_path}");
			StreamWriter stream_writer = new StreamWriter(new_file_path);
			stream_writer.Write(modified_content);
			stream_writer.Close();
		}
		
		private void GenerateFile(string file_path, string new_file_path, string props_info, string interface_props) {
			
			StreamReader stream_reader = new StreamReader(file_path);
			string       content       = stream_reader.ReadToEnd();
			stream_reader.Close();
			
			string modified_content = content.Replace("#Properties#", props_info);
			modified_content = modified_content.Replace("#InterfaceProps#", interface_props);
			modified_content = modified_content.Replace("#ModuleName#", _settings.module_name).Replace("#module_namespace#", _module_namespace).Replace("#DerivedModule#", _settings.derived_module);

			Debug.Log($"Create module part {new_file_path}");
			StreamWriter stream_writer = new StreamWriter(new_file_path);
			stream_writer.Write(modified_content);
			stream_writer.Close();
		}
		
		private void GenerateFile(string file_path, string new_file_path, string interface_info, string interface_impl, string props_info) {
			
			StreamReader stream_reader = new StreamReader(file_path);
			string content = stream_reader.ReadToEnd();
			stream_reader.Close();
			
			string modified_content = content;
			
			Debug.LogError($" Interface info {interface_info}");
			Debug.LogError($"Impl {interface_impl}");
			
			modified_content = modified_content.Replace("#Interfaces#", interface_info);
			modified_content = modified_content.Replace("#InterfacesImpl#", interface_impl);
			modified_content = modified_content.Replace("#Properties#", props_info);
			modified_content = modified_content.Replace("#ModuleName#", _settings.module_name).Replace("#module_namespace#", _module_namespace).Replace("#DerivedModule#", _settings.derived_module);

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
#endif
}