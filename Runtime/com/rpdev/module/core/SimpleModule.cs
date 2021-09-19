using com.rpdev.foundation.module.core.controller;
using UnityEngine;

public class SimpleModule : MonoBehaviour, IModuleFacade {
    
    public virtual void Activate() {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate() {
        gameObject.SetActive(false);
    }

    public virtual void Dispose() {
        
    }
}
