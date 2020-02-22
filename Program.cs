using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.MD;
namespace DotNetPE
{
    class Program
    {


        static void Main(string[] args)
        {
            string dllPath = String.Empty;
            if (args.Length > 0) {
                dllPath = args[0];
                if (File.Exists(dllPath))
                {
                ModuleContext context = ModuleDef.CreateModuleContext();
                ModuleDefMD module = ModuleDefMD.Load(dllPath);
                Metadata metadata= module.Metadata;
                //MetaHeader 
                MetadataHeader metadataHeader= metadata.MetadataHeader;
                //存储流Header数量
                int count=metadataHeader.StreamHeaders.Count;
                //Cor20头
                ImageCor20Header imageCor20Header = metadata.ImageCor20Header;
                int cbOffeset = 0x208; //cb偏移
                int Metadata_VirtualAddress = (int)imageCor20Header.Metadata.VirtualAddress;//Metadata RVA
                int MetadataOffeset = Metadata_VirtualAddress - 0x1E00;//metadata文件偏移
                int stringHeaderSize = 0;//stringHeader size
                int iMajorVerOffeset = MetadataOffeset + 0x4;
                int iMinorVerOffeset = iMajorVerOffeset+0x2;
                int VersionStringLOffeset = MetadataOffeset+0xC;
                int stringHeaderOffeset = MetadataOffeset + 0x30;
                int pe1 = 0x82;
                int pe2 = 0x83;
                int nrs = 0xF4;
                for (int i = 0; i < count; i++)
                {
                    StreamHeader header = metadataHeader.StreamHeaders[i];
                    if (header.Name.Contains("Strings"))
                    {
                       stringHeaderSize = (int)header.StreamSize;
                    }
                    
                }
                    byte[] buffer= File.ReadAllBytes(dllPath);
                    buffer[pe1] = 0x2E;
                    buffer[pe2] = 0x2E;
                    buffer[nrs] = 0xF;
                    buffer[cbOffeset] = 0x88;
                    buffer[iMajorVerOffeset] = 0x2;
                    buffer[iMinorVerOffeset] = 0x2;
                    buffer[VersionStringLOffeset] = 0xB;
                    byte temp = buffer[stringHeaderOffeset];
                    buffer[stringHeaderOffeset] = buffer[stringHeaderOffeset + 1];
                    buffer[stringHeaderOffeset + 1] = buffer[stringHeaderOffeset + 2];
                    buffer[stringHeaderOffeset + 2] = temp;
                    string savePath = dllPath.Replace(".dll", "_encrypt.dll");
                    File.WriteAllBytes(savePath,buffer);
                Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("文件不存在!\n按任意键退出!");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("文件输入有误!\n按任意键退出!");
                Console.ReadLine();
            }

        }
    }
}
