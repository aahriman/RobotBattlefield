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
        /// Call static constructors for every class which is .dll in Plugins dictionary which have attribute <code>ModDesciption</code> 
        /// </summary>
        public static void LoadMods() {
            if(Directory.Exists("./Plugins")) {
                String[] files = Directory.GetFiles("./Plugins/", "*.dll");
                foreach (var s in files) {
                    Load(Path.Combine(Environment.CurrentDirectory, s));
                }
            }
        }

        public static void Load(String file) {
            if (!File.Exists(file) || !file.EndsWith(".dll", true, null))
                return;

            Assembly assembly = Assembly.LoadFile(file);

            Type [] modTypes = (Type[]) assembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    t.GetCustomAttribute(typeof(ModDescription)) != null)
                .ToArray();
            foreach (Type modType in modTypes) {
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(modType.TypeHandle);
            }
        }
    }
}
