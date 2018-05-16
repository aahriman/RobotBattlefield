using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BaseLibrary.utils {
    public static class ModUtils {

        /// <summary>
        /// Call static constructors for every class which is .dll in Plugins dictionary which have attribute <code>ModDescription</code> 
        /// </summary>
        /// <seealso cref="ModDescription"/>
        public static void LoadMods() {
            
            Load(Assembly.GetCallingAssembly());
            Load(Assembly.GetExecutingAssembly());
            Load(Assembly.GetEntryAssembly());
            try {
                Load(GetAllAssemblyInDir(AppDomain.CurrentDomain.BaseDirectory));
                Load(GetAllAssemblyInDir("."));
            } catch (Exception) {
                Console.Error.WriteLine("Error during loading dll. Application will be closed.");
                Thread.Sleep(5000);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Support recursive go through
        /// </summary>
        /// <param name="file"></param>
        public static void LoadFrom(string file) {
            if (Directory.Exists(file)) {
                foreach (var innerFile in Directory.GetFiles(file)) {
                    LoadFrom(innerFile);
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

        public static void Load(Assembly[] assemblies) {
            foreach (var assembly in assemblies) {
                Load(assembly);
            }
        }

        public static Assembly[] GetAllAssemblyInDir(string dir) {
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

        /// <summary>
        /// Deserialize object in jArray. Every element have to be jArray or have attribute "TYPE_NAME" with <code>GetType().Name</code>
        /// </summary>
        /// <param name="jArray"></param>
        /// <returns></returns>
        public static Object[] DeserializeMoreObjects(JArray jArray) {
            Object[] ret = new object[jArray.Count];
            for (int i = 0; i < jArray.Count; i++) {
                if (jArray[i].Type == JTokenType.Array) {
                    ret[i] = DeserializeMoreObjects((JArray) jArray[i]);
                } else {
                    ret[i] = JsonConvert.DeserializeObject(jArray[i].ToString(), GetType(jArray[i]["TYPE_NAME"].ToString()));
                }
            }
            return ret;
        }

        public static Type GetType(string typeName) {
            Type type = Type.GetType(typeName);
            if (type != null)
                return type;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
                type = asm.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}