using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;


namespace csReflexe
{
    class Program
    {
        static void Main(string[] args)
        {
            Execute("/Customer/Add?Name=Pepa&Age=30&IsActive=true");
            Execute("/Customer/List?limit=10");
        }

        private static void Execute(string url)
        {
            var mid = url.Split('?');
            var left = mid[0].Split('/');
            var right = mid[1].Split('&');

            Dictionary<string, string> queryParams = new Dictionary<string, string>();

            foreach (var param in right)
            {
                var keyValue = param.Split('=');
                queryParams.Add(keyValue[0], keyValue[1]);
            }


            Assembly assembly = Assembly.LoadFile(Path.GetFullPath("../../../../MyLib/bin/Debug/netcoreapp3.1/MyLib.dll"));

            Type controllerType = assembly.GetType($"MyLib.Controllers.{left[1]}Controller"); //Reference 
            if (controllerType == null)
            {
                Console.WriteLine("Stranka nenalezena");
                return;
            }

            object controller = Activator.CreateInstance(controllerType);

            MethodInfo listMethod = controllerType.GetMethod(left[2]);
            if (listMethod == null || listMethod.ReturnType != typeof(string))
            {
                Console.WriteLine("Stranka nenalezena");
                return;
            }

            ParameterInfo[] parameters = listMethod.GetParameters();
            object[] arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsClass)
                {
                    object parObject = Activator.CreateInstance(parameters[i].ParameterType);
                    PropertyInfo[] props = parameters[i].ParameterType.GetProperties();

                    foreach (PropertyInfo prop in props)
                    {
                        string value = queryParams.ContainsKey(prop.Name) ? queryParams[prop.Name] : null;

                        bool ignore = prop.GetCustomAttributes().Any(x => x.GetType().Name == "IgnoreAttribute");

                        if (ignore)
                        {
                            continue;
                        }
                        if (value == null)
                        {
                            continue;

                        }

                        if (prop.PropertyType == typeof(int))
                        {
                            prop.SetValue(parObject, int.Parse(value));
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            prop.SetValue(parObject, value);
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            prop.SetValue(parObject, bool.Parse(value));
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }

                    arguments[i] = parObject;
                }
                else
                {
                    string value = queryParams.ContainsKey(parameters[i].Name) ? queryParams[parameters[i].Name] : null;

                    if (parameters[i].ParameterType == typeof(int))
                    {
                        arguments[i] = int.Parse(value);
                    }
                    else if (parameters[i].ParameterType == typeof(string))
                    {
                        arguments[i] = value;
                    }
                    else if (parameters[i].ParameterType == typeof(bool))
                    {
                        arguments[i] = bool.Parse(value);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            string result = (string) listMethod.Invoke(controller, arguments); //parametry te funkce LIST

            Console.WriteLine(result);
        }
    }
}
