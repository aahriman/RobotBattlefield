using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.protocol;

namespace BaseLibrary.utils {
    public static class ModUtils {

        /// <summary>
        /// Call static constructors for every class which is .dll in Plugins dictionary which have attribute <code>ModDescription</code> 
        /// </summary>
        /// <seealso cref="ModDescription"/>
        public static void LoadMods() {
            Load(Assembly.GetCallingAssembly());
            if (Directory.Exists("./")) {
                string[] files = Directory.GetFiles("./", "*.dll");
                foreach (var s in files) {
                    LoadFrom(s);
                }
                files = Directory.GetFiles("./", "*.exe");
                foreach (var s in files) {
                    LoadFrom(s);
                }
            }
        }

        /// <summary>
        /// Support recursive go through
        /// </summary>
        /// <param name="file"></param>
        public static void LoadFrom(string file) {
            if (Directory.Exists(file)) {
                foreach (var innetFile in Directory.GetFiles(file)) {
                    LoadFrom(innetFile);
                }
            } else {
                Assembly assembly = LoadAssembly(file);
                if (assembly != null) {
                    Load(assembly);
                }
            }
        }

        public static Assembly LoadAssembly(string file) {
            if (!File.Exists(file)) {
                return null;
            }

            if (!file.EndsWith(".dll", true, null) && file.EndsWith(".exe", true, null)) {
                return null;
            }
            
            return Assembly.LoadFile(Path.GetFullPath(file));
        }

        public static void Load(Assembly assembly) {
            Type[] modTypes = (Type[]) assembly
                .GetTypes()
                .Where(t =>
                           t.IsClass &&
                           t.GetCustomAttribute(typeof(ModDescription), false) != null)
                .ToArray();
            foreach (Type modType in modTypes) {
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(modType.TypeHandle);
            }
        }

        public static Assembly[] LoadAllAssemblyInDir(string dir) {
            if (Directory.Exists(dir)) {
                List<Assembly> assemblies = new List<Assembly>();
                string[] files = Directory.GetFiles(dir, "*.dll");
                foreach (var file in files) {
                    assemblies.Add(LoadAssembly(file));
                }
                return assemblies.ToArray();
            }
            return new Assembly[0];
        }
    }
}