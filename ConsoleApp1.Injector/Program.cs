using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            // ConsoleApp1.exe と ClassLibrary1.dll があるディレクトリ
            var baseDir = @".\";

            // 対象となるアセンブリと型、メソッド名
            var targetAssemblyName = "ConsoleApp1.exe";
            var targetTypeName = "ConsoleApp1.Program";
            var targetMethodName = "Main";

            // 差し込みたいアセンブリと型、メソッド名
            var injectAssemblyName = "ClassLibrary1.dll";
            var injectTypeName = "ClassLibrary1.Class1";
            var injectMethodName = "Initialize";

            // 出力アセンブリ名
            var outputAssembly = "ConsoleApp1_Injected.exe";

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(baseDir);

            var asm = AssemblyDefinition.ReadAssembly(
                Path.Combine(baseDir, targetAssemblyName),
                new ReaderParameters
                {
                    AssemblyResolver = assemblyResolver,
                }
            );
            var targetType = asm.MainModule.GetType(targetTypeName);
            var targetMethod = targetType.Methods.First(x => x.Name == targetMethodName);

            var asmInject = AssemblyDefinition.ReadAssembly(Path.Combine(baseDir, injectAssemblyName));
            var injectType = asmInject.MainModule.GetType(injectTypeName);
            var injectMethod = injectType.Methods.First(x => x.IsStatic && x.Name == injectMethodName);
            var injectMethodRef = targetMethod.Module.Import(injectMethod);

            var ilProcessor = targetMethod.Body.GetILProcessor();
            ilProcessor.InsertBefore(targetMethod.Body.Instructions[0], Instruction.Create(OpCodes.Call, injectMethodRef));

            asm.Write(Path.Combine(baseDir, outputAssembly));
        }
    }
}
