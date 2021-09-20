using com.rpdev.foundation.module.core.view;
using UnityEngine;
using Zenject;

public static class ViewCustomFactory<TView> where TView : ModuleView {

    public static TView CreateViewFromPrefab(DiContainer container, TView prefab, InitialModuleViewData initial_data) {
        
        TView     view           = container.InstantiatePrefabForComponent<TView>(prefab);
        Transform view_transform = view.transform;
			
        view_transform.SetParent(initial_data.parent_container);
        view_transform.localPosition = initial_data.position;
        view_transform.localRotation = initial_data.rotation;
			
        return view;
    }
}
