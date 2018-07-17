using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;

namespace TcpMonitor
{
    public class InterfaceCreator
    {
        public List<string> Methods { get; set; }
        public List<string> Properties { get; set; }
        public string InterfaceName { get; set; }

        private CodeCompileUnit _CompileUnit;
        private CodeNamespace _CodeNamespace;
        private CodeTypeDeclaration _Interface;

        public InterfaceCreator(string interfaceName, List<string> methods, List<string> properties)
        {
            Methods = methods;
            Properties = properties;
            InterfaceName = interfaceName;

            _CompileUnit = new CodeCompileUnit();
            _CodeNamespace = new CodeNamespace("DynamicallyGeneratedInterfaces");
            _Interface = new CodeTypeDeclaration(interfaceName);
            _Interface.IsInterface = true;
        }

        private Dictionary<string, Type> _systemTypes = new Dictionary<string, Type>()
        {
            {"System.Boolean",  typeof(bool)},
            {"System.Byte", typeof(byte) },
            {"System.SByte", typeof(sbyte) },
            {"System.Char", typeof(char) },
            {"System.Decimal", typeof(decimal) },
            {"System.Double", typeof(double) },
            {"System.Single", typeof(float) },
            {"System.Int32", typeof(int) },
            {"System.UInt32", typeof(uint) },
            {"System.Int64", typeof(long) },
            {"System.UInt64", typeof(ulong) },
            {"System.Object", typeof(object) },
            {"System.Int16", typeof(short) },
            {"System.UInt16", typeof(ushort) },
            {"System.String", typeof(string) },
            {"Void", typeof(void) },
            {"Boolean",  typeof(bool)},
            {"Byte", typeof(byte) },
            {"SByte", typeof(sbyte) },
            {"Char", typeof(char) },
            {"Decimal", typeof(decimal) },
            {"Double", typeof(double) },
            {"Single", typeof(float) },
            {"Int32", typeof(int) },
            {"UInt32", typeof(uint) },
            {"Int64", typeof(long) },
            {"UInt64", typeof(ulong) },
            {"Object", typeof(object) },
            {"Int16", typeof(short) },
            {"UInt16", typeof(ushort) },
            {"String", typeof(string) }
        };

        public void CreateInterfaceAssembly()
        {
            try
            {
                AddInterfaceMethods();
                AddInterfaceProperties();

                _CodeNamespace.Types.Add(_Interface);
                _CompileUnit.Namespaces.Add(_CodeNamespace);
                var csprov = new CSharpCodeProvider();
                ICodeCompiler icc = csprov.CreateCompiler();
                string outputName = $"{InterfaceName}.dll";
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.OutputAssembly = outputName;
                CompilerResults results = icc.CompileAssemblyFromDom(parameters, _CompileUnit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void AddInterfaceMethods()
        {
            foreach (var method in Methods)
            {
                string[] splitMethod = method.Split(' ');
                string returnTypeString = splitMethod[0];
                string signature = splitMethod[1];
                string methodName = signature.Split('(')[0];
                string paraMeters = method.Split('(')[1];
                paraMeters = paraMeters.Remove(paraMeters.Length - 1);

                // exclude propertie methods.
                if (!signature.StartsWith("get_") && !signature.StartsWith("set_"))
                {
                    var mth = new CodeMemberMethod();
                    mth.Name = methodName;
                    mth.ReturnType = new CodeTypeReference(_systemTypes[returnTypeString]);

                    if (paraMeters.Length > 0)
                    {
                        string[] parameterArr = paraMeters.Split(',');
                        foreach (var paraStr in parameterArr)
                        {
                            string p = paraStr.Trim();
                            Console.WriteLine();
                            string type = p.Split(' ')[0];
                            string name = p.Split(' ')[1];

                            mth.Parameters.Add(new CodeParameterDeclarationExpression(_systemTypes[type], name));
                        }
                    }
                    _Interface.Members.Add(mth);
                }
            }
        }

        private void AddInterfaceProperties()
        {
            foreach (var propStr in Properties)
            {
                string[] propArr = propStr.Split(' ');
                string propTypeStr = propArr[0];
                string propNameStr = propArr[1];

                bool hasSetter = Methods.Contains($"{propTypeStr} get_{propNameStr}()");
                bool hasGetter = Methods.Contains($"Void set_{propNameStr}({propTypeStr} value)");

                var prop = new CodeMemberProperty();
                prop.Name = propNameStr;
                prop.HasSet = hasSetter;
                prop.HasGet = hasGetter;
                prop.Type = new CodeTypeReference(_systemTypes[propTypeStr]);

                _Interface.Members.Add(prop);
            }
        }
    }
}
