﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArchBench.PlugIns.Events {
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
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ArchBench.PlugIns.Events.Resource", typeof(Resource).Assembly);
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
        ///   Looks up a localized string similar to &lt;!DOCTYPE HTML&gt;
        ///
        ///&lt;html&gt;
        ///
        ///&lt;head&gt;
        ///	&lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=UTF-8&quot; /&gt;
        ///    &lt;title&gt;HTML Server Sent Events&lt;/title&gt;
        ///&lt;/head&gt;
        ///
        ///&lt;body&gt;
        ///    &lt;script type=&quot;text/javascript&quot;&gt;
        ///        function changeText() {
        ///            document.getElementById(&apos;boldStuff&apos;).innerHTML = &apos;Fred Flinstone&apos;;
        ///        }
        ///    &lt;/script&gt;
        ///    &lt;p&gt;Welcome to the site &lt;b id=&apos;boldStuff&apos;&gt;dude&lt;/b&gt; &lt;/p&gt; 
        ///    &lt;input type=&apos;button&apos; onclick=&apos;changeText()&apos; value=&apos;Change Text&apos;/&gt;
        ///
        ///
        ///    &lt;div id=&quot;sse&quot;&gt;
        ///       &lt; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string index {
            get {
                return ResourceManager.GetString("index", resourceCulture);
            }
        }
    }
}
