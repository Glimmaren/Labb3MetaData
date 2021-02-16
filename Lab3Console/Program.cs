using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab3Console
{
    class Program
    {
        static void Main(string[] args)
        {

            string path;
            string pathFull;
            byte[] fileInByte;
            List<Chunk> chunks = new List<Chunk>();

            Console.WriteLine("Enter file path with or without extension: ");
            path = Console.ReadLine();

            pathFull = GetFile(path);

            fileInByte = FileToByte(pathFull);

            FileTypeFromBin(fileInByte);

            chunks = GetChunks(fileInByte);

            for (int i = 0; i < chunks.Count; i++)
            {
                Console.WriteLine(chunks[i].ToString());
            }
            
            static byte[] FileToByte(string pathFull)
            {
                byte[] fileInByte;

                try
                {
                    fileInByte = File.ReadAllBytes(pathFull);

                    return fileInByte;
                }
                catch (ArgumentException)
                {
                 
                }

                return null;
            }

            static string GetFile(string path) 
            {
                string filename;
                string fullPath;

                try
                {
                    if (path.Substring(path.Length - 3).Equals('.'))
                    {
                        filename = path.Substring(path.LastIndexOf('\\') + 1, path.LastIndexOf('.'));
                    }
                    else
                    {
                        filename = path.Substring(path.LastIndexOf('\\') + 1);
                        filename += ".*";

                        path = path.Substring(0, path.LastIndexOf('\\') + 1);
                    }

                    fullPath = System.IO.Directory.GetFiles(path, filename).First();

                    return fullPath;

                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("File not found!!!");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid path!!!!");
                }

                return string.Empty;
            }

            static void FileTypeFromBin(byte[] file)
            {
                int resWidth;
                int resHight;

                if (file != null)
                {
                    if (file[0] == 66 && file[1] == 77)
                    {
                        resWidth = BitConverter.ToInt32(file, 18);
                        resHight = BitConverter.ToInt32(file, 22);

                        Console.WriteLine($"\nThis is an bmp file whith the resolution of {resWidth}x{resHight}");
                    }
                    else if (file[1] == 80 && file[2] == 78 && file[3] == 71)
                    {
                        byte[] tempW = { file[16], file[17], file[18], file[19] };
                        byte[] tempH = { file[20], file[21], file[22], file[23] };

                        string hexW = BitConverter.ToString(tempW).Replace("-", string.Empty);
                        string hexH = BitConverter.ToString(tempH).Replace("-", string.Empty);

                        resWidth = Convert.ToInt32(hexW, 16);
                        resHight = Convert.ToInt32(hexH, 16);

                        Console.WriteLine($"\nThis is an PNG file whith the resolution of {resWidth}x{resHight}");                       
                    }
                    else
                    {
                        Console.WriteLine("This file is not a bmp or a png file!");
                    }
                }

            }

            static List<Chunk> GetChunks(byte[] byteChar)
            {
                List<Chunk> chunks = new List<Chunk>();
                string[] chunkers = { "IHDR", "PLTE", "IDAT", "IEND", "TRNS", "CHRM", "GAMA", "ICCP", "SBIT", "SRGB", "ITXT", "TEXT", "ZTXT", "BKGB", "PHYS", "SPLT", "TIME" };
                string checkLabel = "", chunkDataString, tempHex;
                int chunkNr = 0, size;
                bool isChunk;


                if (byteChar != null)
                {
                    for (int i = 0; i < byteChar.Length - 4; i++)
                    {
                        byte[] temp = { byteChar[i], byteChar[i + 1], byteChar[i + 2], byteChar[i + 3] };
                        tempHex = BitConverter.ToString(temp).Replace("-", string.Empty);

                        for (int j = 0; j < tempHex.Length; j += 2)
                        {
                            checkLabel += (char)Int16.Parse(tempHex.Substring(j, 2), NumberStyles.AllowHexSpecifier);
                        }

                        isChunk = chunkers.Contains(checkLabel, StringComparer.OrdinalIgnoreCase);

                        if (isChunk)
                        {
                            chunkNr++;
                            byte[] tempSize = { byteChar[i - 4], byteChar[i - 3], byteChar[i - 2], byteChar[i - 1] };
                            tempHex = BitConverter.ToString(tempSize).Replace("-", string.Empty);
                            chunkDataString = BitConverter.ToString(tempSize);
                            size = Convert.ToInt32(tempHex, 16);
                            chunks.Add(new Chunk(chunkNr, checkLabel, size, chunkDataString));
                        }

                        checkLabel = "";
                    } 
                }
            
                return chunks;
            }

            Console.WriteLine("Hit any key......");
            Console.Read();
            
        }
    }
}
