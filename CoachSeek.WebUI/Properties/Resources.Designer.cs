﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoachSeek.WebUI.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CoachSeek.WebUI.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This email address is already in use..
        /// </summary>
        internal static string ErrorBusinessAdminDuplicateEmail {
            get {
                return ResourceManager.GetString("ErrorBusinessAdminDuplicateEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This location already exists..
        /// </summary>
        internal static string ErrorDuplicateLocation {
            get {
                return ResourceManager.GetString("ErrorDuplicateLocation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This business does not exist..
        /// </summary>
        internal static string ErrorInvalidBusiness {
            get {
                return ResourceManager.GetString("ErrorInvalidBusiness", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This location does not exist..
        /// </summary>
        internal static string ErrorInvalidLocation {
            get {
                return ResourceManager.GetString("ErrorInvalidLocation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing business registration data..
        /// </summary>
        internal static string ErrorNoBusinessRegistrationData {
            get {
                return ResourceManager.GetString("ErrorNoBusinessRegistrationData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing location data..
        /// </summary>
        internal static string ErrorNoLocationAddData {
            get {
                return ResourceManager.GetString("ErrorNoLocationAddData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing location data..
        /// </summary>
        internal static string ErrorNoLocationUpdateData {
            get {
                return ResourceManager.GetString("ErrorNoLocationUpdateData", resourceCulture);
            }
        }
    }
}
