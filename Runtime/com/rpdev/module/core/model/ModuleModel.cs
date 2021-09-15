using Zenject;

namespace com.rpdev.foundation.module.core.model {

	public interface IModuleModel {
		
	}
	
	public abstract class ModuleModel : IModuleModel, IInitializable {
		
		private ModuleAdditionalModelData _additional_data;

		protected T AdditionalData<T>() where T : ModuleAdditionalModelData {
			return _additional_data as T;
		}

		[Inject]
		private void InjectDependencies([InjectOptional] ModuleAdditionalModelData additional_data) {
			_additional_data = additional_data;
		}

		[Inject]
		public abstract void Initialize();
	}
}