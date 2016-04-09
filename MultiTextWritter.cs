using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountDistinctWords
{
    public class MultiTextWriter : TextWriter
    {
        private readonly IEnumerable<TextWriter> _writers;

        public MultiTextWriter(IEnumerable<TextWriter> writers)
        {
            this._writers = writers.ToList();
        }
        public MultiTextWriter(params TextWriter[] writers)
        {
            this._writers = writers;
        }

        public override void Write(char value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        public override void Write(string value)
        {
            foreach (var writer in _writers)
                writer.Write(value);
        }

        public override void Flush()
        {
            foreach (var writer in _writers)
                writer.Flush();
        }

        public override void Close()
        {
            foreach (var writer in _writers)
                writer.Close();
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
}
