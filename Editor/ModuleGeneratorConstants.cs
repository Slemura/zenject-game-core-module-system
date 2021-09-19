using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModuleGeneratorConstants {
    
    public const string CONTROLLER_TEMPLATE      = "templates\\ModuleController.tt";
    public const string MODEL_TEMPLATE           = "templates\\ModuleModel.tt";
    public const string VIEW_TEMPLATE            = "templates\\ModuleView.tt";
    public const string INSTALLER_TEMPLATE       = "templates\\ModuleInstaller.tt";
    public const string ADDITIONAL_DATA_TEMPLATE = "templates\\ModuleAdditionalData.tt";
    
    public const  string VIEW_PROPERTY                   = "protected I#ModuleName#View  View  => View<I#ModuleName#View>();";
    public const  string MODEL_PROPERTY                  = "protected I#ModuleName#Model Model => Model<I#ModuleName#Model>();";

    public const string ADDITIONAL_DATA_PROPERTY = "public #ModuleName#AdditionalData AdditionalData => AdditionalData<#ModuleName#AdditionalData>();";
    public const string ADDITIONAL_DATA_INTERFACE_PROPERTY = "#ModuleName#AdditionalData AdditionalData { get; }";

    public const string CONTROLLER_FACADE_INTERFACE_NAME = "I#ModuleName#Facade";
    public const string CONTROLLER_FACADE_INTERFACE_BODY = "public interface I#ModuleName#Facade : I#DerivedModule#Facade { }";
    
    public const string CONTROLLER_INTERFACE_NAME = "I#ModuleName#Controller";
    public const string CONTROLLER_INTERFACE_BODY = "public interface I#ModuleName#Controller : I#DerivedModule#Controller { }";
}
