
	
namespace com.rpdev.foundation.module.editor {
	
#if UNITY_EDITOR
	
	using System;
	using System.Linq;
	using com.rpdev.foundation.module.core.controller;
	using UnityEditor;
	using UnityEngine;
	
	public class ModuleGeneratorEditorWindow : EditorWindow {

		[MenuItem("Tools/ModuleGenerator")]
		private static void ShowWindow() {
			ModuleGeneratorEditorWindow window = EditorWindow.GetWindow<ModuleGeneratorEditorWindow>("Module generator", true);
			window.Show();
			window.OnValidate();
			window.maxSize = new Vector2(800, 600);
		}

		private string   _module_name;
		private string   _module_path;

		private bool _model_exist;
		private bool _is_model_additional_data_exist;
		private bool _view_exist;
		private bool _custom_installer;

		private string[] _modules_list_for_derived;
		private int      _derived_module_index = 0;
		
		
		
		private void OnValidate() {
			_modules_list_for_derived = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
												 .Where(type => type == typeof(ModuleController) || type.IsSubclassOf(typeof(ModuleController)))
												 .Select(type => {
													 string name             = type.Name;
													 int    controller_start = name.IndexOf("Controller");
													 string ready_name       = name.Substring(0, controller_start);
													 return ready_name;
												 }).ToArray();
		}

		private void OnGUI() {
			GUIStyle error_style = new GUIStyle {
				normal = new GUIStyleState {
					textColor = Color.red
				},
				margin = new RectOffset(5, 0,0,0)
			};
			
			GUIStyle normal_style = new GUIStyle {
				margin = new RectOffset(5, -250,0,0)
			};
			
			GUILayout.Label("Module generator", EditorStyles.boldLabel);

			GUILayout.Space(25);
			
			if (string.IsNullOrEmpty(_module_path)) {
				_module_path = "Assets/Scripts/com/rpdev/foundation/module";
			}

			_module_path = EditorGUILayout.TextField("Module path", _module_path);
			
			GUILayout.Space(5);
			
			_module_name = EditorGUILayout.TextField("Module name", _module_name);

			GUILayout.Space(5);
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Base class");
			_derived_module_index = EditorGUILayout.Popup(_derived_module_index, _modules_list_for_derived);
			EditorGUILayout.EndHorizontal();
			
			if (string.IsNullOrEmpty(_module_name)) {
				GUILayout.Label("Module name must be filled", error_style);
			}
			
			GUILayout.Space(15);

			_model_exist = EditorGUILayout.Toggle("Model", _model_exist);

			if (_model_exist) {
				_is_model_additional_data_exist = EditorGUILayout.Toggle("Additional model data", _is_model_additional_data_exist);	
			}
			
			GUILayout.Space(5);

			_view_exist  = EditorGUILayout.Toggle("View", _view_exist);

			GUILayout.Space(5);

			_custom_installer = EditorGUILayout.Toggle("Custom installer", _custom_installer);

			GUILayout.Space(15);

			EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(_module_name) || string.IsNullOrEmpty(_module_path));
			if (GUILayout.Button("Generate module")) {
				
				ModuleGenerator generator = new ModuleGenerator(new ModuleGenerator.ModuleSettings {
					module_name         = _module_name,
					module_path         = _module_path,
					is_custom_installer = _custom_installer,
					is_have_model       = _model_exist,
					is_havel_view       = _view_exist,
					is_additional_custom_data = _is_model_additional_data_exist,
					derived_module = _modules_list_for_derived[_derived_module_index]
				});
				
				generator.Generate();
				OnValidate();
			}
			EditorGUI.EndDisabledGroup();
			
		}
	}
#endif
}