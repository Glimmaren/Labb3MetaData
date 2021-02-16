using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3Console
{
    public class Chunk
    {
        public int ChunkNr { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Data { get; set; }


        public Chunk(int chunkNr, string name, int size, string data)
        {
            ChunkNr = chunkNr;
            Name = name;
            Size = size;
            Data = data;
        }

        public override string ToString()
        {
            if (Size > 1500)
            {
               
                return $"\n=====Chunk=====\nChunk #: {ChunkNr}\nChunk Type: {Name}\nSize: {(Size/1000)} kB, {Data}\n===============";
            }
            else
            {
                return $"\n=====Chunk=====\nChunk #: {ChunkNr}\nChunk Type: {Name}\nSize: {Size} bytes, {Data}\n===============";
            }
        }
    }
}
