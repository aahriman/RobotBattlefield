package BaseLibrary.utils;

import java.io.File;
import java.io.FileFilter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Enumeration;
import java.util.List;
import java.util.jar.JarEntry;
import java.util.jar.JarFile;

public final class ModUtils {

	private static String fileSeparator = System.getProperty("file.separator");
	private static String[] classPaths = System.getProperty("java.class.path").split(";");

	private ModUtils() {
	}

	/**
	 * Call static constructors for every class which is .dll in Plugins dictionary
	 * which have attribute <code>ModDesciption</code>
	 */
	public static void loadMods() {
		FileFilter filefilterJar = new FileFilter() {

			@Override
			public boolean accept(File pathname) {
				return pathname.getName().endsWith(".class");
			}
		};

		File folder = new File("./Plugins");
		if (folder.exists() && folder.isDirectory()) {
			File[] files = folder.listFiles(filefilterJar);
			for (File file : files) {
				load(file);
			}
		}
	}

	public static void load(File file) {
		if (!file.exists() || !file.getName().endsWith(".class"))
			return;

		ClassLoader classLoader = ClassLoader.getSystemClassLoader();
		try {
			classLoader.loadClass(file.getName());
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		}
	}

	public static List<Class<?>> loadClassFromHome() {
		List<Class<?>> list = new ArrayList<>();
		for (String classPath : classPaths) {
			list.addAll(loadClassFromFile(classPath, ""));
		}
		return list;
	}

	public static List<Class<?>> loadClassFromFile(String classPath, String packageName) {
		Holder<String> holder = new Holder<>();
		
		List<Class<?>> classes = new ArrayList<>();
		if (classPath.endsWith("jar")) {
			try {
				try (JarFile jar = new JarFile(classPath)){
					Enumeration<JarEntry> entries = jar.entries();
					while(entries.hasMoreElements()) {
						JarEntry e = entries.nextElement();
						if (classNameFromFile(e, holder) && holder.value.startsWith(packageName)) {
							Class<?> c = loadClass(holder.value);
							if (c != null) {
								classes.add(c);
							}
						}
					}
				}
			} catch (IOException e) {
				e.printStackTrace();
				return new ArrayList<Class<?>>();
			}
		} else {
			File file = new File(classPath + fileSeparator + packageName.replace(".", fileSeparator));
			List<File> files;
			if (file.isDirectory()) {
				files = loadFilesFromDictionary(file);
			} else {
				files = new ArrayList<>();
				files.add(file);
			}

			
			for (File f : files) {
				if (classNameFromFile(f, holder)) {
					Class<?> c = loadClass(holder.value);
					if (c != null) {
						classes.add(c);
					}
				}
			}
		}

		return classes;
	}

	public static List<Class<?>> loadClassFromPackage(String packageName) {
		List<Class<?>> list = new ArrayList<>();
		for (String classPath : classPaths) {
			
			list.addAll(loadClassFromFile(classPath, packageName));
		}
		return list;
	}

	public static List<File> loadFilesFromDictionary(File directory) {
		List<File> list = new ArrayList<>();

		for (File f : directory.listFiles()) {
			if (f.isDirectory()) {
				list.addAll(loadFilesFromDictionary(f));
			} else {
				list.add(f);
			}
		}
		return list;
	}

	private static boolean classNameFromFile(File file, Holder<String> holder) {
		String absolutePath = file.getAbsolutePath();
		for (String classPath : classPaths) {
			classPath += fileSeparator;
			if (absolutePath.startsWith(classPath) && absolutePath.endsWith(".class")) {
				holder.value = absolutePath.substring(classPath.length(), absolutePath.lastIndexOf(".class"))
						.replace(fileSeparator, ".");
				return true;
			}
		}
		return false;
	}

	private static boolean classNameFromFile(JarEntry file, Holder<String> holder) {
		String absolutePath = file.getName();

		if (absolutePath.endsWith(".class")) {
			holder.value = absolutePath.substring(0, absolutePath.lastIndexOf(".class")).replace("/", ".");
			return true;
		}

		return false;
	}
	
	private static Class<?> loadClass(String classFullName) {
		try {
			return Class.forName(classFullName);
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
			return null;
		} catch (RuntimeException e) {
			e.printStackTrace();
			return null;
		}
	}
}
